using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Services.Sorting
{
    public class SortByProductID : IForecastSortingStrategy
    {
        public List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order)
        {
            return order == "ascending"
                                   ? metrics.OrderBy(m => m.getProductID()).ToList()
                                   : metrics.OrderByDescending(m => m.getProductID()).ToList();
        }
    }
}
