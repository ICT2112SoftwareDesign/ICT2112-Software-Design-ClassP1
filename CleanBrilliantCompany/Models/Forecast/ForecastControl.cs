using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastControl : IForecastListener
    {
        private readonly IForecastRepository forecastRepository;
        private readonly IForecastingFacade forecastingFacade;
        private ForecastDashboard dashboard;
        //to be replaced
        List<SalesDTO> _sales = new List<SalesDTO>()
        { 
            //Sales in May 2023
            new SalesDTO(1, new DateTime(2023, 5, 10), 9),

            //Sales in May 2024
            new SalesDTO(1, new DateTime(2024, 5, 10), 11),
            new SalesDTO(2, new DateTime(2024, 5, 15), 3),

            // Sales in May 2025
            new SalesDTO(1, new DateTime(2025, 5, 10), 5),
            new SalesDTO(2, new DateTime(2025, 5, 15), 3),
            new SalesDTO(1, new DateTime(2025, 5, 20), 2),
            
            // Sales in April 2025
            new SalesDTO(2, new DateTime(2025, 4, 20), 4),
            new SalesDTO(3, new DateTime(2025, 4, 25), 6),
            
            // Sales in March 2025
            new SalesDTO(1, new DateTime(2025, 3, 5), 7),
            new SalesDTO(3, new DateTime(2025, 3, 15), 3),
            new SalesDTO(2, new DateTime(2025, 3, 18), 4)
        };
        public ForecastControl(
            IForecastRepository forecastRepository,
            IForecastingFacade forecastingFacade
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

        public ForecastDashboard generateDashboard(String type, DateTime startDate, DateTime endDate)
        {
            List<ForecastMetrics> metricList = type switch
            {
                "price" => forecastingFacade.generatePriceScenario(),
                "stock" => forecastingFacade.generateStockForecast(_sales, startDate),
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
