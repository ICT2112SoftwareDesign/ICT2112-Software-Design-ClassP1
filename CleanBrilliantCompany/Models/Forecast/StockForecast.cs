using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class StockForecast : ForecastMetrics
    {
        
        public StockForecast() { }
        public StockForecast(int productId, int stockRequired)
        {
            this.setProductId(productId);
            this.setForecastedStock(stockRequired);

        }
        [JsonConstructor]

        public StockForecast(int productId, int forecastedStock, string productName)
        {
            this.setProductId(productId);
            this.setForecastedStock(forecastedStock);
            this.setProductName (productName);

        }
        public override StockForecast getForecastedMetrics()
        {
            return this;
        }


       
    }
}
