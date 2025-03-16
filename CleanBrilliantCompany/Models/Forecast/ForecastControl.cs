using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastControl : IForecastListener
    {
        private readonly IForecastRepository forecastRepository;
        private readonly IForecastingFacade forecastingFacade;
        private ForecastDashboard dashboard;

        public ForecastControl()
        {
            getLatestDashboard();
        }
        private void getLatestDashboard()
        {
            ForecastDashboard dashboard = forecastRepository.getLatestDashboard();
            this.dashboard = dashboard;
        }

        public void generateDashboard(String type, DateTime startDate, DateTime endDate)
        {
            List<ForecastMetrics> metricList = type switch
            {
                "price" => forecastingFacade.generatePriceScenario(),
                "stock" => forecastingFacade.generateStockForecast(),
                _ => new List<ForecastMetrics>() // Default to empty if type is invalid
            };
            this.dashboard = new ForecastDashboard(startDate, endDate, metricList);
        }

        public void onQuerySucess()
        {
            
        }
    }
}
