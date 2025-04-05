using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;

namespace CleanBrilliantCompany.Services.Sorting
{
    public class SortByProductName : IForecastSortingStrategy
    {
        public List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order)
        {
            return order == "ascending"
                                ? metrics.OrderBy(m => m.GetProductName()).ToList()
                                : metrics.OrderByDescending(m => m.GetProductName()).ToList();
        }
    }
}
