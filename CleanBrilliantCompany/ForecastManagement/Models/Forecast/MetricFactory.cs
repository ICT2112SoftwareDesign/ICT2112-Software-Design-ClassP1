using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Services.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class MetricFactory
    {
        private readonly IEnumerable<IPredictionService> _predictionServicesList;

        public MetricFactory(IEnumerable<IPredictionService>  predictionServicesList)
        {
            _predictionServicesList = predictionServicesList;
        }

        public ForecastMetrics GenerateForecastMetric(int productId, Dictionary<int, int> aggregatedSales, ProductDTO product, int adjustmentFactor = 0)
        {
            IPredictionService? predictionService;

            if (adjustmentFactor == 0)
            {
                predictionService = _predictionServicesList
                    .FirstOrDefault(service => service is StockForecastPrediction);
            }
            else
            {
                predictionService = _predictionServicesList
                    .FirstOrDefault(service => service is PriceScenarioPrediction);
            }

            if (predictionService == null)
            {
                throw new InvalidOperationException("Suitable prediction service not found.");
            }

            return predictionService.updateMetric(aggregatedSales, product.ID, product.Name, adjustmentFactor);
        }
    }
}
