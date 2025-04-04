using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.ForecastManagement.Interface
{
    public interface IForecastSortingStrategy
    {
        List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order);

    }
}
