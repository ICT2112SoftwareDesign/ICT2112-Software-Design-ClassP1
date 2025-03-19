using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Models.Control
{
    public class IngredientToxicityAnalysisSDM(
        IIngredientDB ingredientRepository,
        IToxicityClassificationStrategy classificationStrategy) : IToxicity
    {
        private readonly IIngredientDB _ingredientRepository = ingredientRepository;
        private readonly IToxicityClassificationStrategy _classificationStrategy = classificationStrategy;

        public async Task<float> RetrieveToxicity(int ingredientId)
        {
            var ingredient = await _ingredientRepository.FindIngredientsbyID(ingredientId);
            return ingredient != null ? (float)ingredient.IngredientToxicity : 0f;
        }

        public async Task<ToxicityReport?> AnalyzeIngredient(int ingredientId)
        {
            var ingredient = await _ingredientRepository.FindIngredientsbyID(ingredientId);
            if (ingredient == null) return null;

            var toxicityScore = (float)ingredient.IngredientToxicity;
            return new ToxicityReport
            {
                ProductName = $"Ingredient: {ingredient.IngredientName}",
                Ingredients = [ingredient],
                OverallToxicityScore = toxicityScore,
                ToxicityClassification = await _classificationStrategy.Classify(toxicityScore),
                SafetyRecommendation = await _classificationStrategy.GetSafetyRecommendation(toxicityScore)
            };
        }
    }
}
