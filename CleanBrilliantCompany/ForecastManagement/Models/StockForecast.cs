using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CleanBrilliantCompany.ForecastManagement.Models
{
    public class StockForecast : ForecastMetrics
    {

        public StockForecast() { }
        public StockForecast(int productId, int stockRequired)
        {
            SetProductId(productId);
            SetForecastedStock(stockRequired);

        }
        [JsonConstructor]

        public StockForecast(int productId, int forecastedStock, string productName)
        {
            SetProductId(productId);
            SetForecastedStock(forecastedStock);
            SetProductName(productName);

        }
        public override StockForecast GetForecastedMetrics()
        {
            return this;
        }



    }
}
