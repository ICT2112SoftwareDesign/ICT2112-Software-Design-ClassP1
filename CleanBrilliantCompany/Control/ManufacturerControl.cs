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

    // Create the dashboard object (Pass the required arguments to the constructor)
    manufacturerDashboard = new ManufacturerDashboardRdm(
        dashboardDto.DashboardId,                  // id
        dashboardDto.Name,                         // name
        requestedStartDate,           // requestedStartDate
        requestedEndDate,             // requestedEndDate
        dashboardDto.ValidityDuration,             // validityDuration
        dashboardDto.TypeId,                       // type
        dashboardDto.GeneratedDate                 // generatedDate (optional)
    );
}


    // Method to retrieve the latest dashboard
    public ManufacturerDashboardRdm? GetLatestDashboard()
    {
        Console.WriteLine("🔍 Retrieving the latest manufacturer dashboard...");
        return manufacturerDashboard as ManufacturerDashboardRdm; 
    }
}
