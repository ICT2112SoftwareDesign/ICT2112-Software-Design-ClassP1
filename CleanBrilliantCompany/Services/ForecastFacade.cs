using CleanBrilliantCompany.DTO;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Services
{
    public class ForecastFacade: IForecastingFacade
    {
        private readonly IStockPredictionService _stockPredictionService;
        private readonly IScenarioPricingService _scenarioPricingService;
        private readonly INotificationService _notificationService;
        private readonly IProduct _iProduct;
        public ForecastFacade(
        IStockPredictionService stockPredictionService,
        IScenarioPricingService scenarioPricingService,
        INotificationService notificationService,
        IProduct iProduct)
        {
            _stockPredictionService = stockPredictionService;
            _scenarioPricingService = scenarioPricingService;
            _notificationService = notificationService;
            _iProduct = iProduct;
        }
        public List<ForecastMetrics> generateStockForecast(List<SalesDTO> sales, DateTime selectedMonth)
        {
            List<ProductDTO> productList = _iProduct.GetProductList();
            List<ForecastMetrics> metrics = _stockPredictionService.generateStockPrediction(sales, selectedMonth, productList);
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
