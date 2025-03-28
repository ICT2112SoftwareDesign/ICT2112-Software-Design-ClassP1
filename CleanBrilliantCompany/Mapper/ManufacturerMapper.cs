

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
        else
        {
            Console.WriteLine($"✅ Manufacturer dashboard found: {dashboard.Name}");
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
        else
        {
            Console.WriteLine($"✅ Found {metricsList.Count} manufacturer metrics.");
        }

        return metricsList;
    }
}
