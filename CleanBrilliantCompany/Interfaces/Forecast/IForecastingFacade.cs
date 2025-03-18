using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Forecast;


namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastingFacade
    {
        List<ForecastMetrics> generateStockForecast(List<SalesDTO>sales,DateTime selectedMonth);
        List<ForecastMetrics> generatePriceScenario();
    }
}