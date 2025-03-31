using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using CleanBrilliantCompany.Services.Sorting;
using Microsoft.Data.SqlClient;

public static class ForecastMetricSorter
{
    public static List<ForecastMetrics> Sort(List<ForecastMetrics> metrics, string type, string order)
    {
        IForecastSortingStrategy strategy = GetStrategy(type);
        return strategy.Sort(metrics, order);
    }

    private static IForecastSortingStrategy GetStrategy(string type)
    {
        switch (type)
        {
            case "name":
                return new SortByProductName();
            case "value":
                return new SortByForecastedStock();
            default:
                return new SortByProductID(); 
        }
    }
}
