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
        public int getMetricId()
        {
            return metricId;
        }

        public int getProductId()
        {
            return productId;
        }
        public void setProductId(int stockId)
        {
            productId = stockId;
        }
        public int getForecastedStock()
        {
            return forecastedStock;
        }
        public void setForecastedStock(int forecastedStock)
        {
            this.forecastedStock = forecastedStock;
        }

        public string getProductName()
        {
            return productName;
        }
        public void setProductName(string productName)
        {
            this.productName = productName;
        }

        public int getProductID()
        {
            return productId;
        }
        public void setProductID(int productID)
        {
            productId = productID;
        }
        public abstract ForecastMetrics getForecastedMetrics();
    }

}
