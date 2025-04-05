public interface ICostRepository
{
    void LoadDashboards();
    CostDashboardRdm? GetLatestDashboard();
    void GenerateNewDashboard(DashboardDTO dto);
    void UpdateDashboard();
}