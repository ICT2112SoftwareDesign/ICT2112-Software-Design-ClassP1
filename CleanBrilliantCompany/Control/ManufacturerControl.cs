public class ManufacturerControl
{
    private Dashboard manufacturerDashboard;
    private readonly ManufacturerRepo ManufacturerMapper;

    public ManufacturerControl(ManufacturerRepo ManufacturerMapper)
    {
        this.ManufacturerMapper = ManufacturerMapper;
        LoadDashboard();
    }

    // Load the latest manufacturer dashboard
    private void LoadDashboard()
    {
        var dashboardDto = ManufacturerMapper.GetLatestManufacturerDashboard();
        if (dashboardDto == null)
        {
            Console.WriteLine("⚠ No manufacturer dashboard found.");
            return;
        }
        Console.WriteLine($"📊 Manufacturer dashboard found: {dashboardDto.Name} generated on: {dashboardDto.GeneratedDate}");

        // Check if the dates are null, and provide default values if needed
        DateTime requestedStartDate = dashboardDto.RequestedStartDate ?? DateTime.MinValue;  // Default to MinValue if null
        DateTime requestedEndDate = dashboardDto.RequestedEndDate ?? DateTime.MinValue;      // Default to MinValue if null

        // Create the dashboard object using the factory method
        manufacturerDashboard = DashboardFactory.createDashboard(dashboardDto) as ManufacturerDashboardRdm;

        // Fetch the manufacturer metrics and assign it to the dashboard
        var metricsList = ManufacturerMapper.GetManufacturerMetrics(dashboardDto.DashboardId); 
        
        if (manufacturerDashboard is ManufacturerDashboardRdm manufacturerDashboardRdm) 
        {
            manufacturerDashboardRdm.Metrics = metricsList;
            Console.WriteLine($"📊 {metricsList.Count} Manufacturer Metrics found.");
        } 
        else 
        {
            Console.WriteLine("⚠ Manufacturer dashboard could not be created.");
        }
    }


    // Method to retrieve the latest dashboard
    public ManufacturerDashboardRdm? GetLatestDashboard()
    {
        Console.WriteLine("🔍 Retrieving the latest manufacturer dashboard...");
        return manufacturerDashboard as ManufacturerDashboardRdm; 
    }
}
