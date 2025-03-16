using CleanBrilliantCompany.Models.Forecast;


namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastingFacade
    {
        List<ForecastMetrics> generateStockForecast();
        List<ForecastMetrics> generatePriceScenario();
    }
}