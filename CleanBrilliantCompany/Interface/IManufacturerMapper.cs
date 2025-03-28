public interface ManufacturerRepo
{
    // Fetch the latest manufacturer dashboard
    DashboardDTO? GetLatestManufacturerDashboard();
}
