using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class StockForecast : ForecastMetrics
    {
        public StockForecast() { }
        public StockForecast(int stockId, int stockRequired)
        {
            this.setStockId(stockId);
            this.setForecastedStock(stockRequired);
        }
        public override StockForecast getForecastedMetrics()
        {
            return this;
        }
    }
}
