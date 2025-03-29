using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Data;
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
        private readonly ApplicationDbContext _dbContext;

        public ToxicityController(
            IToxicity toxicity, 
            IIngredientDB ingredientGateway, 
            IToxicityClassificationStrategy classificationStrategy,
            ILogger<ToxicityController> logger,
            ApplicationDbContext dbContext)
        {
            _toxicity = toxicity;
            _ingredientGateway = ingredientGateway;
            _classificationStrategy = classificationStrategy;
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Toxicity()
        {
            return View(new ToxicityReport {
                ProductName = "Select an ingredient to analyze",
                Ingredients = new List<IngredientSDM>()
            });
        }

        // Helper method to get all products from the database
        private async Task<List<dynamic>> GetAllProductsAsync()
        {
            try
            {
                // Using the ProductMapping class that maps to the Product table
                var products = await _dbContext.Products
                    .AsNoTracking()
                    .Select(p => new { p.ProductId, p.ProductName })
                    .ToListAsync();
                
                return products.Cast<dynamic>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products from database");
                
                // Return a default list in case of error
                return new List<dynamic>
                {
                    new { ProductId = 2, ProductName = "Eco-Friendly Shampoo" },
                    new { ProductId = 14, ProductName = "sp1o0" },
                    new { ProductId = 23, ProductName = "Bleach" },
                    new { ProductId = 167, ProductName = "Organic Surface Spray" },
                    new { ProductId = 203, ProductName = "All-Purpose Cleaner" }
                };
            }
        }

        // Helper method to get all product names
        private async Task<List<string>> GetAllProductNamesAsync()
        {
            var products = await GetAllProductsAsync();
            return products.Select(p => (string)p.ProductName).ToList();
        }



        // Main entry point for toxicity analysis
        public async Task<IActionResult> Index(string productName = "Eco-Friendly Shampoo")
        {
            try
            {
                _logger.LogInformation($"ToxicityController.Index accessed for product: {productName}");
                
                // Get all products for the dropdown
                var allProducts = await GetAllProductNamesAsync();
                ViewBag.Products = allProducts;
                ViewBag.CurrentProduct = productName;
                
                // Get all ingredients for the product by name
                var ingredients = await _ingredientGateway.FindIngredientsByProductName(productName);
                
                if (ingredients == null || !ingredients.Any())
                {
                    _logger.LogWarning($"No ingredients found for product: {productName}");
                    return View(new ToxicityReport { 
                        ProductName = productName,
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

                // Add trend analysis
                report.ToxicityTrends = AnalyzeToxicityTrends(ingredients);
                
                // Add correlation analysis
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

        // Separate page for Add Ingredient form
        public async Task<IActionResult> AddIngredient(string productName = null)
        {
            var productList = await GetAllProductsAsync();
            ViewBag.ProductList = productList;
            
            // If productName is specified, find its ID and pre-select
            if (!string.IsNullOrEmpty(productName))
            {
                var product = productList.FirstOrDefault(p => p.ProductName == productName);
                if (product != null)
                {
                    ViewBag.SelectedProductId = product.ProductId;
                }
            }
            
            return View(new IngredientSDM());
        }

        // Method to handle product selection by name
        public async Task<IActionResult> ViewByProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return RedirectToAction("Index");
            }
            
            // Added an await call to make this truly async
            await Task.Yield(); // This creates an awaitable task so the method is truly async
            return RedirectToAction("Index", new { productName = productName });
        }
        
        // Method to analyze a specific ingredient
        public async Task<IActionResult> AnalyzeIngredient(int ingredientId)
        {
            try
            {
                var report = await _toxicity.AnalyzeIngredient(ingredientId);
                
                if (report == null)
                {
                    TempData["ErrorMessage"] = "Ingredient not found";
                    return RedirectToAction("Index");
                }
                
                return View("Toxicity", report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error analyzing ingredient {ingredientId}");
                TempData["ErrorMessage"] = "An error occurred during analysis";
                return RedirectToAction("Index");
            }
        }
        
        // Compare toxicity between products
        public async Task<IActionResult> CompareProducts(string product1Name, string product2Name)
        {
            try
            {
                // Get all products for dropdowns
                var allProducts = await GetAllProductNamesAsync();
                ViewBag.Products = allProducts;
                
                if (string.IsNullOrEmpty(product1Name) || string.IsNullOrEmpty(product2Name))
                {
                    TempData["ErrorMessage"] = "Both products must be specified for comparison";
                    return RedirectToAction("Index");
                }
                
                var ingredients1 = await _ingredientGateway.FindIngredientsByProductName(product1Name);
                var ingredients2 = await _ingredientGateway.FindIngredientsByProductName(product2Name);
                
                if (ingredients1 == null || !ingredients1.Any() || ingredients2 == null || !ingredients2.Any())
                {
                    TempData["ErrorMessage"] = "One or both products have no ingredients to compare";
                    return RedirectToAction("Index");
                }
                
                var avgToxicity1 = (float)ingredients1.Average(i => i.IngredientToxicity);
                var avgToxicity2 = (float)ingredients2.Average(i => i.IngredientToxicity);
                
                var comparisonViewModel = new ProductComparisonViewModel
                {
                    Product1Name = product1Name,
                    Product2Name = product2Name,
                    Product1ToxicityScore = avgToxicity1,
                    Product2ToxicityScore = avgToxicity2,
                    Product1HighToxicityCount = ingredients1.Count(i => i.IngredientToxicity >= 0.7),
                    Product2HighToxicityCount = ingredients2.Count(i => i.IngredientToxicity >= 0.7),
                    DifferencePct = Math.Abs(avgToxicity1 - avgToxicity2) * 100,
                    MoreToxicProduct = avgToxicity1 > avgToxicity2 ? product1Name : product2Name
                };
                
                return View("CompareProducts", comparisonViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error comparing products");
                TempData["ErrorMessage"] = "Error comparing products";
                return RedirectToAction("Index");
            }
        }
        
        // Method to generate a downloadable report
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
                
                // Generate report content
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
                
                // Add carbon footprint related section
                reportBuilder.AppendLine();
                reportBuilder.AppendLine("CARBON FOOTPRINT CONSIDERATIONS:");
                reportBuilder.AppendLine("High toxicity ingredients often have higher carbon footprints due to intensive manufacturing processes.");
                reportBuilder.AppendLine($"Replacing high toxicity ingredients with alternatives could reduce carbon emissions by approximately {highToxicityIngredients.Count * 5}%.");
                
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
        
        // Method for handling the creation of new ingredients
        // Method for handling the creation of new ingredients
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
                    
                    // Record the newly added ingredient in the log
                    _logger.LogInformation($"Added new ingredient: {ingredient.IngredientName} with toxicity {toxicityScore} ({classification})");
                    
                    // Add success message
                    TempData["SuccessMessage"] = $"Ingredient {ingredient.IngredientName} added successfully and analyzed.";
                    
                    // Redirect to view product analysis that contains this ingredient
                    return RedirectToAction("Index", new { productName = await GetProductNameById(ingredient.ProductId) });
                }
                
                // If ModelState is invalid, repopulate the product list for the dropdown
                var productList = await GetAllProductsAsync();
                ViewBag.ProductList = productList;
                ViewBag.SelectedProductId = ingredient.ProductId;
                
                TempData["ErrorMessage"] = "There was an error with the ingredient data. Please check your inputs.";
                return View("AddIngredient", ingredient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ingredient");
                ModelState.AddModelError("", "An error occurred while processing your request.");
                
                // Repopulate the product list for the dropdown
                var productList = await GetAllProductsAsync();
                ViewBag.ProductList = productList;
                ViewBag.SelectedProductId = ingredient.ProductId;
                
                TempData["ErrorMessage"] = "An error occurred while processing your request.";
                return View("AddIngredient", ingredient);
            }
        }
        
        // Method for updating an existing ingredient
        [HttpPost]
        public async Task<IActionResult> Update(IngredientSDM ingredient)
        {
            try
            {
                _logger.LogInformation($"Updating ingredient ID {ingredient.IngredientId}: {ingredient.IngredientName}");
                
                if (ModelState.IsValid)
                {
                    // Check if ingredient exists
                    var existingIngredient = await _ingredientGateway.FindIngredientsbyID(ingredient.IngredientId);
                    if (existingIngredient == null)
                    {
                        TempData["ErrorMessage"] = "Ingredient not found.";
                        return RedirectToAction("Index");
                    }
                    
                    // Update the ingredient
                    await _ingredientGateway.UpdateIngredient(ingredient);
                    
                    TempData["SuccessMessage"] = $"Ingredient {ingredient.IngredientName} updated successfully.";
                    return RedirectToAction("Index", new { productName = await GetProductNameById(ingredient.ProductId) });
                }
                
                TempData["ErrorMessage"] = "There was an error with the ingredient data. Please check your inputs.";
                return View("EditIngredient", ingredient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating ingredient ID {ingredient.IngredientId}");
                TempData["ErrorMessage"] = "An error occurred while updating the ingredient.";
                return View("EditIngredient", ingredient);
            }
        }
        
        // Method for deleting an ingredient
        [HttpPost]
        public async Task<IActionResult> Delete(int ingredientId)
        {
            try
            {
                // Get ingredient details before deletion for logging and redirection
                var ingredient = await _ingredientGateway.FindIngredientsbyID(ingredientId);
                if (ingredient == null)
                {
                    TempData["ErrorMessage"] = "Ingredient not found.";
                    return RedirectToAction("Index");
                }
                
                int productId = ingredient.ProductId;
                string ingredientName = ingredient.IngredientName;
                
                // Delete the ingredient
                await _ingredientGateway.DeleteIngredient(ingredientId);
                
                _logger.LogInformation($"Deleted ingredient ID {ingredientId}: {ingredientName}");
                TempData["SuccessMessage"] = $"Ingredient {ingredientName} deleted successfully.";
                
                return RedirectToAction("Index", new { productName = await GetProductNameById(productId) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ingredient ID {ingredientId}");
                TempData["ErrorMessage"] = "An error occurred while deleting the ingredient.";
                return RedirectToAction("Index");
            }
        }
        
        // Helper method for trend analysis
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
            
            // Add carbon footprint implications
            if (highToxicity > 0)
            {
                trends["CarbonFootprintImplication"] = $"High-toxicity ingredients typically have {highToxicity * 15}% higher carbon footprint during production. Consider alternatives.";
            }
            else
            {
                trends["CarbonFootprintImplication"] = "This product's ingredients have minimal toxicity-related carbon footprint impact.";
            }
            
            return trends;
        }
        
        // Helper method for correlation analysis
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
            
            // Add information about environmental impact related to carbon footprint
            float avgToxicity = (float)ingredients.Average(i => i.IngredientToxicity);
            correlations["EnvironmentalImpact"] = avgToxicity switch
            {
                < 0.3f => "Low environmental impact. Less carbon footprint during production and disposal.",
                < 0.7f => "Moderate environmental impact. Some ingredients may contribute to carbon emissions.",
                _ => "High environmental impact. Ingredients may contribute significantly to carbon footprint."
            };
            
            return correlations;
        }
        
        // Helper method to get product name from ID
        private async Task<string> GetProductNameById(int productId)
        {
            try
            {
                var product = await _dbContext.Products
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.ProductId == productId);
                
                return product?.ProductName ?? "Unknown Product";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting product name for ID {productId}");
                return "Unknown Product";
            }
        }
    }
    
    // ViewModel for product comparison
    public class ProductComparisonViewModel
    {
        public string Product1Name { get; set; } = string.Empty;
        public string Product2Name { get; set; } = string.Empty;
        public float Product1ToxicityScore { get; set; }
        public float Product2ToxicityScore { get; set; }
        public int Product1HighToxicityCount { get; set; }
        public int Product2HighToxicityCount { get; set; }
        public float DifferencePct { get; set; }
        public string MoreToxicProduct { get; set; } = string.Empty;
    }
}