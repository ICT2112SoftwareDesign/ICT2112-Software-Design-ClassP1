using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Forecast;


namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastingFacade
    {
        List<ForecastMetrics> generateStockForecast(DateTime selectedMonth);
        List<ForecastMetrics> generatePriceScenario(DateTime selectedMonth, int adjustmentFactor);
        ForecastMetrics updateProductPriceAdjustment(DateTime selectedMonth, int productId, String productName, int priceAdjustment);

    }
}