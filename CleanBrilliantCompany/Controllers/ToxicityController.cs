using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace CleanBrilliantCompany.Controllers
{
    public class ToxicityController : Controller
    {
        private readonly IToxicity _toxicity;
        private readonly IIngredientDB _ingredientGateway;
        private readonly IToxicityClassificationStrategy _classificationStrategy;
        private readonly ILogger<ToxicityController> _logger;
        // If you integrate with carbon footprint data later, you'll add that interface here
        // private readonly ICarbonFootprint _carbonFootprint;

        public ToxicityController(
            IToxicity toxicity, 
            IIngredientDB ingredientGateway, 
            IToxicityClassificationStrategy classificationStrategy,
            ILogger<ToxicityController> logger)
        {
            _toxicity = toxicity;
            _ingredientGateway = ingredientGateway;
            _classificationStrategy = classificationStrategy;
            _logger = logger;
        }

        // Updated to use productName instead of productId
        public async Task<IActionResult> Index(string productName = "Eco-Friendly Shampoo")
        {
            try
            {
                _logger.LogInformation($"ToxicityController.Index accessed for product: {productName}");
                
                // Get all ingredients for the product by name
                var ingredients = await _ingredientGateway.FindIngredientsByProductName(productName);
                
                if (ingredients == null || !ingredients.Any())
                {
                    _logger.LogWarning($"No ingredients found for product: {productName}");
                    return View(new ToxicityReport { 
                        ProductName = "Product not found",
                        Ingredients = new List<IngredientSDM>() 
                    });
                }

                // Calculate average toxicity
                float avgToxicity = (float)ingredients.Average(i => i.IngredientToxicity);
                
                // Get classification and recommendation using Strategy Pattern
                string classification = await _classificationStrategy.Classify(avgToxicity);
                string recommendation = await _classificationStrategy.GetSafetyRecommendation(avgToxicity);
                
                // Create report
                var report = new ToxicityReport
                {
                    ProductName = productName,
                    Ingredients = ingredients,
                    OverallToxicityScore = avgToxicity,
                    ToxicityClassification = classification,
                    SafetyRecommendation = recommendation
                };
                
                // Add enhanced analysis
                report.ProductSafetyAnalysis = await _classificationStrategy.AnalyzeProductSafety(ingredients);
                
                // Get ingredient-specific recommendations and alternatives
                foreach (var ingredient in ingredients)
                {
                    report.IngredientSpecificRecommendations[ingredient.IngredientId] = 
                        await _classificationStrategy.GetIngredientRecommendation((float)ingredient.IngredientToxicity, ingredient.IngredientName);
                    
                    if (ingredient.IngredientToxicity >= 0.7) // Only get alternatives for high toxicity ingredients
                    {
                        report.IngredientAlternatives[ingredient.IngredientId] = 
                            await _classificationStrategy.RecommendAlternatives((float)ingredient.IngredientToxicity, ingredient.IngredientName);
                    }
                }

                // Add trend analysis - new feature
                report.ToxicityTrends = AnalyzeToxicityTrends(ingredients);
                
                // Add correlation analysis - new feature
                report.CorrelationAnalysis = AnalyzeIngredientCorrelations(ingredients);
                
                // Here you would integrate carbon footprint data when available
                // if (_carbonFootprint != null)
                // {
                //     var carbonData = await _carbonFootprint.GetProductCarbonFootprint(productName);
                //     report.CarbonFootprintData = carbonData;
                // }
                
                return View(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ToxicityController.Index");
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // New method to handle product selection by name
        public async Task<IActionResult> ViewByProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return RedirectToAction("Index");
            }
            
            return RedirectToAction("Index", new { productName = productName });
        }
        
        // New method to generate a downloadable report
        public async Task<IActionResult> GenerateReport(string productName)
        {
            try
            {
                if (string.IsNullOrEmpty(productName))
                {
                    return BadRequest("Product name is required");
                }
                
                _logger.LogInformation($"Generating toxicity report for product: {productName}");
                
                // Get report data the same way as in Index action
                var ingredients = await _ingredientGateway.FindIngredientsByProductName(productName);
                
                if (ingredients == null || !ingredients.Any())
                {
                    return NotFound($"No ingredients found for product: {productName}");
                }
                
                float avgToxicity = (float)ingredients.Average(i => i.IngredientToxicity);
                string classification = await _classificationStrategy.Classify(avgToxicity);
                string recommendation = await _classificationStrategy.GetSafetyRecommendation(avgToxicity);
                var productSafetyAnalysis = await _classificationStrategy.AnalyzeProductSafety(ingredients);
                var toxicityTrends = AnalyzeToxicityTrends(ingredients);
                var correlationAnalysis = AnalyzeIngredientCorrelations(ingredients);
                
                // Generate report content (for simplicity, we're using plain text)
                // In a real application, you might use a PDF library like iTextSharp
                var reportBuilder = new StringBuilder();
                reportBuilder.AppendLine($"TOXICITY ANALYSIS REPORT FOR {productName.ToUpper()}");
                reportBuilder.AppendLine($"Generated on: {DateTime.Now}");
                reportBuilder.AppendLine("===========================================================");
                reportBuilder.AppendLine();
                
                reportBuilder.AppendLine("OVERALL TOXICITY ASSESSMENT:");
                reportBuilder.AppendLine($"Toxicity Score: {avgToxicity:F2}");
                reportBuilder.AppendLine($"Classification: {classification}");
                reportBuilder.AppendLine($"Safety Recommendation: {recommendation}");
                reportBuilder.AppendLine();
                
                reportBuilder.AppendLine("SAFETY ANALYSIS:");
                foreach (var analysis in productSafetyAnalysis)
                {
                    reportBuilder.AppendLine($"- {analysis.Key}: {analysis.Value}");
                }
                reportBuilder.AppendLine();
                
                reportBuilder.AppendLine("TOXICITY DISTRIBUTION:");
                foreach (var trend in toxicityTrends)
                {
                    reportBuilder.AppendLine($"- {trend.Key}: {trend.Value}");
                }
                reportBuilder.AppendLine();
                
                reportBuilder.AppendLine("INGREDIENT INTERACTION ANALYSIS:");
                foreach (var correlation in correlationAnalysis)
                {
                    reportBuilder.AppendLine($"- {correlation.Key}: {correlation.Value}");
                }
                reportBuilder.AppendLine();
                
                reportBuilder.AppendLine("INGREDIENT BREAKDOWN:");
                reportBuilder.AppendLine("ID\tName\tToxicity\tClassification");
                foreach (var ingredient in ingredients)
                {
                    string ingClassification = ingredient.IngredientToxicity >= 0.7 ? "High" : 
                                              ingredient.IngredientToxicity >= 0.3 ? "Moderate" : "Low";
                    
                    reportBuilder.AppendLine($"{ingredient.IngredientId}\t{ingredient.IngredientName}\t{ingredient.IngredientToxicity:F2}\t{ingClassification}");
                }
                reportBuilder.AppendLine();
                
                reportBuilder.AppendLine("HIGH TOXICITY INGREDIENTS AND ALTERNATIVES:");
                var highToxicityIngredients = ingredients.Where(i => i.IngredientToxicity >= 0.7).ToList();
                if (highToxicityIngredients.Any())
                {
                    foreach (var ingredient in highToxicityIngredients)
                    {
                        reportBuilder.AppendLine($"- {ingredient.IngredientName} (Toxicity: {ingredient.IngredientToxicity:F2})");
                        
                        // Get alternatives
                        var alternatives = await _classificationStrategy.RecommendAlternatives((float)ingredient.IngredientToxicity, ingredient.IngredientName);
                        if (alternatives.Any())
                        {
                            reportBuilder.AppendLine("  Recommended alternatives:");
                            foreach (var alternative in alternatives)
                            {
                                reportBuilder.AppendLine($"  * {alternative}");
                            }
                        }
                    }
                }
                else
                {
                    reportBuilder.AppendLine("No high toxicity ingredients found in this product.");
                }
                
                // Return as a downloadable text file
                byte[] reportBytes = Encoding.UTF8.GetBytes(reportBuilder.ToString());
                return File(reportBytes, "text/plain", $"ToxicityReport_{productName.Replace(" ", "_")}.txt");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating toxicity report");
                return StatusCode(500, "An error occurred while generating the report.");
            }
        }

        // Test method to verify routing
        public IActionResult Test()
        {
            return Content("ToxicityController is working correctly");
        }
        
        // New method for trend analysis
        private Dictionary<string, string> AnalyzeToxicityTrends(List<IngredientSDM> ingredients)
        {
            var trends = new Dictionary<string, string>();
            
            // Count ingredients by toxicity level
            int lowToxicity = ingredients.Count(i => i.IngredientToxicity < 0.3);
            int moderateToxicity = ingredients.Count(i => i.IngredientToxicity >= 0.3 && i.IngredientToxicity < 0.7);
            int highToxicity = ingredients.Count(i => i.IngredientToxicity >= 0.7);
            
            // Calculate percentages
            int total = ingredients.Count;
            float lowPercent = total > 0 ? (float)lowToxicity / total * 100 : 0;
            float moderatePercent = total > 0 ? (float)moderateToxicity / total * 100 : 0;
            float highPercent = total > 0 ? (float)highToxicity / total * 100 : 0;
            
            // Add analysis results
            trends["ToxicityDistribution"] = $"Low: {lowToxicity} ({lowPercent:F1}%), Moderate: {moderateToxicity} ({moderatePercent:F1}%), High: {highToxicity} ({highPercent:F1}%)";
            
            // Compare to industry averages (example values)
            float industryAvgHigh = 15.0f; // 15% high toxicity is average
            if (highPercent > industryAvgHigh)
            {
                trends["IndustryComparison"] = $"This product contains {highPercent:F1}% high-toxicity ingredients, which is higher than the industry average of {industryAvgHigh}%.";
            }
            else
            {
                trends["IndustryComparison"] = $"This product contains {highPercent:F1}% high-toxicity ingredients, which is lower than the industry average of {industryAvgHigh}%.";
            }
            
            return trends;
        }
        
        // New method for correlation analysis
        private Dictionary<string, string> AnalyzeIngredientCorrelations(List<IngredientSDM> ingredients)
        {
            var correlations = new Dictionary<string, string>();
            
            // Check for potentially interacting ingredient combinations
            bool containsAcid = ingredients.Any(i => i.IngredientName.Contains("Acid", StringComparison.OrdinalIgnoreCase));
            bool containsAlkaline = ingredients.Any(i => i.IngredientName.Contains("Sodium Hydroxide", StringComparison.OrdinalIgnoreCase) || 
                                                      i.IngredientName.Contains("Potassium Hydroxide", StringComparison.OrdinalIgnoreCase));
            
            if (containsAcid && containsAlkaline)
            {
                correlations["ReactiveIngredients"] = "This product contains both acidic and alkaline ingredients which may neutralize each other, potentially reducing effectiveness.";
            }
            
            // Check for ingredient synergies
            bool containsGlycerin = ingredients.Any(i => i.IngredientName.Contains("Glycerin", StringComparison.OrdinalIgnoreCase));
            bool containsHyaluronicAcid = ingredients.Any(i => i.IngredientName.Contains("Hyaluronic Acid", StringComparison.OrdinalIgnoreCase));
            
            if (containsGlycerin && containsHyaluronicAcid)
            {
                correlations["PositiveSynergy"] = "Glycerin and Hyaluronic Acid work synergistically to improve moisture retention.";
            }
            
            // Check for potentially excessive similar ingredients
            var preservatives = ingredients.Count(i => i.IngredientName.Contains("Paraben", StringComparison.OrdinalIgnoreCase) || 
                                                   i.IngredientName.Contains("Phenoxyethanol", StringComparison.OrdinalIgnoreCase) ||
                                                   i.IngredientName.Contains("Benzoate", StringComparison.OrdinalIgnoreCase));
            
            if (preservatives > 2)
            {
                correlations["PreservativeLoad"] = $"This product contains {preservatives} different preservatives, which may increase irritation potential without providing additional benefits.";
            }
            
            return correlations;
        }

        [HttpPost]
        public async Task<IActionResult> Create(IngredientSDM ingredient)
        {
            try
            {
                _logger.LogInformation($"Processing ingredient: {ingredient.IngredientName}");
                
                if (ModelState.IsValid)
                {
                    await _ingredientGateway.InsertIngredient(ingredient);
                    
                    // Get toxicity classification and recommendation
                    float toxicityScore = (float)ingredient.IngredientToxicity;
                    string classification = await _classificationStrategy.Classify(toxicityScore);
                    string recommendation = await _classificationStrategy.GetSafetyRecommendation(toxicityScore);
                    
                    var report = new ToxicityReport
                    {
                        ProductName = ingredient.IngredientName,
                        Ingredients = new List<IngredientSDM> { ingredient },
                        OverallToxicityScore = toxicityScore,
                        ToxicityClassification = classification,
                        SafetyRecommendation = recommendation
                    };
                    
                    return View("Index", report);
                }
                
                return View("Index", new ToxicityReport { Ingredients = new List<IngredientSDM>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ingredient");
                ModelState.AddModelError("", "An error occurred while processing your request.");
                return View("Index", new ToxicityReport { Ingredients = new List<IngredientSDM>() });
            }
        }
    }
}