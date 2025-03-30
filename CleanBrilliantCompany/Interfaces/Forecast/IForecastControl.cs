using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastControl
    {
        ForecastDashboard GetDashboard();
        string GenerateReport();
    }
}
