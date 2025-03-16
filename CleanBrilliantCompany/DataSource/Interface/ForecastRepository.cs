using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.DataSource.Interface
{
    public interface IForecastRepository
    {
        public ForecastDashboard getLatestDashboard();
    }
}
