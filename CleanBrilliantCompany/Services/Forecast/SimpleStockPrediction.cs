using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using Microsoft.Extensions.Logging.Abstractions;
namespace CleanBrilliantCompany.Services.Forecast
{
    public class SimpleStockPrediction : IStockPredictionService
    {

        public List<ForecastMetrics> generateStockPrediction(List<SalesDTO> sales, DateTime selectedMonth, List<ProductDTO> productList)
        {
            // Aggregate past sales data
            var aggregated = aggregateResults(sales);

            // Dictionary to store forecasted stock values for each product
            var forecastDictionary = productList.ToDictionary(
                product => product.ID,
                product => 0 // Default forecast is 0
            );

            // Process historical sales and update the forecast dictionary
            foreach (var product in productList)
            {
                var pastSales = aggregated
                    .Where(record => record.Key.Month.Month == selectedMonth.Month &&
                                     record.Key.Month.Year != selectedMonth.Year &&
                                     record.Key.ProductId == product.ID)
                    .Select(record => record.Value)
                    .ToList();

                if (pastSales.Any())
                {
                    // Compute average of past sales and increase by 50%
                    double averageValue = pastSales.Average();
                    forecastDictionary[product.ID] = (int)Math.Round(averageValue * 1.5);
                }
            }

            // Convert to list of StockForecast objects with Product Name
            var forecastList = forecastDictionary
                .Select(entry =>
                {
                    var product = productList.FirstOrDefault(p => p.ID == entry.Key);
                    string productName = product != null ? product.Name : "Unknown Product";

                    return new StockForecast(entry.Key, entry.Value, productName);
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
