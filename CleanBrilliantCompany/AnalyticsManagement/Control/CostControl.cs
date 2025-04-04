public class CostControl 
{
    private readonly ApplicationDbContext dbContext;
    private List<CostDashboardRdm> dashboards; 
    private CostMapper costMapper; 
    private ILogger<CostDashboardRdm>? logger;
    private IAlertService? alertService;
    public ApplicationDbContext DbContext => dbContext;

    public CostControl(
        CostMapper costMapper,
        ILogger<CostDashboardRdm> logger,
        // IVisualizationService visualizationService,
        IAlertService alertService,
        ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
        this.costMapper = costMapper;
        this.logger = logger;
        // this.visualizationService = visualizationService;
        this.alertService = alertService;
        this.dashboards = new List<CostDashboardRdm>();

        LoadDashboards(); 
    }

    private void LoadDashboards()
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
            if (dashboard == null)
            {
                throw new InvalidOperationException("DashboardFactory.createDashboard returned null.");
            }
            costDashboard = (CostDashboardRdm)dashboard;
            costDashboard.InitializeServices(logger!, alertService!);
        }
        else
        {
            var dto = costMapper.ToDTO(dashboardEntity);
            var dashboard = DashboardFactory.createDashboard(dto);
            if (dashboard == null)
            {
                throw new InvalidOperationException("DashboardFactory.createDashboard returned null.");
            }
            costDashboard = (CostDashboardRdm)dashboard;
            costDashboard.InitializeServices(logger!, alertService!);
        }

        var manufacturers = costMapper.GetAllManufacturers();
        var productBatches = costMapper.GetAllProductBatches();
        var items = costMapper.GetAllItems();

        costDashboard.ProcessManufacturers(manufacturers);
        costDashboard.ProcessProductBatches(productBatches);
        costDashboard.ProcessItems(items);
        

        dashboards.Add(costDashboard);
        logger!.LogInformation($"✅ Loaded dashboard: {costDashboard.Name}");
    }

    public CostDashboardRdm? GetLatestDashboard()
    {
        logger!.LogInformation("🔍 Retrieving the latest dashboard from memory...");
        return dashboards.OrderByDescending(d => d.GeneratedDate).FirstOrDefault();
    }

   public void generateNewDashboard(DashboardDTO dto) 
    {
        dto.Type = 4;

        var dashboardBase = DashboardFactory.createDashboard(dto);
        if (dashboardBase is not CostDashboardRdm costdashboard)
        {
            logger!.LogError("❌ Failed to create CostDashboardRdm from factory.");
            return;
        }
        costdashboard.InitializeServices(logger!, alertService!);
        

        var manufacturers = costMapper.GetAllManufacturers();
        var productBatches = costMapper.GetAllProductBatches();
        var items = costMapper.GetAllItems();

        costdashboard.ProcessManufacturers(manufacturers);
        costdashboard.ProcessProductBatches(productBatches);
        costdashboard.ProcessItems(items);

        var dashboardEntity = DashboardMapper.ToEntity(dto);
        dashboardEntity.GeneratedDate = DateTime.Now;
        dbContext.Dashboards.Add(dashboardEntity);
        dbContext.SaveChanges();

        dashboards.Add(costdashboard);

        logger!.LogInformation($"✅ New dashboard generated: {costdashboard.Name}");
    }
    
    public void UpdateDashboard()
    {
        var today = DateTime.Today;

        logger!.LogInformation($"🔄 Checking if a dashboard was already created today ({today})...");

        var dashboardDto = new DashboardDTO
        {
            Name = "Cost Dashboard",
            RequestedStartDate = DateTime.Today,
            RequestedEndDate = DateTime.Today,
            GeneratedDate = DateTime.Now,
            ValidityDuration = 1,
            Type = 4
        };

        var latestDashboard = dbContext.Dashboards
            .Where(d => d.TypeId == dashboardDto.Type)
            .OrderByDescending(d => d.GeneratedDate)
            .FirstOrDefault();

        if (latestDashboard != null && latestDashboard.GeneratedDate.Date == today)
        {
            logger!.LogInformation($"🗑 Dashboard with ID {latestDashboard.DashboardId} was generated today — deleting and updating...");

            dbContext.Dashboards.Remove(latestDashboard);
            dbContext.SaveChanges();

            var newDashboard = costMapper.ToEntity(dashboardDto);
            newDashboard.GeneratedDate = DateTime.Now;

            var dashboardBase = DashboardFactory.createDashboard(dashboardDto);
            if (dashboardBase is not CostDashboardRdm costDashboard)
            {
                logger!.LogError("❌ Failed to create CostDashboardRdm from factory.");
                return;
            }

            costDashboard.InitializeServices(logger!, alertService!);

            var manufacturers = costMapper.GetAllManufacturers();
            var productBatches = costMapper.GetAllProductBatches();
            var items = costMapper.GetAllItems();

            costDashboard.ProcessManufacturers(manufacturers);
            costDashboard.ProcessProductBatches(productBatches);
            costDashboard.ProcessItems(items);

            dashboards.Add(costDashboard);

            dbContext.Dashboards.Add(newDashboard);
            dbContext.SaveChanges();

            logger!.LogInformation($"✅ Dashboard replaced. New dashboard created with name: {newDashboard.Name}, GeneratedDate: {newDashboard.GeneratedDate}");
        }
        else
        {
            logger!.LogInformation("🆕 No dashboard created today — creating a new one...");

            var newDashboard = costMapper.ToEntity(dashboardDto);
            newDashboard.GeneratedDate = DateTime.Now;

            var dashboardBase = DashboardFactory.createDashboard(dashboardDto);
            if (dashboardBase is not CostDashboardRdm costDashboard)
            {
                logger!.LogError("❌ Failed to create CostDashboardRdm from factory.");
                return;
            }

            costDashboard.InitializeServices(logger!, alertService!);

            var manufacturers = costMapper.GetAllManufacturers();
            var productBatches = costMapper.GetAllProductBatches();
            var items = costMapper.GetAllItems();

            costDashboard.ProcessManufacturers(manufacturers);
            costDashboard.ProcessProductBatches(productBatches);
            costDashboard.ProcessItems(items);

            dashboards.Add(costDashboard);

            dbContext.Dashboards.Add(newDashboard);
            dbContext.SaveChanges();

            logger!.LogInformation($"✅ New dashboard created with name: {newDashboard.Name}, GeneratedDate: {newDashboard.GeneratedDate}");
        }
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
    report.AppendLine($"<p>Generated: {dashboard.GeneratedDate}</p>");
    report.AppendLine("<hr/>");

    // Average Batch Cost by Manufacturer
    report.AppendLine("<h2>Average Batch Cost by Manufacturer</h2>");
    var avgCosts = dashboard.GetAvgBatchCost(this.DbContext);
    report.AppendLine("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse: collapse;'>");
    report.AppendLine("<thead><tr><th>Manufacturer ID</th><th>Name</th><th>Average Cost</th></tr></thead><tbody>");
    foreach (var entry in avgCosts)
    {
        report.AppendLine($"<tr><td>{entry.ManufacturerId}</td><td>{entry.ManufacturerName}</td><td>${entry.AvgBatchCost:F2}</td></tr>");
    }
    report.AppendLine("</tbody></table>");

    // Supplier Comparison
    report.AppendLine("<h2>Supplier Batch Count</h2>");
    var supplierStats = dashboard.GetSupplierComparison(this.DbContext);
    report.AppendLine("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse: collapse;'>");
    report.AppendLine("<thead><tr><th>Manufacturer ID</th><th>Name</th><th>Batch Count</th></tr></thead><tbody>");
    foreach (var s in supplierStats)
    {
        report.AppendLine($"<tr><td>{s.ManufacturerId}</td><td>{s.ManufacturerName}</td><td>{s.BatchCount}</td></tr>");
    }
    report.AppendLine("</tbody></table>");

    // Summary
    report.AppendLine("<h2>Price Extremes Overview</h2>");
    var summary = dashboard.GetCheapestAndMostExpensiveOverview(this.DbContext);
    report.AppendLine("<ul>");
    report.AppendLine($"<li>Cheapest Batch Code: {summary.CheapestBatchCode}</li>");
    report.AppendLine($"<li>Most Expensive Batch Code: {summary.ExpensiveBatchCode}</li>");
    report.AppendLine($"<li>Cheapest Manufacturer ID: {summary.CheapestManufacturer}</li>");
    report.AppendLine($"<li>Most Expensive Manufacturer ID: {summary.ExpensiveManufacturer}</li>");
    report.AppendLine("</ul>");

    Console.WriteLine("Cost Report generated.");
    return report.ToString();
}

}