using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.ForecastManagement.Interface
{
    public interface IAlert
    {
        public List<string> Alert(List<ForecastMetrics> metrics);
    }
}
