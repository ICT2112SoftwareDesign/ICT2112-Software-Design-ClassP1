using CleanBrilliantCompany.Domain;
using CleanBrilliantCompany.DomainControl;
using CleanBrilliantCompany.Interfaces;
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

        // just for mocking purposes
        private readonly ProductControl _productControl; 
        private readonly ItemControl _itemControl;

        public DashboardPageController(ProductCarbonFootprintControl productCFControl, ItemCarbonFootprintControl itemCFControl, OrderCarbonFootprintControl orderCFControl, CarbonFootprintCalculatorControl carbonFootprintCalculatorControl, ProductControl productControl, ItemControl itemControl)
        {
            _productCFControl = productCFControl;
            _itemCFControl = itemCFControl;
            _orderCFControl = orderCFControl;
            
            // for product, item creation mockup
            _carbonFootprintCalculatorControl = carbonFootprintCalculatorControl;
            _productControl = productControl ?? throw new ArgumentNullException(nameof(productControl));
            _itemControl = itemControl ?? throw new ArgumentNullException(nameof(itemControl));
        }

        public IActionResult Dashboard()
        {
            var products = _productCFControl.getAllProductCarbonFootprint();
            var ecoFriendlyCount = products.Count(p => p.calculateSelfEmission() < 250);
            var nonEcoFriendlyCount = products.Count - ecoFriendlyCount;

            var orders = _orderCFControl.getAllOrderCarbonFootprint();
            var itemList = _itemCFControl.getAllItemCarbonFootprint();

            var transportModeCounts = orders
            .GroupBy(o => o.retrieveTransportMode().ToUpper())
            .ToDictionary(g => g.Key, g => g.Count());

            var oneWeekAgo = DateTime.Now.AddDays(-7);
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

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

                ProductPastWeekCF = products.Where(p => p.retrieveDateCreated() >= oneWeekAgo).Sum(p => (float)p.calculateSelfEmission()),
                ProductPastMonthCF = products.Where(p => p.retrieveDateCreated() >= oneMonthAgo).Sum(p => (float)p.calculateSelfEmission()),

                ItemPastWeekCF = itemList.Where(i => i.retrieveDateCreated() >= oneWeekAgo).Sum(i => (float)i.calculateSelfEmission()),
                ItemPastMonthCF = itemList.Where(i => i.retrieveDateCreated() >= oneMonthAgo).Sum(i => (float)i.calculateSelfEmission()),

                OrderPastWeekCF = orders.Where(o => o.retrieveDateCreated() >= oneWeekAgo).Sum(o => (float)o.calculateSelfEmission()),
                OrderPastMonthCF = orders.Where(o => o.retrieveDateCreated() >= oneMonthAgo).Sum(o => (float)o.calculateSelfEmission()),

                // for product and item mockup purposes
                ProductList = _productControl.getAllProducts(),
                ItemList = _itemControl.getAllItems().Result
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

            var dailyProductEmissions = products
                .GroupBy(p => p.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .Select(g => new { Date = g.Key, Emission = g.Sum(p => (float)p.calculateSelfEmission()) });

            var dailyItemEmissions = itemList
                .GroupBy(i => i.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .Select(g => new { Date = g.Key, Emission = g.Sum(i => (float)i.calculateSelfEmission()) });

            var dailyOrderEmissions = orders
                .GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .Select(g => new { Date = g.Key, Emission = g.Sum(o => (float)o.calculateSelfEmission()) });

            // Combine all into one
            var dailyEmissions = dailyProductEmissions
                .Concat(dailyItemEmissions)
                .Concat(dailyOrderEmissions)
                .GroupBy(x => x.Date)
                .OrderBy(g => DateTime.Parse(g.Key))
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Emission)
                );

            viewModel.EmissionTrendDaily = dailyEmissions;

            var weeklyProductEmissions = products
                .GroupBy(p => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                    p.retrieveDateCreated(), CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new { Week = g.Key, Emission = g.Sum(p => (float)p.calculateSelfEmission()) });

            var weeklyItemEmissions = itemList
                .GroupBy(i => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                    i.retrieveDateCreated(), CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new { Week = g.Key, Emission = g.Sum(i => (float)i.calculateSelfEmission()) });

            var weeklyOrderEmissions = orders
                .GroupBy(o => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                    o.retrieveDateCreated(), CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new { Week = g.Key, Emission = g.Sum(o => (float)o.calculateSelfEmission()) });

            var weeklyEmissions = weeklyProductEmissions
                .Concat(weeklyItemEmissions)
                .Concat(weeklyOrderEmissions)
                .GroupBy(x => x.Week)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => "Week " + g.Key,
                    g => g.Sum(x => x.Emission)
                );

            viewModel.EmissionTrendWeekly = weeklyEmissions;

            var monthlyProductEmissions = products
                .GroupBy(p => p.retrieveDateCreated().ToString("yyyy-MM"))
                .Select(g => new { Month = g.Key, Emission = g.Sum(p => (float)p.calculateSelfEmission()) });

            var monthlyItemEmissions = itemList
                .GroupBy(i => i.retrieveDateCreated().ToString("yyyy-MM"))
                .Select(g => new { Month = g.Key, Emission = g.Sum(i => (float)i.calculateSelfEmission()) });

            var monthlyOrderEmissions = orders
                .GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM"))
                .Select(g => new { Month = g.Key, Emission = g.Sum(o => (float)o.calculateSelfEmission()) });

            var monthlyEmissions = monthlyProductEmissions
                .Concat(monthlyItemEmissions)
                .Concat(monthlyOrderEmissions)
                .GroupBy(x => x.Month)
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(x => x.Emission)
                );

            viewModel.EmissionTrendMonthly = monthlyEmissions;

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

            var ecoProductTrend = products
            .Where(p => p.retrieveEcoStatus() == "Eco-Friendly")
            .GroupBy(p => p.retrieveDateCreated().ToString("yyyy-MM"))
            .OrderBy(g => g.Key)
            .ToDictionary(
                g => g.Key,
                g => g.Count()
            );

            viewModel.EcoProductTrendLabels = ecoProductTrend.Keys.ToList();
            viewModel.EcoProductTrendCounts = ecoProductTrend.Values.ToList();

            var topEmittingProducts = products
            .OrderByDescending(p => p.calculateSelfEmission())
            .Take(5)
            .Select(p => new
            {
                Name = p.retrieveProductName(),
                Emission = p.calculateSelfEmission()
            })
            .ToList();

            viewModel.TopEmitProductLabels = topEmittingProducts.Select(p => p.Name).ToList();
            viewModel.TopEmitProductValues = topEmittingProducts.Select(p => p.Emission).ToList();

            viewModel.ItemEmissionBreakdown = products
            .Select(p =>
            {
                var relatedItems = itemList.Where(i => i.retrieveProductId() == p.retrieveProductId());
                float totalItemEmission = relatedItems.Sum(i => (float)i.calculateSelfEmission());
                return new
                {
                    ProductName = p.retrieveProductName(),
                    TotalItemEmission = totalItemEmission
                };
            })
            .OrderByDescending(x => x.TotalItemEmission)
            .Take(5)
            .ToDictionary(x => x.ProductName, x => x.TotalItemEmission);

            var ecoItemCountOverTime = itemList
            .GroupBy(i => i.retrieveDateCreated().ToString("yyyy-MM"))
            .ToDictionary(
                g => g.Key,
                g => new Dictionary<string, int>
                {
                    { "Eco", g.Count(i => i.retrieveEcoStatus() == "Eco-Friendly") },
                    { "NonEco", g.Count(i => i.retrieveEcoStatus() == "Not Eco-Friendly") }
                }
            );

            viewModel.EcoVsNonEcoItemTimeline = ecoItemCountOverTime.ToDictionary(kvp => kvp.Key, kvp => (object)kvp.Value);
            viewModel.EcoVsNonEcoMonths = ecoItemCountOverTime.Keys.OrderBy(k => k).ToList();

            return View(viewModel);
        }

        public IActionResult EcoFriendlyReport()
        {
            var allItemEmission = _itemCFControl.getTotalCarbonFootprint();

            var ecoFriendlyItemEmission = _itemCFControl.getTotalEcoFriendlyCarbonFootprint();

            ecoFriendlyItemEmission = (float)Math.Round(ecoFriendlyItemEmission, 2);

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
                var productItems = _itemCFControl.getItemCarbonFootprintByProductId(prod.retrieveProductId());
                int numOfProductItems = productItems.Count();
                if(numOfProductItems > 0)
                {
                    viewModel.ItemCarbonFootprints.Add(new
                    {
                        productId = prod.retrieveProductId(),
                        name = prod.retrieveProductName(),
                        dateCreated = prod.retrieveDateCreated().ToString("dd/MM/yyyy"),
                        baseEmission = prod.calculateSelfEmission(),
                        ecoStatus = prod.retrieveEcoStatus().Equals("Eco-Friendly") ? "Yes" : "No",
                        numOfProductItems = numOfProductItems,
                        averagePerItemEmission = Math.Round(productItems.Average(x => x.calculateSelfEmission()), 2)
                    });
                }
                
            }
            return View("EcoFriendlyReport", viewModel);
        }

        [HttpGet]
        public IActionResult ProductItemEmissionTrend(int productId)
        {
            if (_productCFControl.getProductCarbonFootprint(productId) == 0) return NotFound("The specified product does not exist");

            List<ItemCarbonFootprintRDM> productItems = _itemCFControl.getItemCarbonFootprintByProductId(productId);

            Dictionary<string, float> result = productItems.GroupBy(o => o.retrieveDateCreated().ToString("yyyy-MM-dd"))
                .OrderBy(g => DateTime.Parse(g.Key))
                .ToDictionary(g => g.Key, g => g.Average(o => (float)o.calculateSelfEmission()));
            
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
                ProductList = _productControl.getAllProducts()
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
                toxicityPercentage = newProduct.ToxicityPercentage,
                carbonEmission = newProduct.CarbonFootprint
            });
        }

        [HttpPost]
        public IActionResult CalculateItem(int itemId)
        {
            Console.WriteLine("Item ID caught: " + itemId);

            // Use await for async operation rather than blocking with .Result
            Item i = _itemControl.getItemById(itemId)?.Result;
            
            if (i == null)
            {
                return Json(new { success = false, message = "Item not found." });
            }

            int productId = i.getProductId();

            // Assuming the calculation is async, ensure you await it properly
            bool success = _carbonFootprintCalculatorControl.CalculateItemCF(itemId, productId);

            return Json(new { success });
        }

        [HttpPost]
        public IActionResult UpdateItemsCFDaily()
        {
            bool success = _itemCFControl.updateAllItemCF();

            // Create the updated ViewModel
            var itemList = _itemCFControl.getAllItemCarbonFootprint();
            var carbonDashboardViewModel = new CarbonDashboardViewModel
            {
                TotalProductCF = _productCFControl.getTotalCarbonFootprint(),
                TotalItemCF = _itemCFControl.getTotalCarbonFootprint(),
                TotalOrderCF = _orderCFControl.getTotalCarbonFootprint(),

                ItemPastWeekCF = itemList.Where(i => i.retrieveDateCreated() >= DateTime.Now.AddDays(-7))
                                        .Sum(i => (float)i.calculateSelfEmission()),
                ItemPastMonthCF = itemList.Where(i => i.retrieveDateCreated() >= DateTime.Now.AddMonths(-1))
                                        .Sum(i => (float)i.calculateSelfEmission()),
            };

            return Json(new { success, updatedViewModel = carbonDashboardViewModel });
        }
                [HttpPost]
        public IActionResult GetFilteredProductData([FromBody] FilterRequest request)
        {
            try
            {
                var startDate = DateTime.Parse(request.StartDate);
                var endDate = DateTime.Parse(request.EndDate);
                var productIds = request.ProductIds;

                var products = _productCFControl.getAllProductCarbonFootprint()
                    .Where(p => productIds.Contains(p.retrieveProductId().ToString()) &&
                               p.retrieveDateCreated() >= startDate &&
                               p.retrieveDateCreated() <= endDate)
                    .ToList();

                var result = new
                {
                    labels = products.Select(p => p.retrieveProductName()).ToList(),
                    values = products.Select(p => p.calculateSelfEmission()).ToList()
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult GetFilteredShippingData([FromBody] FilterRequest request)
        {
            try
            {
                var startDate = DateTime.Parse(request.StartDate);
                var endDate = DateTime.Parse(request.EndDate);
                var shippingMethods = request.ShippingMethods;

                var orders = _orderCFControl.getAllOrderCarbonFootprint()
                    .Where(o => shippingMethods.Contains(o.retrieveTransportMode().ToUpper()) &&
                               o.retrieveDateCreated() >= startDate &&
                               o.retrieveDateCreated() <= endDate)
                    .ToList();

                var result = new
                {
                    labels = shippingMethods,
                    values = shippingMethods.Select(method =>
                        orders.Where(o => o.retrieveTransportMode().ToUpper() == method)
                              .Average(o => o.calculateSelfEmission())
                    ).ToList()
                };

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult GetFilteredProducts([FromBody] DateRangeRequest request)
        {
            try
            {
                var startDate = DateTime.Parse(request.StartDate);
                var endDate = DateTime.Parse(request.EndDate);

                var products = _productCFControl.getAllProductCarbonFootprint()
                    .Where(p => p.retrieveDateCreated() >= startDate && 
                               p.retrieveDateCreated() <= endDate)
                    .Select(p => new
                    {
                        ProductId = p.retrieveProductId(),
                        ProductName = p.retrieveProductName(),
                        CarbonEmission = p.calculateSelfEmission()
                    })
                    .ToList();

                return Json(new { success = true, products = products });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult GetFilteredShippingMethods([FromBody] DateRangeRequest request)
        {
            try
            {
                var startDate = DateTime.Parse(request.StartDate);
                var endDate = DateTime.Parse(request.EndDate);

                var orders = _orderCFControl.getAllOrderCarbonFootprint()
                    .Where(o => o.retrieveDateCreated() >= startDate && 
                               o.retrieveDateCreated() <= endDate)
                    .ToList();

                var shippingMethods = orders
                    .GroupBy(o => o.retrieveTransportMode().ToUpper())
                    .Select(g => new
                    {
                        TransportMode = g.Key,
                        AverageCarbonEmission = g.Average(o => o.calculateSelfEmission())
                    })
                    .ToList();

                return Json(new { success = true, shippingMethods = shippingMethods });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    public class FilterRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public List<string> ProductIds { get; set; }
        public List<string> ShippingMethods { get; set; }
    }

    public class DateRangeRequest
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}