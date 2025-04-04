using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;
using CleanBrilliantCompany.Models.Forecast;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Services.Forecast
{
    public class StockForecastPrediction : IPredictionService
    {
        
        public ForecastMetrics updateMetric(Dictionary<int, int> aggregatedSales, int productId, string productName, int adjustmentFactor=0)
        {
            int forecastValue = 0;

            // Check if there are past sales for this product
            if (aggregatedSales.TryGetValue(productId, out int totalSales))
            {
                // Compute average of past sales and increase by 50%
                forecastValue = (int)Math.Round(totalSales * 1.5);
            }

            // Return a single ForecastMetrics (e.g., StockForecast)
            return new StockForecast(productId, forecastValue, productName);
        }
    }
}
