using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class PriceScenarioForecast : ForecastMetrics
    {
        // Properties
        private int adjustmentFactor { get; set; }
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
    }
}
