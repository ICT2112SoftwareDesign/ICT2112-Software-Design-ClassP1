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
        private readonly ISales _isale; //simulated interface

        public ForecastFacade(
        IStockPredictionService stockPredictionService,
        IScenarioPricingService scenarioPricingService,
        INotificationService notificationService,

        IProduct iProduct,
        ISales iSale)
        {
            _stockPredictionService = stockPredictionService;
            _scenarioPricingService = scenarioPricingService;
            _notificationService = notificationService;
            _iProduct = iProduct;
            _isale = iSale;

        }
        public List<ForecastMetrics> generateStockForecast( DateTime selectedMonth)
        {
            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            getSalesAndProduct(selectedMonth, out productList, out aggregatedSales);//to be updated to accept selectedMonth to pull data for exact months
            List<ForecastMetrics> metrics = _stockPredictionService.generateStockPrediction(aggregatedSales, productList);
            return metrics;

            //TODO: Implement this method
        }

        

        public List<ForecastMetrics> generatePriceScenario(DateTime  selectedMonth, int adjustmentFactor)
        {
            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            getSalesAndProduct(selectedMonth, out productList, out aggregatedSales);
            List<ForecastMetrics> metrics = _scenarioPricingService.generateScenarioPricing(aggregatedSales, productList,adjustmentFactor);
            return metrics;
            //TODO: Implement this method

        }

        private void getSalesAndProduct(DateTime selectedMonth, out List<ProductDTO> productList, out Dictionary<int, int> aggregatedSales)
        {
            productList = _iProduct.GetProductList();
            var salesList = _isale.getSalesData(selectedMonth.Month);
            aggregatedSales = aggregateResults(salesList);

        }

        private Dictionary<int, int> aggregateResults(List<SalesDTO> sales)
        {
            // Aggregate total sales per product ID
            return sales
                .GroupBy(s => s.ProductID)
                .ToDictionary(
                    g => g.Key, // ProductID as key
                    g => g.Sum(s => s.Quantity) // Sum up all quantities for this product
                );
        }



    }
}
