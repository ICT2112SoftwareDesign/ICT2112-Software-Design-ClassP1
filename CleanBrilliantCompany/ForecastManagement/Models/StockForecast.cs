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
            setProductId(productId);
            setForecastedStock(stockRequired);

        }
        [JsonConstructor]

        public StockForecast(int productId, int forecastedStock, string productName)
        {
            setProductId(productId);
            setForecastedStock(forecastedStock);
            setProductName(productName);

        }
        public override StockForecast getForecastedMetrics()
        {
            return this;
        }



    }
}
