using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IAlert
    {
        public List<string> alert(List<ForecastMetrics> metrics);
    }
}
