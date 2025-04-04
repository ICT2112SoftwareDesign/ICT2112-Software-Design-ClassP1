using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Services.Sorting
{
    public class SortByForecastedStock : IForecastSortingStrategy
    {
        public List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string order)
        {
            return order == "ascending"
                        ? metrics.OrderBy(m => m.getForecastedStock()).ToList()
                        : metrics.OrderByDescending(m => m.getForecastedStock()).ToList();
        }
    }
}
