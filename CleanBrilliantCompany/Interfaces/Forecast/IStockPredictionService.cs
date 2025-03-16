using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IStockPredictionService
    {
        public List<ForecastMetrics> generateStockPrediction();
    }
}