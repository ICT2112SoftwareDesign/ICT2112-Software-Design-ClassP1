public interface ManufacturerRepo
{
    // Fetch the latest manufacturer dashboard
    DashboardDTO? GetLatestManufacturerDashboard();

    List<ManufacturerMetricsDTO> GetManufacturerMetrics(int dashboardId);

    void saveDashboardandMetrics(ManufacturerDashboardRdm dashboard);
}
