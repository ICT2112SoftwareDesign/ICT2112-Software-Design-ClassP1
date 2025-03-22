using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

namespace CleanBrilliantCompany.Services.Forecast
{
    public class PriceScenarioPrediction : IPredictionService
    {
        //prediction for a all product
        public List<ForecastMetrics> generateForecastMetric(Dictionary<int, int> aggregatedSales, List<ProductDTO> productList, int adjustmentFactor)
        {
            int minSales = aggregatedSales.Values.Min();
            int maxSales = aggregatedSales.Values.Max();
            var forecastDictionary = new Dictionary<int, int>();

            foreach(var product in productList)
            {
                double normalizedSales;
                double baseMultiplier;
                double priceMultiplier;
                int predictedDemand;
                int pastSales = aggregatedSales.TryGetValue(product.ID, out int totalSales) ? totalSales : 0;
                if (adjustmentFactor == 0)
                {
                     baseMultiplier = 1.5;
                    predictedDemand = (int)Math.Round(pastSales * baseMultiplier );

                }
                else
                {
                     normalizedSales = (maxSales == minSales) ? 1 : (pastSales - minSales) / (double)(maxSales - minSales);
                     baseMultiplier = 0.5 + (normalizedSales * 1); // Scales from 0.5 to 1.5
                                                                         // Apply inverse price impact multiplier (high price reduces demand)
                     priceMultiplier = GetPriceImpactMultiplier(pastSales, adjustmentFactor);
                     predictedDemand = (int)Math.Round(pastSales * baseMultiplier * priceMultiplier);
                }
                
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

        //prediction for a single product
        public ForecastMetrics updateMetric(Dictionary<int, int> aggregatedSales, int productId, string productName, int adjustmentFactor)
        {
           

            int minSales = aggregatedSales.Values.Min();
            int maxSales = aggregatedSales.Values.Max();
            double normalizedSales;
            double baseMultiplier=1.5;
            double priceMultiplier;
            int predictedDemand;
            // Get past sales for the given product
            int pastSales = aggregatedSales.TryGetValue(productId, out int totalSales) ? totalSales : 0;
            
            
            // Normalize past sales between 0 and 1
            normalizedSales = (maxSales == minSales) ? 1 : (pastSales - minSales) / (double)(maxSales - minSales);
            // Calculate base multiplier (scales between 0.5 - 1.5)
            // Apply inverse price impact multiplier (high price reduces demand)
            priceMultiplier = GetPriceImpactMultiplier(normalizedSales, adjustmentFactor);
            // Compute predicted demand
             predictedDemand = (int)Math.Round(pastSales * baseMultiplier * priceMultiplier);
           
        
            
            // Ensure predicted demand is not negative
            predictedDemand = Math.Max(predictedDemand, 0);

            // Return forecast for a single product
            return new PriceScenarioForecast(productId, predictedDemand, productName, adjustmentFactor, 0);
        }
    }
}
