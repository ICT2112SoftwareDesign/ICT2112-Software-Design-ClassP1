using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IPredictionService
    {
        public List<ForecastMetrics> generateForecastMetric(Dictionary<int, int> aggregatedSales, List<ProductDTO> productList, int adjustmentFactor);
        public ForecastMetrics updateMetric(Dictionary<int, int> aggregatedSales, int productId, string productName, int adjustmentFactor);

    }
}
