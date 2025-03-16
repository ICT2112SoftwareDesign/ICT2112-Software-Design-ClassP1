using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
namespace CleanBrilliantCompany.Services.Forecast
{
    public class SimpleStockPrediction : IStockPredictionService
    {
        public List<ForecastMetrics> generateStockPrediction()
        {
            List<ForecastMetrics> list = [new StockForecast()];
            return list;
            //TODO: Implement this method
        }
    }
}
