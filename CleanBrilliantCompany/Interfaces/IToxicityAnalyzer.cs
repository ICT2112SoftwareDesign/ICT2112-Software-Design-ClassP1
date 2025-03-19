using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IToxicityAnalyzer
    {
        Task<ToxicityReport> AnalyzeIngredient(int ingredientId);
        Task<List<ToxicityReport>> AnalyzeProduct(int productId);
    }
}
