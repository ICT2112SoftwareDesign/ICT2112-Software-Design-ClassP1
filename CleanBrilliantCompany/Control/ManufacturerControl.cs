public class ManufacturerControl
{
    private Dashboard manufacturerDashboard;
    private readonly ManufacturerRepo ManufacturerMapper;
    private readonly FakeReorderInterface fakeReorderInterface;

    public ManufacturerControl(ManufacturerRepo ManufacturerMapper, FakeReorderInterface fakeReorderInterface)
    {
        this.ManufacturerMapper = ManufacturerMapper;
        this.fakeReorderInterface = fakeReorderInterface;
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
        DateTime requestedStartDate = dashboardDto.RequestedStartDate;
        DateTime requestedEndDate = dashboardDto.RequestedEndDate;

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

    // Method to generate a new manufacturer dashboard and calculate/save metrics
    public void GenerateNewDashboard(DashboardDTO dto)
    {
        // Set the type for the dashboard
        dto.Type = 3; // Assuming 3 corresponds to Manufacturer Dashboard Type ID

        // Create the new dashboard
        var dashboard = DashboardFactory.createDashboard(dto);

        // Assuming you have a reorder interface injected (like in your earlier instructions)
        var reorders = fakeReorderInterface.GetAllReorderDetails(); // Adjust this to match the method that fetches batches for manufacturers

        // Filter the reorders by the requested start and end date
        var filteredOrders = reorders
            .Where(b => b.ExpectedDeliveryDate >= dashboard.RequestedStartDate && b.ExpectedDeliveryDate <= dashboard.RequestedEndDate)
            .ToList();

        // Populate the analytics section of the dashboard (same logic, assuming you have analytics for Manufacturer)
        (dashboard as ManufacturerDashboardRdm).populateMetrics(filteredOrders);

        // Save the newly generated dashboard and analytics
        ManufacturerMapper.saveDashboardandMetrics(dashboard as ManufacturerDashboardRdm);
    }
}
