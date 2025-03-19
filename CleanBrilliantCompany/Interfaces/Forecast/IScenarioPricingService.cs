using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IScenarioPricingService
    {
        public List<ForecastMetrics> generateScenarioPricing(Dictionary<int, int> aggregatedSales, List<ProductDTO>productList, int adjustmentFactor);
    }
}