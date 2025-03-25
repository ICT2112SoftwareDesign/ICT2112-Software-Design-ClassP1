using System.Threading.Tasks;
using CleanBrilliantCompany.DTO;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IToxicity
    {
        Task<float> RetrieveToxicity(int ingredientId);
        Task<ToxicityReport> AnalyzeIngredient(int ingredientId);
    }
}