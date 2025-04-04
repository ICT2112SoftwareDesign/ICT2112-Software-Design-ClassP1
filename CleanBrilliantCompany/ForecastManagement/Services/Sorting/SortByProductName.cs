using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Services.Sorting
{
    public class SortByProductName : IForecastSortingStrategy
    {
        public List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order)
        {
            return order == "ascending"
                                ? metrics.OrderBy(m => m.getProductName()).ToList()
                                : metrics.OrderByDescending(m => m.getProductName()).ToList();
        }
    }
}
