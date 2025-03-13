using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class IngredientToxicityAnalysisSDM : IToxicity
    {
        private readonly IIngredientDB _ingredientRepository;

        public IngredientToxicityAnalysisSDM(IIngredientDB ingredientRepository)
        {
            _ingredientRepository = ingredientRepository;
        }

        public float RetrieveToxicity(int ingredientId)
        {
            var ingredient = _ingredientRepository.FindIngredientsbyID(ingredientId);
            return ingredient != null ? ingredient.IngredientToxicity : 0;
        }
    }
}
