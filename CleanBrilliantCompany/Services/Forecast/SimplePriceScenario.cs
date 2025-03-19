using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Services.Forecast
{
    public class SimplePriceScenario : IScenarioPricingService
    {
        public List<ForecastMetrics> generateScenarioPricing(Dictionary<int, int> aggregatedSales, List<ProductDTO> productList, int adjustmentFactor)
        {
            int minSales = aggregatedSales.Values.Min();
            int maxSales = aggregatedSales.Values.Max();
            var forecastDictionary = new Dictionary<int, int>();

            foreach(var product in productList)
            {
                int pastSales = aggregatedSales.TryGetValue(product.ID, out int totalSales) ? totalSales : 0;
                double normalizedSales = (maxSales == minSales) ? 1 : (pastSales - minSales) / (double)(maxSales - minSales);
                double baseMultiplier = 0.5 + (normalizedSales * 1); // Scales from 0.5 to 1.5
                // Apply inverse price impact multiplier (high price reduces demand)
                double priceMultiplier = GetPriceImpactMultiplier(pastSales, adjustmentFactor);
                int predictedDemand = (int)Math.Round(pastSales * baseMultiplier * priceMultiplier);
                forecastDictionary[product.ID] = Math.Max(predictedDemand, 0);


            }
            return forecastDictionary.Select(entry =>
            {
                var product = productList.FirstOrDefault(p => p.ID == entry.Key);
                string productName = product != null ? product.Name : "Unknown Product";

                return new PriceScenarioForecast(entry.Key, entry.Value, productName, adjustmentFactor,0);
            })
            .Cast<ForecastMetrics>()
            .ToList();
            //TODO: Implement this method
        }

        private double GetPriceImpactMultiplier(double normalizedSales, int adjustmentFactor)
        {
            double priceChange = adjustmentFactor / 100.0;
            double sensitivityFactor;

            // Apply different price sensitivities based on normalized sales
            if (normalizedSales <= 0.2)
            {
                sensitivityFactor = 1.5; // Very sensitive to price changes (low sales)
            }
            else if (normalizedSales <= 0.6)
            {
                sensitivityFactor = 1.2; // Moderately sensitive (medium sales)
            }
            else
            {
                sensitivityFactor = 0.8; // Less sensitive (high sales)
            }

            return Math.Max(1 - (priceChange * sensitivityFactor), 0);
        }


    }
}
