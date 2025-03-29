using CleanBrilliantCompany.Domain;
using CleanBrilliantCompany.DomainControl;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CleanBrilliantCompany.Controllers
{
    public class DashboardPageController : Controller
    {
        private readonly ProductCarbonFootprintControl _productCFControl;
        private readonly ItemCarbonFootprintControl _itemCFControl;
        private readonly OrderCarbonFootprintControl _orderCFControl;

        private readonly CarbonFootprintCalculatorControl _carbonFootprintCalculatorControl;
        private readonly ProductControl _productControl; // just for mocking purposes

        public DashboardPageController(ProductCarbonFootprintControl productCFControl, ItemCarbonFootprintControl itemCFControl, OrderCarbonFootprintControl orderCFControl, CarbonFootprintCalculatorControl carbonFootprintCalculatorControl, ProductControl productControl)
        {
            _productCFControl = productCFControl;
            _itemCFControl = itemCFControl;
            _orderCFControl = orderCFControl;
            
            // for product, item creation mockup
            _carbonFootprintCalculatorControl = carbonFootprintCalculatorControl;
            _productControl = productControl ?? throw new ArgumentNullException(nameof(productControl));
        }

        public IActionResult Dashboard()
        {
            var products = _productCFControl.getAllProductCarbonFootprint();
            var ecoFriendlyCount = products.Count(p => p.calculateSelfEmission() < 250);
            var nonEcoFriendlyCount = products.Count - ecoFriendlyCount;

            var orders = _orderCFControl.getAllOrderCarbonFootprint();

            var transportModeCounts = orders
            .GroupBy(o => o.retrieveTransportMode().ToUpper())
            .ToDictionary(g => g.Key, g => g.Count());

            var viewModel = new CarbonDashboardViewModel
            {
                TotalProductCF = _productCFControl.getTotalCarbonFootprint(),
                TotalItemCF = _itemCFControl.getTotalCarbonFootprint(),
                TotalOrderCF = _orderCFControl.getTotalCarbonFootprint(),

                EcoFriendlyProductCount = ecoFriendlyCount,
                NonEcoFriendlyProductCount = nonEcoFriendlyCount,

                ProductEcoBreakdown = new Dictionary<string, int>
                {
                    { "Eco-Friendly", ecoFriendlyCount },
                    { "Not Eco-Friendly", nonEcoFriendlyCount }
                },

                OrderTransportBreakdown = transportModeCounts,

                EmissionTrendOverTime = new Dictionary<string, float>(),

                // for product and item mockup purposes
                RandomProductList = _productControl.getAllProducts(),
                ItemId = "",
                RandomItemInstance = new Item()
            };

            // Populate comparison data
            viewModel.Products = products.Select(p => new ProductComparisonData
            {
                ProductId = p.retrieveProductId(),
                ProductName = p.retrieveProductName(),
                CarbonEmission = p.calculateSelfEmission()
            }).ToList();

            var shippingMethodEmissions = orders
                .GroupBy(o => o.retrieveTransportMode().ToUpper())
                .Select(g => new ShippingMethodComparisonData
                {
                    TransportMode = g.Key,
                    AverageCarbonEmission = g.Average(o => o.calculateSelfEmission())
                })
                .ToList();

            viewModel.ShippingMethods = shippingMethodEmissions;

            viewModel.EmissionTrendDaily = orders
                .GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .OrderBy(g => DateTime.Parse(g.Key))
                .ToDictionary(g => g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));

            viewModel.EmissionTrendWeekly = orders
                .GroupBy(o =>
                    CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                        o.retrieveDateCreated(), CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .OrderBy(g => g.Key)
                .ToDictionary(g => "Week " + g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));

            viewModel.EmissionTrendMonthly = orders
                .GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM"))
                .OrderBy(g => g.Key)
                .ToDictionary(g => g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));

            var topEcoEfficientProducts = products
            .OrderBy(p => p.calculateSelfEmission())
            .Take(5)
            .Select(p => new
            {
                Name = p.retrieveProductName(),
                Emission = p.calculateSelfEmission()
            })
            .ToList();

            viewModel.TopEcoProductLabels = topEcoEfficientProducts.Select(p => p.Name).ToList();
            viewModel.TopEcoProductValues = topEcoEfficientProducts.Select(p => p.Emission).ToList();

            var categoryEmission = products
            .GroupBy(p => p.retrieveProductCategory())
            .ToDictionary(
                g => g.Key,
                g => g.Sum(p => p.calculateSelfEmission())
            );

            viewModel.ProductCategoryLabels = categoryEmission.Keys.ToList();
            viewModel.ProductCategoryValues = categoryEmission.Values.ToList();

            return View(viewModel);
        }

        public IActionResult EcoFriendlyReport()
        {
            var allItemEmission = _itemCFControl.getTotalCarbonFootprint();
            var allItemCF = _itemCFControl.getAllItemCarbonFootprint();

            var ecoFriendlyItemEmission = allItemCF
                .Where(x => x.retrieveEcoStatus().Equals("Eco-Friendly"))
                .Sum(x => x.calculateSelfEmission());

            ecoFriendlyItemEmission = Math.Round(ecoFriendlyItemEmission, 2);

            var ecoFriendlyEmissionPercent = Math.Round((ecoFriendlyItemEmission / allItemEmission) * 100.0, 2);

            var products = _productCFControl.getAllProductCarbonFootprint();

            var viewModel = new EcoFriendlyReportViewModel
            {
                TotalItemCF = allItemEmission,
                TotalEcoFriendlyItemCF = (float)ecoFriendlyItemEmission,
                EcoFriendlyCFPercentage = (float)ecoFriendlyEmissionPercent
            };

            viewModel.ItemCarbonFootprints = new List<object>();

            foreach (ProductCarbonFootprintRDM prod in products)
            {
                var productItems = allItemCF.Where(x => x.retrieveProductId() == prod.retrieveProductId());
                int numOfProductItems = productItems.Count();
                viewModel.ItemCarbonFootprints.Add(new
                {
                    name = prod.retrieveProductName(),
                    baseEmission = prod.calculateSelfEmission(),
                    ecoStatus = prod.retrieveEcoStatus(),
                    numOfProductItems = numOfProductItems,
                    averagePerItemEmission = Math.Round(productItems.Average(x => x.calculateSelfEmission()), 2)
                });
            }
            return View("EcoFriendlyReport", viewModel);
        }
        public IActionResult ProductItemEmissionTrend(int productId)
        {
            if (_productCFControl.getProductCarbonFootprint(productId) == 0) return NotFound("The specified product does not exist");

            List<ItemCarbonFootprintRDM> productItems = _itemCFControl.getAllItemCarbonFootprint().Where(x => x.retrieveProductId() == productId).ToList();

            Dictionary<string, float> result = productItems.GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .OrderBy(g => DateTime.Parse(g.Key))
                .ToDictionary(g => g.Key, g => g.Sum(o => (float)o.calculateSelfEmission()));
            
            return Json(result);
        }

        [HttpPost]
        public IActionResult CreateProduct()
        {
            var random = new Random();

            // Generate a random product name (Product A-Z)
            char productLetter = (char)('A' + random.Next(0, 26));
            string productName = $"Product {productLetter}";

            // Random product category
            string[] categories = { "Detergent", "Kitchen", "Household", "Personal Care" };
            string productCategory = categories[random.Next(categories.Length)];

            // Randomized attributes within given ranges
            float productCost = (float)random.NextDouble() * (15 - 6) + 6;
            int manufacturerId = random.Next(1, 11);
            float productWeight = (float)(random.NextDouble() * (1 - 0.3) + 0.3);
            int quantity = random.Next(20, 101);
            float volume = (float)(random.NextDouble() * (100 - 1) + 1);
            float toxicityPercentage = (float)(random.NextDouble() * (0.4 - 0.05) + 0.05);

            // Create a new product instance
            var newProduct = new Product
            {
                ProductName = productName,
                ProductCategory = productCategory,
                ProductCost = (float)Math.Round(productCost, 2),
                ManufacturerId = manufacturerId,
                ProductWeight = productWeight,
                Quantity = quantity,
                Volume = (int)volume,
                ToxicityPercentage = toxicityPercentage,
                CarbonFootprint = 0,
                ProductState = "Ready"
            };

            // Add product to the database (or stub)
            _productControl.createProduct(
                productName, productCategory, (float)Math.Round(productCost, 2),
                manufacturerId, productWeight, quantity, (int)volume,
                toxicityPercentage, 0, "Ready");

            // Retrieve the last added product
            Product lastProduct = _productControl.getAllProducts().Last();
            int productId = lastProduct.ProductId;

            // Calculate Carbon Footprint
            newProduct.CarbonFootprint = (int)_carbonFootprintCalculatorControl.CalculateProductCF(volume, toxicityPercentage, productId);

            // ✅ Update ViewModel before returning JSON
            var mockupCreationVM = new CarbonDashboardViewModel
            {
                RandomProductList = _productControl.getAllProducts()
            };

            return Json(new { 
                success = true, 
                productId = newProduct.ProductId,
                productName = newProduct.ProductName, 
                productCategory = newProduct.ProductCategory,
                productCost = newProduct.ProductCost,
                manufacturerId = newProduct.ManufacturerId,
                productWeight = newProduct.ProductWeight,
                quantity = newProduct.Quantity,
                volume = newProduct.Volume,
                toxicityPercentage = newProduct.ToxicityPercentage
            });
        }
    }
}