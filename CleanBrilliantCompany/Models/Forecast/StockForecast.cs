using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class StockForecast : ForecastMetrics
    {
        public override StockForecast getForecastedMetrics()
        {
            return this;
        }
    }
}
