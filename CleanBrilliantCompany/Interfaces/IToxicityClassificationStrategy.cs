using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IToxicityClassificationStrategy
    {
        Task<string> Classify(float toxicityScore);
        Task<string> GetSafetyRecommendation(float toxicityScore);
        Task<string> GetIngredientRecommendation(float toxicityScore, string ingredientName);
        Task<Dictionary<string, string>> AnalyzeProductSafety(List<IngredientSDM> ingredients);
        Task<List<string>> RecommendAlternatives(float toxicityScore, string ingredientName);
    }
}