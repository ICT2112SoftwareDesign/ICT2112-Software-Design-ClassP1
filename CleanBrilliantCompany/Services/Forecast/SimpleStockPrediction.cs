using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Services.Forecast
{
    public class SimpleStockPrediction : IStockPredictionService
    {
        public List<ForecastMetrics> generateStockPrediction(Dictionary<int, int> aggregatedSales, List<ProductDTO> productList)
        {
            // Dictionary to store forecasted stock values for each product
            var forecastDictionary = productList.ToDictionary(
                product => product.ID,
                product => 0 // Default forecast is 0 if no past sales exist
            );

            // Process historical sales and update the forecast dictionary
            foreach (var product in productList)
            {
                if (aggregatedSales.TryGetValue(product.ID, out int totalSales))
                {
                    // Compute average of past sales and increase by 50%
                    double forecastValue = totalSales * 1.5;
                    forecastDictionary[product.ID] = (int)Math.Round(forecastValue);
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
    }
}
