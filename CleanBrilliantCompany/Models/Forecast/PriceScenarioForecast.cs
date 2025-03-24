using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class PriceScenarioForecast : ForecastMetrics
    {
       
        // Properties
        [JsonInclude]
        private int adjustmentFactor { get; set; }
        [JsonInclude]
        private int pricingAfterAdjustment { get; set; }
        public int getAdjustmentFactor()
        {
            return adjustmentFactor;
        }

        public void setAdjustmentFactor(int adjustmentFactor)
        {
            this.adjustmentFactor = adjustmentFactor;
        }

        public int getPricingAfterAdjustment()
        {
            return pricingAfterAdjustment;
        }

        public void setPricingAfterAdjustment(int pricingAfterAdjustment)
        {
            this.pricingAfterAdjustment = pricingAfterAdjustment;
        }

        public override PriceScenarioForecast getForecastedMetrics()
        {
            return this;
        }

        public PriceScenarioForecast() { } // for the serializer

        [JsonConstructor]
        public PriceScenarioForecast(int productId, int forecastedStock, String productName,int adjustmentFactor, int pricingAfterAdjustment)
        {
            this.setProductId(productId);
            this.setProductName(productName);
            this.setForecastedStock(forecastedStock);
            this.adjustmentFactor = adjustmentFactor;
            this.pricingAfterAdjustment = pricingAfterAdjustment;
        }
    }
}
