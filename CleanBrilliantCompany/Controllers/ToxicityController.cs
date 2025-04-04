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
        private readonly IProductCF _productCF;
        private readonly IProduct _product;

        public ToxicityController(
            IToxicity toxicity, 
            IIngredientDB ingredientGateway, 
            IToxicityClassificationStrategy classificationStrategy,
            ILogger<ToxicityController> logger,
            ApplicationDbContext dbContext,
            IProductCF productCF,
            IProduct product)
        {
            _toxicity = toxicity;
            _ingredientGateway = ingredientGateway;
            _classificationStrategy = classificationStrategy;
            _logger = logger;
            _dbContext = dbContext;
            _productCF = productCF;
            _product = product;
        }

        // Helper method to get all products using IProduct interface
        private List<dynamic> GetAllProducts()
        {
            try
            {
                // Get all products using the IProduct interface
                var products = _product.getAllProducts();
                
                // Convert to dynamic list with ProductId and ProductName properties
                return products.Select(p => new { 
                    ProductId = p.ProductId, 
                    ProductName = p.ProductName 
                }).Cast<dynamic>().ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching products using IProduct interface");
                
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
        private List<string> GetAllProductNames()
        {
            var products = GetAllProducts();
            return products.Select(p => (string)p.ProductName).ToList();
        }

        // Helper method to get product by name
        private Product GetProductByName(string productName)
        {
            try
            {
                var products = _product.getAllProducts();
                return products.FirstOrDefault(p => p.ProductName == productName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error finding product by name: {productName}");
                return null;
            }
        }

        // Main entry point for toxicity analysis
        public async Task<IActionResult> Index(string productName = "Eco-Friendly Shampoo")
        {
            try
            {
                _logger.LogInformation($"ToxicityController.Index accessed for product: {productName}");
                
                // Get all products for the dropdown
                var allProducts = GetAllProductNames();
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

                // Get product using IProduct interface
                var product = GetProductByName(productName);

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
                
                // Get carbon footprint data if product exists
                if (product != null)
                {
                    try
                    {
                        int productId = product.ProductId;
                        
                        // Get carbon footprint value for the product
                        double carbonFootprint = _productCF.getProductCarbonFootprint(productId);
                        
                        // Get eco status for the product if available
                        var allProductCFs = _productCF.getAllProductCarbonFootprint();
                        string ecoStatus = "Unknown";
                        
                        // Find the product's eco status in the collection
                        var productCF = allProductCFs.FirstOrDefault(p => p.retrieveProductId() == productId);
                        if (productCF != null)
                        {
                            ecoStatus = productCF.retrieveEcoStatus();
                        }
                        
                        // Add carbon footprint data to ViewBag
                        ViewBag.CarbonFootprint = carbonFootprint;
                        ViewBag.EcoStatus = ecoStatus;
                        ViewBag.CarbonFootprintImpact = AnalyzeCarbonFootprintImpact(carbonFootprint, avgToxicity);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error retrieving carbon footprint data for product: {productName}");
                        // Don't set the ViewBag values if there's an error
                    }
                }
                
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
                
                return View(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ToxicityController.Index");
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // Helper method to analyze carbon footprint impact
        private string AnalyzeCarbonFootprintImpact(double carbonFootprint, float toxicityScore)
        {
            if (carbonFootprint <= 0)
                return "No carbon footprint data available for this product.";
                
            string impact;
            if (carbonFootprint < 50)
                impact = "Low environmental impact. ";
            else if (carbonFootprint < 100)
                impact = "Moderate environmental impact. ";
            else
                impact = "High environmental impact. ";
                
            if (toxicityScore > 0.7)
                impact += "High toxicity ingredients typically contribute significantly to the carbon footprint during production.";
            else if (toxicityScore > 0.3)
                impact += "Some moderate-toxicity ingredients may contribute to the carbon footprint during manufacturing.";
            else
                impact += "Low-toxicity ingredients generally have minimal impact on carbon emissions.";
                
            return impact;
        }

        // Separate page for Add Ingredient form
        public IActionResult AddIngredient(string productName = null)
        {
            var productList = GetAllProducts();
            ViewBag.ProductList = productList;
            
            // If productName is specified, find its ID and pre-select
            if (!string.IsNullOrEmpty(productName))
            {
                var product = GetProductByName(productName);
                if (product != null)
                {
                    ViewBag.SelectedProductId = product.ProductId;
                }
            }
            
            return View(new IngredientSDM());
        }

        // Method to handle product selection by name
        public IActionResult ViewByProductName(string productName)
        {
            if (string.IsNullOrEmpty(productName))
            {
                return RedirectToAction("Index");
            }
            
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
                    
                    // Get product name using IProduct
                    var product = _product.getProductDetails(ingredient.ProductId);
                    string productName = product != null ? product.ProductName : "Unknown Product";
                    
                    // Redirect to view product analysis that contains this ingredient
                    return RedirectToAction("Index", new { productName = productName });
                }
                
                // If ModelState is invalid, repopulate the product list for the dropdown
                var productList = GetAllProducts();
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
                var productList = GetAllProducts();
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
                    
                    // Get product name using IProduct
                    var product = _product.getProductDetails(ingredient.ProductId);
                    string productName = product != null ? product.ProductName : "Unknown Product";
                    
                    return RedirectToAction("Index", new { productName = productName });
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
                
                // Get product name using IProduct before deleting the ingredient
                var product = _product.getProductDetails(productId);
                string productName = product != null ? product.ProductName : "Unknown Product";
                
                // Delete the ingredient
                await _ingredientGateway.DeleteIngredient(ingredientId);
                
                _logger.LogInformation($"Deleted ingredient ID {ingredientId}: {ingredientName}");
                TempData["SuccessMessage"] = $"Ingredient {ingredientName} deleted successfully.";
                
                return RedirectToAction("Index", new { productName = productName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting ingredient ID {ingredientId}");
                TempData["ErrorMessage"] = "An error occurred while deleting the ingredient.";
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
                
                // Get product using IProduct interface
                var product = GetProductByName(productName);
                if (product == null)
                {
                    return NotFound($"Product not found: {productName}");
                }
                
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
                
                // Get carbon footprint data
                double carbonFootprint = 0;
                string ecoStatus = "Unknown";
                string carbonImpact = "No carbon footprint data available";
                
                try 
                {
                    int productId = product.ProductId;
                    
                    // Get carbon footprint value for the product
                    carbonFootprint = _productCF.getProductCarbonFootprint(productId);
                    
                    // Get eco status for the product if available
                    var allProductCFs = _productCF.getAllProductCarbonFootprint();
                    var productCF = allProductCFs.FirstOrDefault(p => p.retrieveProductId() == productId);
                    if (productCF != null)
                    {
                        ecoStatus = productCF.retrieveEcoStatus();
                    }
                    
                    carbonImpact = AnalyzeCarbonFootprintImpact(carbonFootprint, avgToxicity);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error retrieving carbon footprint data for product: {productName}");
                    // Keep default values if there's an error
                }
                
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
                reportBuilder.AppendLine($"Carbon Footprint Value: {carbonFootprint} units");
                reportBuilder.AppendLine($"Eco Status: {ecoStatus}");
                reportBuilder.AppendLine($"Environmental Impact: {carbonImpact}");
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