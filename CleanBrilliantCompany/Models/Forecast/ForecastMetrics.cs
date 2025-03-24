using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CleanBrilliantCompany.Models.Forecast
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
        private String productName {  get; set; }
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
            this.productId = stockId;
        }
        public int getForecastedStock()
        {
            return forecastedStock;
        }
        public void setForecastedStock(int forecastedStock)
        {
            this.forecastedStock = forecastedStock;
        }

        public String getProductName()
        {
            return productName;
        }
        public void setProductName(String productName)
        {
            this.productName = productName;
        }

        public int getProductID()
        {
            return productId;
        }
        public void setProductID(int productID)
        {
            this.productId = productID;
        }
        public abstract ForecastMetrics getForecastedMetrics();
    }

}
