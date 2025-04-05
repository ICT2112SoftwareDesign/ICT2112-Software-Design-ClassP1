using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CleanBrilliantCompany.ForecastManagement.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "$discriminator")]
    [JsonDerivedType(typeof(PriceScenarioForecast), "priceScenario")]
    [JsonDerivedType(typeof(StockForecast), "stockForecast")]
    public abstract class ForecastMetrics
    {
        // Properties
        [JsonInclude]
        private int metricId { get; set; }
        [JsonInclude]
        private int productId { get; set; }
        [JsonInclude]
        private int forecastedStock { get; set; }
        [JsonInclude]
        private string productName { get; set; }
        public int GetMetricId()
        {
            return metricId;
        }

        public int GetProductId()
        {
            return productId;
        }
        public void SetProductId(int stockId)
        {
            productId = stockId;
        }
        public int GetForecastedStock()
        {
            return forecastedStock;
        }
        public void SetForecastedStock(int forecastedStock)
        {
            this.forecastedStock = forecastedStock;
        }

        public string GetProductName()
        {
            return productName;
        }
        public void SetProductName(string productName)
        {
            this.productName = productName;
        }

        public int GetProductID()
        {
            return productId;
        }
        public void SetProductID(int productID)
        {
            productId = productID;
        }
        public abstract ForecastMetrics GetForecastedMetrics();
    }

}
