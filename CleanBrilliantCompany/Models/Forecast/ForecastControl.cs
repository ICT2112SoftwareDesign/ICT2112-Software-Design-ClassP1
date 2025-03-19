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
            this.dashboard = new ForecastDashboard(startDate, endDate, metricList);
            return this.dashboard;
        }
        public ForecastDashboard GetDashboard()
        {
            return dashboard;
        }
        public void onQuerySucess() { }
    }
}
