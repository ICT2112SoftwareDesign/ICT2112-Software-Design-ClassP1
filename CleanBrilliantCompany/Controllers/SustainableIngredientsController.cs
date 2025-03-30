// -----------------------------------------------------------------
// <filename> SustainableIngredientsController.cs </filename>
// <author> Yuen Wee Kin, Edwin </author>

using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models;
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

            // Retrieve carbon data and join it with product data
            var carbonDataWithSavings = _context.CarbonFootprintRecords
                .Join(
                    // Joining with the Products table (mapped to ProductMapping)
                    _context.Products,
                    // Matching EntityId from CarbonFootprintRecord with ProductId from ProductMapping
                    carbon => carbon.EntityId,
                    // Matching ProductId in ProductMapping
                    product => product.ProductId,
                    (carbon, product) => new CarbonFootprintWithSavings
                    {
                        CarbonFootprintId = carbon.carbonFootprintId,
                        EntityId = carbon.EntityId,
                        EntityType = carbon.EntityType,
                        CarbonEmission = carbon.CarbonEmission,
                        EcoStatus = carbon.EcoStatus,
                        DateCreated = carbon.DateCreated,
                        // Accessing product cost from the mapped table.
                        ProductCost = product.ProductCost,
                        // Calculate savings.
                        CostSavings = CalculateCostSavings(carbon.CarbonEmission, product.ProductCost)
                    }).ToList();

            // Calculate Reduction Percentage and store separately.
            foreach (var data in carbonDataWithSavings)
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
                data.ReductionPercentage = Math.Round(reductionPercentage, 2);
            }

            // Initialize the variables for the total emissions before and after the eco-friendly change.
            double totalCarbonBefore = carbonDataWithSavings.Where(item => item.EcoStatus == "Not Eco-Friendly").Sum(item => item.CarbonEmission);
            double totalCarbonAfter = carbonDataWithSavings.Where(item => item.EcoStatus == "Eco-Friendly").Sum(item => item.CarbonEmission);

            // Initialize the overall reduction percentage and the class for styling.
            double overallReduction = 0;
            string overallReductionClass = string.Empty;

            // Calculate overall reduction.
            if (totalCarbonBefore > 0)
            {
                // Normal reduction calculation.
                overallReduction = ((totalCarbonBefore - totalCarbonAfter) / totalCarbonBefore) * 100;
            }
            else if (totalCarbonBefore == 0 && totalCarbonAfter > 0)
            {
                // If no emissions before, but there are emissions after, consider a 100% increase.
                overallReduction = -100;
            }
            else if (totalCarbonBefore == 0 && totalCarbonAfter == 0)
            {
                // If no emissions before and after, no reduction.
                overallReduction = 0;
            }

            // Decide the overall reduction class based on the value.
            overallReductionClass = overallReduction >= 50 ? "bg-success" : overallReduction >= 20 ? "bg-warning" : "bg-danger";

            // Send carbon data with savings and reduction percentage.
            ViewBag.CarbonData = carbonDataWithSavings;
            ViewBag.OverallReduction = overallReduction;
            ViewBag.OverallReductionClass = overallReductionClass;
            return View(ingredients);
        }

        // Method to calculate cost savings based on the carbon emission and product cost.
        private static double CalculateCostSavings(double carbonEmission, double productCost)
        {
            // We assume that for each kg of CO2 saved, there's a certain percentage cost reduction.
            // 5% savings for each kg of CO2 reduction.
            double savingsPercentage = 0.05;
            return productCost * savingsPercentage * carbonEmission;
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