using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastReportDetails
    {
        ForecastDashboard GetDashboard();
        string GenerateReport();
    }
}
