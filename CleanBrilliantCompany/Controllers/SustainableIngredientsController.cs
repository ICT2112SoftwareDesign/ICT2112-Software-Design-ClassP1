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
            // Hardcoded list of products.
            var products = new List<dynamic>
            {
                new {Id = 1, ProductName = "Eco-Friendly Detergent"},
                new {Id = 2, ProductName = "Biodegradable Packaging"},
                new {Id = 3, ProductName = "Sustainable Textile"}
            };

            // Send to view.
            ViewBag.Products = products;


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
                    Quantity = 50,
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
                    Quantity = 5,
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
                    Quantity = 1,
                    MeasurementUnit = "kg",
                    ReorderStatus = true,
                    CreatedAt = DateTime.Now.AddMonths(-1),
                    UpdatedAt = DateTime.Now,
                }
            };
        }

        [HttpPost]
        public IActionResult Create(int ProductId, string IngredientName, int ThresholdQuantity, string MeasurementUnit)
        {
            var validUnits = new List<string> { "kg", "g", "l", "ml", "pcs" };

            // Log input values.
            Console.WriteLine($"Received Data -> ProductId: {ProductId}, IngredientName: {IngredientName}, ThresholdQuantity: {ThresholdQuantity}, MeasurementUnit: {MeasurementUnit}");

            if (!string.IsNullOrEmpty(IngredientName) && ThresholdQuantity >= 0 && validUnits.Contains(MeasurementUnit))
            {
                var newIngredient = new IngredientSDM
                {
                    // Assign the selected product Id.
                    ProductId = ProductId,
                    IngredientName = IngredientName,
                    ThresholdQuantity = ThresholdQuantity,
                    MeasurementUnit = MeasurementUnit,
                    ReorderStatus = ThresholdQuantity < 10,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                };

                Console.WriteLine($"New Ingredient Created: {newIngredient.IngredientName}, Status:{newIngredient.ReorderStatus}");

                // TODO: Add to database.
                // TODO: Save changes to DB

                // Redirect to list view.
                return RedirectToAction("Index");
            }

            // If invalid data, return to the form with an error message.
            ViewBag.ErrorMessage = "Invalid input. Please check your data.";
            return View();
        }
    }
}