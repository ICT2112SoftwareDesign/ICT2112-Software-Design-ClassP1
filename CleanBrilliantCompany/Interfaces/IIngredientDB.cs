using System.Collections.Generic;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IIngredientDB
    {
        List<Ingredient> FindIngredients();
        Ingredient FindIngredientsbyID(int id);
        void InsertIngredient(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
        void DeleteIngredient(int id);
    }
}
