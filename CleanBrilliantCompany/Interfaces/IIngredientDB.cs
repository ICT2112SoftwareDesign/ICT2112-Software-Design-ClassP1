using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IIngredientDB
    {
        Task<List<IngredientSDM>> FindIngredients();
        Task<IngredientSDM> FindIngredientsbyID(int id);
        Task<List<IngredientSDM>> FindIngredientsByProductID(int productId);
        Task<List<IngredientSDM>> FindIngredientsByProductName(string productName);
        Task InsertIngredient(IngredientSDM ingredient);
        Task UpdateIngredient(IngredientSDM ingredient);
        Task DeleteIngredient(int id);
    }
}