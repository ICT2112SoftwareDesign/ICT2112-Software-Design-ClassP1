using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CleanBrilliantCompany.ForecastManagement.Models
{
    public class PriceScenarioForecast : ForecastMetrics
    {

        // Properties
        [JsonInclude]
        private int adjustmentFactor { get; set; }
        [JsonInclude]
        private int pricingAfterAdjustment { get; set; }
        public int GetAdjustmentFactor()
        {
            return adjustmentFactor;
        }

        public void SetAdjustmentFactor(int adjustmentFactor)
        {
            this.adjustmentFactor = adjustmentFactor;
        }

        public int GetPricingAfterAdjustment()
        {
            return pricingAfterAdjustment;
        }

        public void SetPricingAfterAdjustment(int pricingAfterAdjustment)
        {
            this.pricingAfterAdjustment = pricingAfterAdjustment;
        }

        public override PriceScenarioForecast GetForecastedMetrics()
        {
            return this;
        }

        public PriceScenarioForecast() { } // for the serializer

        [JsonConstructor]
        public PriceScenarioForecast(int productId, int forecastedStock, string productName, int adjustmentFactor, int pricingAfterAdjustment)
        {
            SetProductId(productId);
            SetProductName(productName);
            SetForecastedStock(forecastedStock);
            this.adjustmentFactor = adjustmentFactor;
            this.pricingAfterAdjustment = pricingAfterAdjustment;
        }
    }
}
