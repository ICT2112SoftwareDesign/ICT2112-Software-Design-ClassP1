using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Data;

namespace CleanBrilliantCompany.Data
{
    public class IngredientGateway : IIngredientDB
    {
        private readonly ApplicationDbContext _context;

        public IngredientGateway(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Ingredient> FindIngredients()
        {
            return _context.Ingredients.ToList();
        }

        public Ingredient FindIngredientsbyID(int id)
        {
            return _context.Ingredients.Where(i => i.IngredientId == id).FirstOrDefault();
        }

        public void InsertIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            _context.SaveChanges();
        }

        public void UpdateIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            _context.SaveChanges();
        }

        public void DeleteIngredient(int id)
        {
            var ingredient = _context.Ingredients.Find(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                _context.SaveChanges();
            }
        }
    }
}
