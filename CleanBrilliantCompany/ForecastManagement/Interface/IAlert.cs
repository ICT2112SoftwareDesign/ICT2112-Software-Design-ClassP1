using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.ForecastManagement.Interface
{
    public interface IAlert
    {
        public List<string> alert(List<ForecastMetrics> metrics);
    }
}
