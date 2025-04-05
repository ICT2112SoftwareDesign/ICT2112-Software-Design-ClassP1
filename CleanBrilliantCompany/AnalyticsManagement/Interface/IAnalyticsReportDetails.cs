using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.ForecastManagement.Interface;

public interface IAnalyticsReportDetails
{
    string GenerateCombinedReport(List<string> selectedDashboards);
}