using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IStockPredictionService
    {
        public List<ForecastMetrics> generateStockPrediction(Dictionary<int, int> aggregatedSales, List<ProductDTO> productList);
    }
}