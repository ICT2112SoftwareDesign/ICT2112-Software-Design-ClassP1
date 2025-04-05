public class CostControl : ICostRepository
{
    private readonly ApplicationDbContext dbContext;
    private readonly List<CostDashboardRdm> dashboards;
    private readonly CostMapper costMapper;
    private readonly ILogger<CostDashboardRdm>? logger;
    private readonly IAlertService? alertService;

    public ApplicationDbContext DbContext => dbContext;

    public CostControl(
        CostMapper costMapper,
        ILogger<CostDashboardRdm> logger,
        IAlertService alertService,
        ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.costMapper = costMapper;
        this.logger = logger;
        this.alertService = alertService;
        this.dashboards = new List<CostDashboardRdm>();

        LoadDashboards();
    }

    public void LoadDashboards()
    {
        logger!.LogInformation("[DEBUG] Attempting to load dashboards from database...");

        var dashboardEntity = dbContext.Dashboards
            .Where(d => d.TypeId == 4)
            .OrderByDescending(d => d.RequestedStartDate)
            .FirstOrDefault();

        CostDashboardRdm costDashboard;

        if (dashboardEntity == null)
        {
            logger!.LogWarning("⚠ No dashboard found in database. Creating a new one.");

            var newDashboardDto = new DashboardDTO
            {
                Name = "New Cost Dashboard Generated",
                RequestedStartDate = DateTime.Now,
                RequestedEndDate = DateTime.Now,
                GeneratedDate = DateTime.Now,
                ValidityDuration = 1,
                Type = 4
            };

            var newEntity = DashboardMapper.ToEntity(newDashboardDto);
            dbContext.Dashboards.Add(newEntity);
            dbContext.SaveChanges();

            var dashboard = DashboardFactory.createDashboard(newDashboardDto);
            if (dashboard == null) throw new InvalidOperationException("DashboardFactory.createDashboard returned null.");
            costDashboard = (CostDashboardRdm)dashboard;
            costDashboard.InitializeServices(logger!, alertService!);
        }
        else
        {
            var dto = costMapper.ToDTO(dashboardEntity);
            var dashboard = DashboardFactory.createDashboard(dto);
            if (dashboard == null) throw new InvalidOperationException("DashboardFactory.createDashboard returned null.");
            costDashboard = (CostDashboardRdm)dashboard;
            costDashboard.InitializeServices(logger!, alertService!);
        }

        costDashboard.ProcessManufacturers(costMapper.GetAllManufacturers());
        costDashboard.ProcessProductBatches(costMapper.GetAllProductBatches());
        costDashboard.ProcessItems(costMapper.GetAllItems());

        dashboards.Add(costDashboard);
        logger!.LogInformation($"✅ Loaded dashboard: {costDashboard.Name}");
    }

    public CostDashboardRdm? GetLatestDashboard()
    {
        logger!.LogInformation("🔍 Retrieving the latest dashboard from memory...");
        return dashboards.OrderByDescending(d => d.GeneratedDate).FirstOrDefault();
    }

    public void GenerateNewDashboard(DashboardDTO dto)
    {
        dto.Type = 4;

        var dashboardBase = DashboardFactory.createDashboard(dto);
        if (dashboardBase is not CostDashboardRdm costDashboard)
        {
            logger!.LogError("❌ Failed to create CostDashboardRdm from factory.");
            return;
        }

        costDashboard.InitializeServices(logger!, alertService!);
        costDashboard.ProcessManufacturers(costMapper.GetAllManufacturers());
        costDashboard.ProcessProductBatches(costMapper.GetAllProductBatches());
        costDashboard.ProcessItems(costMapper.GetAllItems());

        var dashboardEntity = DashboardMapper.ToEntity(dto);
        dashboardEntity.GeneratedDate = DateTime.Now;
        dbContext.Dashboards.Add(dashboardEntity);
        dbContext.SaveChanges();

        dashboards.Add(costDashboard);
        logger!.LogInformation($"✅ New dashboard generated: {costDashboard.Name}");
    }

    public void UpdateDashboard()
    {
        var today = DateTime.Today;
        logger!.LogInformation($"🔄 Checking if a dashboard was already created today ({today})...");

        var dashboardDto = new DashboardDTO
        {
            Name = "Cost Dashboard",
            RequestedStartDate = today,
            RequestedEndDate = today,
            GeneratedDate = DateTime.Now,
            ValidityDuration = 1,
            Type = 4
        };

        var existing = dbContext.Dashboards
            .Where(d => d.TypeId == 4)
            .OrderByDescending(d => d.GeneratedDate)
            .FirstOrDefault();

        if (existing != null && existing.GeneratedDate.Date == today)
        {
            logger!.LogInformation($"🗑 Removing today's existing dashboard: ID {existing.DashboardId}");
            dbContext.Dashboards.Remove(existing);
            dbContext.SaveChanges();
        }

        var newEntity = costMapper.ToEntity(dashboardDto);
        newEntity.GeneratedDate = DateTime.Now;

        var dashboardBase = DashboardFactory.createDashboard(dashboardDto);
        if (dashboardBase is not CostDashboardRdm costDashboard)
        {
            logger!.LogError("❌ Failed to create CostDashboardRdm from factory.");
            return;
        }

        costDashboard.InitializeServices(logger!, alertService!);
        costDashboard.ProcessManufacturers(costMapper.GetAllManufacturers());
        costDashboard.ProcessProductBatches(costMapper.GetAllProductBatches());
        costDashboard.ProcessItems(costMapper.GetAllItems());

        dashboards.Add(costDashboard);
        dbContext.Dashboards.Add(newEntity);
        dbContext.SaveChanges();

        logger!.LogInformation($"✅ Dashboard updated: {newEntity.Name}, {newEntity.GeneratedDate}");
    }

    public string GenerateReport()
    {
        var dashboard = GetLatestDashboard();
        if (dashboard == null)
        {
            Console.WriteLine("Cost dashboard is null.");
            return "<p>No data found for cost dashboard.</p>";
        }

        var report = new System.Text.StringBuilder();

        report.AppendLine($"<h1>Cost Report - {dashboard.Name}</h1>");
        report.AppendLine($"<p>Generated: {dashboard.GeneratedDate}</p><hr/>");

        // Avg batch cost
        report.AppendLine("<h2>Average Batch Cost by Manufacturer</h2>");
        report.AppendLine("<table border='1'><tr><th>ID</th><th>Name</th><th>Avg Cost</th></tr>");
        foreach (var e in dashboard.GetAvgBatchCost(dbContext))
        {
            report.AppendLine($"<tr><td>{e.ManufacturerId}</td><td>{e.ManufacturerName}</td><td>${e.AvgBatchCost:F2}</td></tr>");
        }
        report.AppendLine("</table>");

        // Supplier batch count
        report.AppendLine("<h2>Supplier Batch Count</h2>");
        report.AppendLine("<table border='1'><tr><th>ID</th><th>Name</th><th>Count</th></tr>");
        foreach (var s in dashboard.GetSupplierComparison(dbContext))
        {
            report.AppendLine($"<tr><td>{s.ManufacturerId}</td><td>{s.ManufacturerName}</td><td>{s.BatchCount}</td></tr>");
        }
        report.AppendLine("</table>");

        // Summary
        var summary = dashboard.GetCheapestAndMostExpensiveOverview(dbContext);
        report.AppendLine("<h2>Price Extremes Overview</h2><ul>");
        report.AppendLine($"<li>Cheapest Batch: {summary.CheapestBatchCode}</li>");
        report.AppendLine($"<li>Most Expensive Batch: {summary.ExpensiveBatchCode}</li>");
        report.AppendLine($"<li>Cheapest Manufacturer: {summary.CheapestManufacturer}</li>");
        report.AppendLine($"<li>Most Expensive Manufacturer: {summary.ExpensiveManufacturer}</li>");
        report.AppendLine("</ul>");

        Console.WriteLine("Cost Report generated.");
        return report.ToString();
    }
}