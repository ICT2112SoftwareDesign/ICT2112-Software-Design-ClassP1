using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.ForecastManagement.Interface
{
    public interface IPredictionService
    {
        //public List<ForecastMetrics> generateForecastMetric(Dictionary<int, int> aggregatedSales, List<ProductDTO> productList, int adjustmentFactor);
        public ForecastMetrics UpdateMetric(Dictionary<int, int> aggregatedSales, int productId, string productName, int adjustmentFactor);

    }
}
