// -----------------------------------------------------------------
// <filename> SustainableIngredientsController.cs </filename>
// <author> Yuen Wee Kin, Edwin </author>

using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class SustainableIngredientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Inject DbContext
        public SustainableIngredientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Action method for the sustainable ingredients page. 
        /// </summary>
        /// <returns>View displaying the sustainable resources.</returns>
        public IActionResult Index()
        {
            // Hardcoded list of products.
            var products = new List<dynamic>
            {
                new {Id = 101, ProductName = "Eco-Friendly Detergent"},
                new {Id = 102, ProductName = "Biodegradable Packaging"},
                new {Id = 103, ProductName = "Sustainable Textile"}
            };

            // Send to view.
            ViewBag.Products = products;
            var ingredients = GetMockSustainableIngredients();

            // Retrieve carbon footprint data from the database and cast it properly.
            var carbonData = _context.CarbonFootprintRecords.Select(data => new CarbonFootprintRecord
            {
                carbonFootprintId = data.carbonFootprintId,
                EntityId = data.EntityId,
                EntityType = data.EntityType,
                CarbonEmission = data.CarbonEmission,
                EcoStatus = data.EcoStatus,
                DateCreated = data.DateCreated,
            }).ToList();

            // Calculate Reduction Percentage and store separately.
            foreach (var data in carbonData)
            {
                double reductionPercentage = 0;
                // Get baseline emission logic.            
                double baselineEmissions = GetBaselineEmission(data.EntityId);

                // If there is a baseline emission value, calculate the reduction percentage.
                if (baselineEmissions > 0)
                {
                    reductionPercentage = (baselineEmissions - data.CarbonEmission) / baselineEmissions * 100;
                }

                // Update the emission value as reduction percentage.
                data.CarbonEmission = Math.Round(reductionPercentage, 2);
            }

            // Send carbon data to the view.
            ViewBag.CarbonData = carbonData;
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
        public IActionResult ProcessResource(int ProductId, string IngredientName, int ThresholdQuantity, string MeasurementUnit)
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


                // ProcessResource(newIngredient);
                // Redirect to list view.
                return RedirectToAction("Index");
            }

            // If invalid data, return to the form with an error message.
            ViewBag.ErrorMessage = "Invalid input. Please check your data.";
            return View();
        }

        // private void AddIngredient(IngredientSDM ingredient)
        // {
        //     Console.WriteLine($"New Ingredient Created: {ingredient.IngredientName}, Status:{ingredient.ReorderStatus}");
        //     // TODO: Add to database.
        //     // TODO: Save changes to DB
        // }

        // Fetches the baseline emission for a specific EntityId (Product or Order).
        private double GetBaselineEmission(int entityId)
        {
            // Get the highest "Not Eco-Friendly" carbon emission as the baseline.
            var baselineRecord = _context.CarbonFootprintRecords
                .Where(c => c.EntityId == entityId && c.EcoStatus == "Not Eco-Friendly")
                // Highest emission is the worst case.
                .OrderByDescending(c => c.CarbonEmission)
                .FirstOrDefault();

            return (double)(baselineRecord?.CarbonEmission ?? 0);
        }
    }
}