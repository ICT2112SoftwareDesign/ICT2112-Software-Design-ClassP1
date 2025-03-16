using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IScenarioPricingService
    {
        public List<ForecastMetrics> generateScenarioPricing();
    }
}