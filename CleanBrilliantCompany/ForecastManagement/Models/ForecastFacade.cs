using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.Services.Sorting;
using CleanBrilliantCompany.ForecastManagement.DTO;
using CleanBrilliantCompany.ForecastManagement.Interface;

namespace CleanBrilliantCompany.ForecastManagement.Models
{
    public class ForecastFacade : IForecastReportDetails

    {
        //private readonly IStockPredictionService _stockPredictionService;
        //private readonly IScenarioPricingService _scenarioPricingService;
        //private readonly INotificationService _notificationService;
        //simulated interface
        private readonly MetricFactory _metricFactory;
        private readonly IForecastRepository _forecastRepository;
        private readonly IAlert _alertService;
        private readonly IForecastDataAdapter _forecastDataAdapter;

        public ForecastFacade(
            MetricFactory metricFactory,
            IForecastRepository forecastRepository,
            TempForecastIProduct iProduct,
            IOrderRange iSale,
            IAlert alertService,
            IForecastDataAdapter forecastDataAdapter
        )
        {
            //_stockPredictionService = stockPredictionService;
            //_scenarioPricingService = scenarioPricingService;
            //_notificationService = notificationService;

            _metricFactory = metricFactory;
            _forecastRepository = forecastRepository;
            _alertService = alertService;
            _forecastDataAdapter = forecastDataAdapter;
        }

        public ForecastDashboard GenerateDashboard(DateTime selectedMonth, int adjustmentFactor = 0)
        {

            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            _forecastDataAdapter.GetForecastInputs(selectedMonth, out productList, out aggregatedSales);
            ForecastDashboard dashboard = null;
            if (adjustmentFactor == 0)
            {
                dashboard = _forecastRepository.GetDashboard(selectedMonth.Month, selectedMonth.Year);
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
                if (adjustmentFactor == 0)
                {
                    _forecastRepository.SaveDashboard(dashboard);

                }


                //Save to repo
                //List<ForecastMetrics> metrics = _stockPredictionService.generateStockPrediction(aggregatedSales, productList);
            }
            else
            {
                foreach (var metric in dashboard.GetMetrics())
                {
                    var product = productList.FirstOrDefault(p => p.ID == metric.GetProductId());
                    metric.SetProductName(product.Name);

                }
            }
            //sorting
            dashboard.SetMetrics(ForecastMetricSorter.Sort(dashboard.GetMetrics(), "id", "ascending"));

            //Sorting 
            List<string> alert = new List<string>();
            if (dashboard.GetMetrics().Count != 0)
            {
                alert = _alertService.Alert(dashboard.GetMetrics());
                dashboard.SetAlertItemList(alert);
            }


            return dashboard;

        }
        public ForecastDashboard UpdateMetric(int productId, ForecastDashboard dashboard, int adjustmentFactor = 0)
        {
            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            _forecastDataAdapter.GetForecastInputs(dashboard.GetStartDate(), out productList, out aggregatedSales);//to be updated to accept selectedMonth to pull data for exact months

            ProductDTO product = productList.FirstOrDefault(p => p.ID == productId);

            ForecastMetrics metric = _metricFactory.GenerateForecastMetric(productId, aggregatedSales, product, adjustmentFactor);
            dashboard.UpdateMetric(metric);
            List<string> alert = new List<string>();
            if (dashboard.GetMetrics().Count != 0)
            {
                alert = _alertService.Alert(dashboard.GetMetrics());
                dashboard.SetAlertItemList(alert);
            }
            //dashboard.SetMetrics(ForecastMetricSorter.Sort(dashboard.GetMetrics(), sortingType, order));


            //Save to repo
            //List<ForecastMetrics> metrics = _stockPredictionService.generateStockPrediction(aggregatedSales, productList);
            return dashboard;

        }

