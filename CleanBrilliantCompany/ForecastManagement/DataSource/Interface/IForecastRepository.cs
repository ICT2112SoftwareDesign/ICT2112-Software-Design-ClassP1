using CleanBrilliantCompany.ForecastManagement.Models;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.DataSource.Interface
{
    public interface IForecastRepository
    {
        public void SaveDashboard(ForecastDashboard newDashboard);
        public ForecastDashboard GetDashboard(int month, int year);
    }
}
