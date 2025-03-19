using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    public class ToxicityPageController : Controller
    {
        private readonly IToxicity _toxicity;
        private readonly IIngredientDB _ingredientGateway;
        private readonly IToxicityClassificationStrategy _classificationStrategy;
        private readonly ILogger<ToxicityPageController> _logger;

        public ToxicityPageController(
            IToxicity toxicity, 
            IIngredientDB ingredientGateway, 
            IToxicityClassificationStrategy classificationStrategy,
            ILogger<ToxicityPageController> logger)
        {
            _toxicity = toxicity;
            _ingredientGateway = ingredientGateway;
            _classificationStrategy = classificationStrategy;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int ingredientId = 1)
        {
            try
            {
                _logger.LogInformation($"ToxicityPageController.Index accessed with ingredientId: {ingredientId}");
                
                var ingredient = await _ingredientGateway.FindIngredientsbyID(ingredientId);
                if (ingredient == null)
                {
                    _logger.LogWarning($"Ingredient with ID {ingredientId} not found");
                    // Return an empty model for the form
                    return View("Toxicity", new ToxicityReport { Ingredients = new List<IngredientSDM>() });
                }

                float toxicityScore = await _toxicity.RetrieveToxicity(ingredientId);
                _logger.LogInformation($"Retrieved toxicity score: {toxicityScore} for ingredient: {ingredient.IngredientName}");
                
                var toxicityReport = new ToxicityReport
                {
                    ProductName = ingredient.IngredientName,
                    Ingredients = new List<IngredientSDM> { ingredient },
                    OverallToxicityScore = toxicityScore,
                    ToxicityClassification = await _classificationStrategy.Classify(toxicityScore),
                    SafetyRecommendation = await _classificationStrategy.GetSafetyRecommendation(toxicityScore)
                };

                return View("Toxicity", toxicityReport);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ToxicityPageController.Index");
                ModelState.AddModelError("", "An error occurred while retrieving toxicity data. Please try again.");
                return View("Toxicity", new ToxicityReport { Ingredients = new List<IngredientSDM>() });
            }
        }
    }
}