using CleanBrilliantCompany.DTO;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Services.Sorting;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastFacade
    {
        //private readonly IStockPredictionService _stockPredictionService;
        //private readonly IScenarioPricingService _scenarioPricingService;
        //private readonly INotificationService _notificationService;
        private readonly IProduct _iProduct;
        private readonly ISales _isale; //simulated interface
        private readonly MetricFactory _metricFactory;
        private readonly IForecastRepository _forecastRepository;
        private readonly IAlert _alertService;

        public ForecastFacade(
        MetricFactory metricFactory,
        IForecastRepository forecastRepository,
        IProduct iProduct,
        ISales iSale,
        IAlert alertService
        )
        {
            //_stockPredictionService = stockPredictionService;
            //_scenarioPricingService = scenarioPricingService;
            //_notificationService = notificationService;
            _iProduct = iProduct;
            _isale = iSale;
            _metricFactory = metricFactory;
            _forecastRepository = forecastRepository;
            _alertService = alertService;
        }
        public ForecastDashboard getLatestDashboard()
        {
            ForecastDashboard dashboard = _forecastRepository.getLatestDashboard();
            return dashboard;
        }
        public ForecastDashboard generateDashboard(DateTime selectedMonth, int adjustmentFactor = 0)
        {
            
            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            getSalesAndProduct(selectedMonth, out productList, out aggregatedSales);//to be updated to accept selectedMonth to pull data for exact months
            ForecastDashboard dashboard=null;
            if (adjustmentFactor == 0)
            {
                dashboard = _forecastRepository.getDashboard(selectedMonth.Month, selectedMonth.Year);
            }
            if (dashboard == null)
            {
                 dashboard = new ForecastDashboard(0, selectedMonth, selectedMonth.AddMonths(1).AddDays(-1), DateTime.Now, 0, new List<ForecastMetrics>()
);

                foreach (ProductDTO product in productList)
                {
                    ForecastMetrics metric = _metricFactory.GenerateForecastMetric(product.ID, aggregatedSales, product, adjustmentFactor);
                    dashboard.AddMetric(metric);

                }
                _forecastRepository.saveDashboard(dashboard);
               
                //Save to repo
                //List<ForecastMetrics> metrics = _stockPredictionService.generateStockPrediction(aggregatedSales, productList);
            }
            else
            {
                foreach (var metric in dashboard.GetMetrics())
                {
                    var product = productList.FirstOrDefault(p => p.ID == metric.getProductId());
                    metric.setProductName(product.Name);
                    
                }
            }
            //sorting
            dashboard.SetMetrics(ForecastMetricSorter.Sort(dashboard.GetMetrics(), "id", "ascending"));

            //Sorting 
            List<string> alert = new List<string>();
            if (dashboard.GetMetrics().Count != 0)
            {
               alert=_alertService.alert(dashboard.GetMetrics());
                dashboard.SetAlertItemList(alert);
            }
            

            return dashboard;

        }
        public ForecastDashboard updateMetric(int productId, ForecastDashboard dashboard, int adjustmentFactor = 0)
        {
            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            getSalesAndProduct(dashboard.GetStartDate(), out productList, out aggregatedSales);//to be updated to accept selectedMonth to pull data for exact months

            ProductDTO product = productList.FirstOrDefault(p => p.ID == productId);

            ForecastMetrics metric = _metricFactory.GenerateForecastMetric(productId, aggregatedSales, product, adjustmentFactor);
            dashboard.UpdateMetric(metric);
            List<string> alert = new List<string>();
            if (dashboard.GetMetrics().Count != 0)
            {
                alert = _alertService.alert(dashboard.GetMetrics());
                dashboard.SetAlertItemList(alert);
            }
            //dashboard.SetMetrics(ForecastMetricSorter.Sort(dashboard.GetMetrics(), sortingType, order));


            //Save to repo
            //List<ForecastMetrics> metrics = _stockPredictionService.generateStockPrediction(aggregatedSales, productList);
            return dashboard;

        }


        //private List<ForecastMetrics> SortMetrics(List<ForecastMetrics> metrics, String sortType)
        //{
        //    IForecastSortingStrategy strategy;

        //    switch (sortType)
        //    {
        //        case "name":
        //            strategy = new SortByProductName();
        //            break;
        //        case "value":
        //            strategy = new SortByForecastedStock();
        //            break;
        //        default:
        //            strategy = new SortByProductID(); 
        //            break;
        //    }

        //    var sorter = new ForecastMetricSorter(strategy);
        //    return sorter.Sort(metrics);
        //}

        //public List<ForecastMetrics> generatePriceScenario(DateTime  selectedMonth, int adjustmentFactor)
        //{
        //    List<ProductDTO> productList;
        //    Dictionary<int, int> aggregatedSales;
        //    getSalesAndProduct(selectedMonth, out productList, out aggregatedSales);
        //    List<ForecastMetrics> metrics = _scenarioPricingService.generateScenarioPricing(aggregatedSales, productList,adjustmentFactor);
        //    return metrics;
        //    //TODO: Implement this method

        //}
        //public ForecastMetrics updateProductPriceAdjustment(DateTime selectedMonth,int productId, String productName, int priceAdjustment)
        //{
        //    var salesList = _isale.getSalesData(selectedMonth.Month);
        //    Dictionary<int, int>  aggregatedSales = aggregateResults(salesList);

        //    ForecastMetrics metric = _scenarioPricingService.generateScenarioPricing( aggregatedSales,  productId,  productName, priceAdjustment);
        //    return metric;
        //    //TODO: Implement this method

        //}

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
