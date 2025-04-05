public interface IAgingRepository
{
    // Fetch the latest dashboard DTO
    DashboardDTO? GetLatestAgingDashboard();

    // Fetch Aging Analytics DTOs for a given dashboard ID
    List<AgingAnalyticsDetailsDTO> GetAgingAnalytics(int dashboardId);

    void saveDashboardandAnalytics(AgingDashboardRdm dashboard); 
}