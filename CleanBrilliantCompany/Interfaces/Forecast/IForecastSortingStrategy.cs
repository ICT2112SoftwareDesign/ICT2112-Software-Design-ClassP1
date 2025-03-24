using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastSortingStrategy
    {
        List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order);

    }
}