        // 1) Make sure your method signature matches the return type:
        public Dictionary<string, Dictionary<int, object>> GenerateForecastTrendData(DateTime startMonth, int months)
        {
            var trendData = new Dictionary<string, Dictionary<int, object>>();

            for (int i = 0; i < months; i++)
            {
                var date = startMonth.AddMonths(i);
                var forecast = GenerateDashboard(date, 0);
                var metrics = forecast.GetMetrics();

                // Create a dictionary mapping productId to an anonymous object
                // with both the product name and forecasted stock.
                var productMap = metrics
                    .GroupBy(m => m.GetProductId())
                    .ToDictionary(
                        grp => grp.Key,
                        grp => (object)new
                        {
                            ProductName = grp.First().GetProductName(),
                            ForecastedStock = grp.Sum(m => (decimal)m.GetForecastedStock())
                        }
                    );

                trendData[date.ToString("yyyy-MM")] = productMap;
            }

            return trendData;
        }





        public ForecastMetrics UpdateProductPriceAdjustment(DateTime selectedMonth, int productId, string productName, int priceAdjustment)
        {
            List<ProductDTO> productList;
            Dictionary<int, int> aggregatedSales;
            _forecastDataAdapter.GetForecastInputs(selectedMonth, out productList, out aggregatedSales);

            ProductDTO product = productList.FirstOrDefault(p => p.ID == productId);

            ForecastMetrics metric = _metricFactory.GenerateForecastMetric(productId, aggregatedSales, product, priceAdjustment);
            return metric;
        }

        public List<ForecastMetrics> GeneratePriceScenario(DateTime selectedMonth, int adjustmentFactor)
        {
            var dashboard = GenerateDashboard(selectedMonth, adjustmentFactor);
            return dashboard.GetMetrics();
        }
        public List<ForecastMetrics> GenerateStockForecast(DateTime selectedMonth)
        {
            var dashboard = GenerateDashboard(selectedMonth, 0);
            return dashboard.GetMetrics();
        }

        public ForecastDashboard GetDashboard()
        {
            return GenerateDashboard(DateTime.Now.AddMonths(1), 0);

        }
        public string GenerateReport()
        {
            var dashboard = GetDashboard();

            if (dashboard == null || dashboard.GetMetrics().Count == 0)
            {
                return "<p>No forecast data available for the upcoming month.</p>";
            }

            var report = new System.Text.StringBuilder();
            report.AppendLine("<h1>Forecast Dashboard Report</h1>");
            report.AppendLine($"<p>Forecast Period: {dashboard.GetStartDate():yyyy-MM-dd} to {dashboard.GetEndDate():yyyy-MM-dd}</p>");
            report.AppendLine("<hr/>");
            report.AppendLine("<h2>Forecast Metrics</h2>");
            report.AppendLine("<table border='1' cellpadding='6' cellspacing='0' style='border-collapse: collapse;'>");
            report.AppendLine("<thead><tr>");
            report.AppendLine("<th>Product ID</th>");
            report.AppendLine("<th>Product Name</th>");
            report.AppendLine("<th>Forecasted Stock</th>");
            report.AppendLine("<th>Stock Status</th>");
            report.AppendLine("</tr></thead><tbody>");

            foreach (var metric in dashboard.GetMetrics())
            {
                report.AppendLine("<tr>");
                report.AppendLine($"<td>{metric.GetProductId()}</td>");
                report.AppendLine($"<td>{metric.GetProductName()}</td>");
                report.AppendLine($"<td>{metric.GetForecastedStock()}</td>");
                report.AppendLine("</tr>");
            }

            report.AppendLine("</tbody></table>");

            if (dashboard.GetAlertItemList().Count > 0)
            {
                report.AppendLine("<h2>Alerts</h2><ul>");
                foreach (var alert in dashboard.GetAlertItemList())
                {
                    report.AppendLine($"<li>{alert}</li>");
                }
                report.AppendLine("</ul>");
            }

            return report.ToString();
        }




    }
}
