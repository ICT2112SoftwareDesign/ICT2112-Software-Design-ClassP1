using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Forecast
{
    public abstract class ForecastMetrics
    {
        // Properties
        private int stockId { get; set; }
        private int forecastedStock { get; set; }

        public int getStockId()
        {
            return stockId;
        }
        public void setStockId(int stockId)
        {
            this.stockId = stockId;
        }
        public int getForecastedStock()
        {
            return forecastedStock;
        }
        public void setForecastedStock(int forecastedStock)
        {
            this.forecastedStock = forecastedStock;
        }
        public abstract ForecastMetrics getForecastedMetrics();
    }

}
