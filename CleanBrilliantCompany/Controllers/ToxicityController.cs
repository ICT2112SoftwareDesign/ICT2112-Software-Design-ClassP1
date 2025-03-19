using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    public class ToxicityController : Controller
    {
        private readonly IToxicity _toxicity;
        private readonly IIngredientDB _ingredientGateway;
        private readonly IToxicityClassificationStrategy _classificationStrategy;
        private readonly ILogger<ToxicityController> _logger;

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

        public async Task<IActionResult> Index(int productId = 2) // Default to Eco-Friendly Shampoo
        {
            try
            {
                _logger.LogInformation($"ToxicityController.Index action accessed for productId: {productId}");
                
                // Get all ingredients for the product
                var ingredients = await _ingredientGateway.FindIngredientsByProductID(productId);
                
                if (ingredients == null || !ingredients.Any())
                {
                    _logger.LogWarning($"No ingredients found for product ID {productId}");
                    return View(new ToxicityReport { 
                        ProductName = "Product not found",
                        Ingredients = new List<IngredientSDM>() 
                    });
                }

                // Calculate average toxicity
                float avgToxicity = (float)ingredients.Average(i => i.IngredientToxicity);
                
                // Get classification and recommendation
                string classification = await _classificationStrategy.Classify(avgToxicity);
                string recommendation = await _classificationStrategy.GetSafetyRecommendation(avgToxicity);
                
                // Create report
                var report = new ToxicityReport
                {
                    ProductName = $"Product ID: {productId}",
                    Ingredients = ingredients,
                    OverallToxicityScore = avgToxicity,
                    ToxicityClassification = classification,
                    SafetyRecommendation = recommendation
                };
                
                return View(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ToxicityController.Index");
                return View("Error", new ErrorViewModel { RequestId = ex.Message });
            }
        }

        // Test method to verify routing
        public IActionResult Test()
        {
            return Content("ToxicityController is working correctly - Your database has Product, Ingredient, ToxicityReport, and ReportLog tables.");
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
                    _logger.LogInformation($"Ingredient '{ingredient.IngredientName}' added successfully");
                    
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
                
                _logger.LogWarning("Invalid model state for ingredient submission");
                return View("Index", new ToxicityReport { Ingredients = new List<IngredientSDM>() });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing ingredient");
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again.");
                return View("Index", new ToxicityReport { Ingredients = new List<IngredientSDM>() });
            }
        }
        
        // Add a method to view ingredients by product ID
        public async Task<IActionResult> ViewByProduct(int productId)
        {
            // Instead of simply calling Index which returns View()
            // We need to specify which view to use explicitly
            var ingredients = await _ingredientGateway.FindIngredientsByProductID(productId);
            
            if (ingredients == null || !ingredients.Any())
            {
                _logger.LogWarning($"No ingredients found for product ID {productId}");
                return View("Index", new ToxicityReport { 
                    ProductName = "Product not found",
                    Ingredients = new List<IngredientSDM>() 
                });
            }

            // Calculate average toxicity
            float avgToxicity = (float)ingredients.Average(i => i.IngredientToxicity);
            
            // Get classification and recommendation
            string classification = await _classificationStrategy.Classify(avgToxicity);
            string recommendation = await _classificationStrategy.GetSafetyRecommendation(avgToxicity);
            
            // Create report
            var report = new ToxicityReport
            {
                ProductName = $"Product ID: {productId}",
                Ingredients = ingredients,
                OverallToxicityScore = avgToxicity,
                ToxicityClassification = classification,
                SafetyRecommendation = recommendation
            };
            
            // Explicitly specify the view to use
            return View("Index", report);
        }
    }
}