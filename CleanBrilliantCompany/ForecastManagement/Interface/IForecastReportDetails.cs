using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.ForecastManagement.Interface
{
    public interface IForecastReportDetails
    {
        ForecastDashboard GetDashboard();
        string GenerateReport();
    }
}
