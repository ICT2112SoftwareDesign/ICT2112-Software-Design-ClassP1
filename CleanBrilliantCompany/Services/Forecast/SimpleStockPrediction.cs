using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using Microsoft.Extensions.Logging.Abstractions;
namespace CleanBrilliantCompany.Services.Forecast
{
    public class SimpleStockPrediction : IStockPredictionService
    {
        
        public List<ForecastMetrics> generateStockPrediction(List<SalesDTO> sales,DateTime selectedMonth)
        {
            //sales = sales ?? _sales;
            //TODO to be replaced

            // Get aggregated results grouped by ProductId and Month (first day of the month)
            var aggregated = aggregateResults(sales);

            // Filter the aggregated results to only include records from the same month (ignoring the year)
            // Then group by product id so we can average values across different years.
            var forecastList = aggregated
                .Where(record => record.Key.Month.Month == selectedMonth.Month &&
                                 record.Key.Month.Year != selectedMonth.Year)
                .GroupBy(record => record.Key.ProductId)
                .Select(g =>
                {
                    // Calculate the average value for this product in the given month (across different years)
                    double averageValue = g.Average(r => r.Value);
                    // Increase the average by 50%
                    int forecastValue = (int)Math.Round(averageValue * 1.5);
                    return new StockForecast(g.Key, forecastValue);
                })
                .Cast<ForecastMetrics>()
                .ToList();

            return forecastList;
        }

        private Dictionary<(int ProductId, DateTime Month), int> aggregateResults(List<SalesDTO> sales)
        {
            //sales = sales ?? _sales;
            var aggregated = sales
                .GroupBy(s => new { s.ProductID, Year = s.DateTime.Year, Month = s.DateTime.Month })
                .ToDictionary(
                    g => (g.Key.ProductID, new DateTime(g.Key.Year, g.Key.Month, 1)),
                    g => g.Sum(s => s.Quantity)
                );

            return aggregated;
        }
    }
}
