using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Services.Forecast
{
    public class SimplePriceScenario : IScenarioPricingService
    {
        public List<ForecastMetrics> generateScenarioPricing()
        {
            List<ForecastMetrics> list = [new PriceScenarioForecast()];
            return list;
            //TODO: Implement this method
        }
    }
}
