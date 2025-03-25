using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using System.Collections.Generic;
using CleanBrilliantCompany.DTO;

namespace CleanBrilliantCompany.Models.Control
{
    public class IngredientToxicityAnalysisSDM : IToxicity
    {
        private readonly IIngredientDB _ingredientRepository;
        private readonly IToxicityClassificationStrategy _classificationStrategy;

        public IngredientToxicityAnalysisSDM(
            IIngredientDB ingredientRepository,
            IToxicityClassificationStrategy classificationStrategy)
        {
            _ingredientRepository = ingredientRepository;
            _classificationStrategy = classificationStrategy;
        }

        public async Task<float> RetrieveToxicity(int ingredientId)
        {
            var ingredient = await _ingredientRepository.FindIngredientsbyID(ingredientId);
            return ingredient != null ? (float)ingredient.IngredientToxicity : 0f;
        }

        public async Task<ToxicityReport> AnalyzeIngredient(int ingredientId)
        {
            var ingredient = await _ingredientRepository.FindIngredientsbyID(ingredientId);
            if (ingredient == null) return null;

            var toxicityScore = (float)ingredient.IngredientToxicity;
            
            var report = new ToxicityReport
            {
                ProductName = $"Ingredient: {ingredient.IngredientName}",
                Ingredients = new List<IngredientSDM> { ingredient },
                OverallToxicityScore = toxicityScore,
                ToxicityClassification = await _classificationStrategy.Classify(toxicityScore),
                SafetyRecommendation = await _classificationStrategy.GetSafetyRecommendation(toxicityScore)
            };
            
            // Add ingredient-specific recommendation
            report.IngredientSpecificRecommendations[ingredient.IngredientId] = 
                await _classificationStrategy.GetIngredientRecommendation(toxicityScore, ingredient.IngredientName);
            
            // Add alternatives if high toxicity
            if (toxicityScore >= 0.7)
            {
                report.IngredientAlternatives[ingredient.IngredientId] =
                    await _classificationStrategy.RecommendAlternatives(toxicityScore, ingredient.IngredientName);
            }
            
            return report;
        }
    }
}