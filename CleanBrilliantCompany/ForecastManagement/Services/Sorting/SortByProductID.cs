using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.Services.Sorting
{
    public class SortByProductID : IForecastSortingStrategy
    {
        public List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order)
        {
            return order == "ascending"
                                   ? metrics.OrderBy(m => m.GetProductID()).ToList()
                                   : metrics.OrderByDescending(m => m.GetProductID()).ToList();
        }
    }
}
