// -----------------------------------------------------------------
// <filename> SustainableIngredientsController.cs </filename>
// <author> Yuen Wee Kin, Edwin </author>

using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class SustainableIngredientsController : Controller
    {
        /// <summary>
        /// Action method for the sustainable ingredients page. 
        /// </summary>
        /// <returns>View displaying the sustainable resources.</returns>
        public IActionResult Index()
        {
            var ingredients = GetMockSustainableIngredients();
            return View(ingredients);
        }

        /// <summary>
        /// Retrieves a static list of mock sustainable ingredients.
        /// This function serves as temporary data until database integration is implemented.
        /// </summary>
        /// <returns>List of sustainable ingredients.</returns>
        private List<IngredientSDM> GetMockSustainableIngredients()
        {
            return new List<IngredientSDM> {
                new IngredientSDM {
                    IngredientId = 1,
                    ProductId = 101,
                    IngredientName = "Recycled Paper",
                    IngredientToxicity = 0.0,
                    ThresholdQuantity = 10,
                    MeasurementUnit = "pcs",
                    ReorderStatus = false,
                    CreatedAt = DateTime.Now.AddMonths(-2),
                    UpdatedAt = DateTime.Now,
                },
                new IngredientSDM {
                    IngredientId = 2,
                    ProductId = 102,
                    IngredientName = "Bamboo Fibres",
                    IngredientToxicity = 0.0,
                    ThresholdQuantity = 10,
                    MeasurementUnit = "kg",
                    ReorderStatus = true,
                    CreatedAt = DateTime.Now.AddMonths(-3),
                    UpdatedAt = DateTime.Now,
                },
                new IngredientSDM {
                    IngredientId = 3,
                    ProductId = 103,
                    IngredientName = "Organic Cotton",
                    IngredientToxicity = 0.0,
                    ThresholdQuantity = 10,
                    MeasurementUnit = "kg",
                    ReorderStatus = true,
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    UpdatedAt = DateTime.Now,
                }
            };
        }
    }
}