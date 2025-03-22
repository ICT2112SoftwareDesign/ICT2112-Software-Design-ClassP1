using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastControl : IForecastListener
    {
        private readonly IForecastRepository forecastRepository;
        private readonly IForecastingFacade forecastingFacade;
        private ForecastDashboard dashboard;
        //to be replaced

        public ForecastControl(
            IForecastRepository forecastRepository,
            IForecastingFacade forecastingFacade,
            ISales isale
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
            this.dashboard = new ForecastDashboard(0,startDate, endDate, DateTime.Now,0,metricList);

            return this.dashboard;
        }

        public ForecastDashboard updateProductPriceAdjustment(int productId, String productName, int priceAdjustment,ForecastDashboard existingDashboard)
        {

            ForecastMetrics newMetric = forecastingFacade.updateProductPriceAdjustment(existingDashboard.GetStartDate(),productId, productName, priceAdjustment);
            List<ForecastMetrics> oldMetrics = existingDashboard.GetMetrics();
            List<ForecastMetrics> updatedMetrics = existingDashboard.GetMetrics()
            .Where(metric => metric.getProductId() != productId) // Remove the old metric
            .ToList();

            updatedMetrics.Add(newMetric);
            updatedMetrics = updatedMetrics.OrderBy(metric => metric.getProductId()).ToList();

            this.dashboard = new ForecastDashboard(0,existingDashboard.GetStartDate(), existingDashboard.GetEndDate(),DateTime.Now,0, updatedMetrics);
            return this.dashboard;
        }
        public ForecastDashboard GetDashboard()
        {
            return dashboard;
        }
        public void onQuerySucess() { }
    }
}
