public class ManufacturerMapper : ManufacturerRepo
{
    private readonly ApplicationDbContext _dbContext;

    public ManufacturerMapper(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Fetch the latest Manufacturer dashboard
    public DashboardDTO? GetLatestManufacturerDashboard()
    {
        var dashboard = _dbContext.Dashboards
            .Where(d => d.TypeId == 3)  // TypeId for Manufacturer Dashboard
            .OrderByDescending(d => d.GeneratedDate)
            .Select(d => new DashboardDTO
            {
                DashboardId = d.DashboardId,
                Name = d.Name,
                RequestedStartDate = d.RequestedStartDate,
                RequestedEndDate = d.RequestedEndDate,
                GeneratedDate = d.GeneratedDate,
                ValidityDuration = d.ValidityDuration,
                Type = d.TypeId
            })
            .FirstOrDefault();

        if (dashboard == null)
        {
            Console.WriteLine("⚠ No manufacturer dashboard found.");
        }
        return dashboard;
    }

    // Fetch ManufacturerMetrics from the corresponding table as a list
    public List<ManufacturerMetricsDTO> GetManufacturerMetrics(int dashboardId)
    {
        var metricsList = _dbContext.ManufacturerMetrics
            .Where(m => m.DashboardId == dashboardId)  // Filter by DashboardId
            .Select(m => new ManufacturerMetricsDTO
            {
                MetricId = m.MetricId,
                DashboardId = m.DashboardId,
                ManufacturerId = m.ManufacturerId,
                DeliveryRate = m.DeliveryRate,
                DefectRate = m.DefectRate,
                DependencyRate = m.DependencyRate,
                RiskFlag = m.RiskFlag
            })
            .ToList();

        if (metricsList.Count == 0)
        {
            Console.WriteLine("⚠ No manufacturer metrics found for this dashboard.");
        }

        return metricsList;
    }

    // Save the new manufacturer dashboard to the database
    public void saveDashboardandMetrics(ManufacturerDashboardRdm dashboard)
    {
        if (_dbContext == null)
            return;

        // Create the DashboardTable (Entity) instead of DashboardDTO
        var newDbTable = new DashboardTable
        {
            Name = dashboard.Name,
            RequestedStartDate = dashboard.RequestedStartDate,
            RequestedEndDate = dashboard.RequestedEndDate,
            GeneratedDate = dashboard.GeneratedDate ?? DateTime.Now,
            ValidityDuration = dashboard.ValidityDuration,
            TypeId = 3 // Manufacturer Dashboard TypeId is 3 based on your setup
        };

        // Add the Dashboard entity to the DbContext
        _dbContext.Dashboards.Add(newDbTable);
        _dbContext.SaveChanges(); // Save dashboard to the database first

        // Loop through ManufacturerMetrics calculated for the dashboard
        foreach (var manufacturerMetrics in dashboard.MetricDetailsList) 
        {
            // Create ManufacturerMetricsTable from the ManufacturerMetrics RDM data
            var metricsTable = new ManufacturerMetricsTable
            {
                DashboardId = newDbTable.DashboardId,  // Link it to the created DashboardId
                ManufacturerId = int.Parse(manufacturerMetrics.ManufacturerId),
                DeliveryRate = manufacturerMetrics.DeliveryRate,
                DefectRate = manufacturerMetrics.DefectRate,
                DependencyRate = manufacturerMetrics.DependencyRate,
                RiskFlag = manufacturerMetrics.RiskFlag
            };

            // Add the metrics data to the database
            _dbContext.ManufacturerMetrics.Add(metricsTable);
        }

        // Save changes to the real database for Manufacturer Metrics
        _dbContext.SaveChanges();
    }

}
