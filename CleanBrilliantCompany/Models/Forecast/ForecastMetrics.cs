using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Forecast
{
    public abstract class ForecastMetrics
    {
        // Properties
        private int productId { get; set; }
        private int forecastedStock { get; set; }
        private String productName {  get; set; }


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
        public abstract ForecastMetrics getForecastedMetrics();
    }

}
