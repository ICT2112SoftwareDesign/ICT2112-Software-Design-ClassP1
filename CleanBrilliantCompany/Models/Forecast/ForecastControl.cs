using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastControl : IForecastControl, IForecastListener
    {
        private readonly IForecastRepository forecastRepository;
        private readonly IForecastingFacade forecastingFacade;
        private ForecastDashboard dashboard;
        //to be replaced

        public ForecastControl(
            IForecastRepository forecastRepository,
            IForecastingFacade forecastingFacade,
            IOrderRange isale
        )
        {
            this.forecastRepository = forecastRepository;
            this.forecastingFacade = forecastingFacade;

            getLatestDashboard();
        }

        private void getLatestDashboard()
        {
            ForecastDashboard dashboard = forecastRepository.getLatestDashboard();
            this.dashboard = dashboard;
        }

        public ForecastDashboard generateDashboard(String type, DateTime startDate, DateTime endDate, int? adjustmentFactor)
        {
            List<ForecastMetrics> metricList = type switch
            {
                "price" => forecastingFacade.generatePriceScenario(startDate, adjustmentFactor ?? 0),
                "stock" => forecastingFacade.generateStockForecast(startDate),
                _ => new List<ForecastMetrics>(), // Default to empty if type is invalid
            };
            this.dashboard = new ForecastDashboard(0, startDate, endDate, DateTime.Now, 0, metricList);

            return this.dashboard;
        }

        public ForecastDashboard updateProductPriceAdjustment(int productId, String productName, int priceAdjustment, ForecastDashboard existingDashboard)
        {

            ForecastMetrics newMetric = forecastingFacade.updateProductPriceAdjustment(existingDashboard.GetStartDate(), productId, productName, priceAdjustment);
            List<ForecastMetrics> oldMetrics = existingDashboard.GetMetrics();
            List<ForecastMetrics> updatedMetrics = existingDashboard.GetMetrics()
            .Where(metric => metric.getProductId() != productId) // Remove the old metric
            .ToList();

            updatedMetrics.Add(newMetric);
            updatedMetrics = updatedMetrics.OrderBy(metric => metric.getProductId()).ToList();

            this.dashboard = new ForecastDashboard(0, existingDashboard.GetStartDate(), existingDashboard.GetEndDate(), DateTime.Now, 0, updatedMetrics);
            return this.dashboard;
        }
        public ForecastDashboard GetDashboard()
        {
            return dashboard;
        }

        public string GenerateReport()
        {
            var dashboard = forecastingFacade.retrieveUpcomingDashboard();

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
                report.AppendLine($"<td>{metric.getProductId()}</td>");
                report.AppendLine($"<td>{metric.getProductName()}</td>");
                report.AppendLine($"<td>{metric.getForecastedStock()}</td>");
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

        public void onQuerySucess() { }
    }
}
