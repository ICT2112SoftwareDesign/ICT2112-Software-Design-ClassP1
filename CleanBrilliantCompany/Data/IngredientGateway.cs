using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Data;
using Microsoft.Extensions.Logging;

namespace CleanBrilliantCompany.Data
{
    public class IngredientGateway : IIngredientDB
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IngredientGateway> _logger;

        public IngredientGateway(ApplicationDbContext context, ILogger<IngredientGateway> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<IngredientSDM>> FindIngredients()
        {
            return await _context.Ingredients.AsNoTracking().ToListAsync();
        }

        public async Task<IngredientSDM> FindIngredientsbyID(int id)
        {
            return await _context.Ingredients.AsNoTracking().FirstOrDefaultAsync(i => i.IngredientId == id);
        }

        public async Task<List<IngredientSDM>> FindIngredientsByProductID(int productId)
        {
            return await _context.Ingredients.AsNoTracking().Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task InsertIngredient(IngredientSDM ingredient)
        {
            try
            {
                ingredient.CreatedAt = DateTime.Now;
                ingredient.UpdatedAt = DateTime.Now;
                await _context.Ingredients.AddAsync(ingredient);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Ingredient '{ingredient.IngredientName}' added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting ingredient: {ex.Message}");
            }
        }

        public async Task UpdateIngredient(IngredientSDM ingredient)
        {
            try
            {
                ingredient.UpdatedAt = DateTime.Now;
                _context.Ingredients.Update(ingredient);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Ingredient '{ingredient.IngredientName}' updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating ingredient: {ex.Message}");
            }
        }

        public async Task DeleteIngredient(int id)
        {
            try
            {
                var ingredient = await _context.Ingredients.FindAsync(id);
                if (ingredient != null)
                {
                    _context.Ingredients.Remove(ingredient);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Ingredient '{ingredient.IngredientName}' deleted.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting ingredient: {ex.Message}");
            }
        }
    }
}
