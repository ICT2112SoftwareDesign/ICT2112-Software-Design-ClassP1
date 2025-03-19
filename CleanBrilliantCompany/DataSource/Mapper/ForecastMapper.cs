using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.DataSource.Mapper
{
    public class ForecastMapper : IForecastRepository
    {

        public ForecastDashboard getLatestDashboard()
        {
            DateTime startDate = DateTime.Now.AddDays(-7); // Start date 7 days ago
            DateTime endDate = DateTime.Now;
            List<ForecastMetrics> metricsList = new List<ForecastMetrics>
            {
              new StockForecast(1,12,"A"),
              new StockForecast(2,3, "B")
            };
            ForecastDashboard forecastDashboard = new ForecastDashboard(startDate, endDate, metricsList);
            return forecastDashboard;
        }
    }
}

