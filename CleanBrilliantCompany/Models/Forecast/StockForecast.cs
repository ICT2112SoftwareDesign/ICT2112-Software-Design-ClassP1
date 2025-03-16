using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{
    public class StockForecast : ForecastMetrics
    {
        public override StockForecast getForecastedMetrics()
        {
            return this;
        }
    }
}
