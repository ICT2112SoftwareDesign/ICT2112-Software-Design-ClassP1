using CleanBrilliantCompany.ForecastManagement.Models;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.DataSource.Interface
{
    public interface IForecastRepository
    {
        public void saveDashboard(ForecastDashboard newDashboard);
        public ForecastDashboard getDashboard(int month, int year);
    }
}
