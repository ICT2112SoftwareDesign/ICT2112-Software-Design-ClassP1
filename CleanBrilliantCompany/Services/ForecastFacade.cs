using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Services
{
    public class ForecastFacade: IForecastingFacade
    {
        private readonly IStockPredictionService _stockPredictionService;
        private readonly IScenarioPricingService _scenarioPricingService;
        private readonly INotificationService _notificationService;
        public ForecastFacade(
        IStockPredictionService stockPredictionService,
        IScenarioPricingService scenarioPricingService,
        INotificationService notificationService)
        {
            _stockPredictionService = stockPredictionService;
            _scenarioPricingService = scenarioPricingService;
            _notificationService = notificationService;
        }
        public List<ForecastMetrics> generateStockForecast()
        {
            List<ForecastMetrics> metrics =_stockPredictionService.generateStockPrediction();
            return metrics;
            
            //TODO: Implement this method
        }
        public List<ForecastMetrics> generatePriceScenario()
        {
            List<ForecastMetrics> metrics = _scenarioPricingService.generateScenarioPricing();
            return metrics;
            //TODO: Implement this method

        }
        
    }
}
