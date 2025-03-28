

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
                TypeId = d.TypeId
            })
            .FirstOrDefault();

        Console.WriteLine("DAHHBOARDID WE ARE IN MAPPER---------------------------------");
        Console.WriteLine(dashboard.DashboardId);

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
}
