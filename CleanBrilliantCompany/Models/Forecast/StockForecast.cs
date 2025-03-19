using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class StockForecast : ForecastMetrics
    {
        
        public StockForecast() { }
        public StockForecast(int productId, int stockRequired, string productName)
        {
            this.setProductId(productId);
            this.setForecastedStock(stockRequired);
            this.setProductName (productName);

        }
        public override StockForecast getForecastedMetrics()
        {
            return this;
        }
    }
}
