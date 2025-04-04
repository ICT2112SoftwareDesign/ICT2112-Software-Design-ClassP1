using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.ForecastManagement.Interface;

public interface IAnalyticsReportDetails
{
    List<Dashboard> GetDashboardsData();

    string GenerateCombinedReport(List<string> selectedDashboards);
}