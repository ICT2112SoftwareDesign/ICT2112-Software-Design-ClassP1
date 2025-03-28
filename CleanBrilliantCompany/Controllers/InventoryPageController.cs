using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Interface;

namespace CleanBrilliantCompany.Controllers
{
    public class InventoryPageController : Controller
    {
        private InventoryControl _inventoryControl;

        public InventoryPageController(InventoryControl inventoryControl)
        {
            _inventoryControl = inventoryControl;
        }

        public IActionResult ViewDashboard(string category)
        {
            try
            {
                var inventoryDashboard = _inventoryControl.FetchDashboard();
                var chartData = _inventoryControl.GenerateStockLevelChartData(category); // Pass filter
                var lowStock = _inventoryControl.CheckLowStock(category);
                var overStock = _inventoryControl.CheckOverStock(category);
                var toReplenish = _inventoryControl.GenerateReplenishmentActions(category);

                var alertStreaks = _inventoryControl.GetWeeklyConsecutiveAlertCounts();

                // Send categories to dropdown
                var categories = _inventoryControl.GetAllProducts()
                                    .Select(p => p.productCategory)
                                    .Distinct()
                                    .ToList();
                //var products = _inventoryControl.GetAllProducts();

                var productLookup = _inventoryControl.GetProductLookup();

                ViewBag.Categories = categories;
                ViewBag.SelectedCategory = category;
                ViewBag.ChartData = chartData;
                ViewBag.AlertStreaks = alertStreaks;
                //ViewBag.Products = products;
                ViewBag.ProductLookup = productLookup;
                ViewBag.LowStock = lowStock;
                ViewBag.OverStock = overStock;
                ViewBag.Replenish = toReplenish;

                return View("ViewInventoryDashboard", inventoryDashboard);
            }
            catch (InvalidOperationException ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View("ViewInventoryDashboard");
            }
        }

        [HttpPost]
        public IActionResult CreateDashboard()
        {
            _inventoryControl.CreateDashboard("Inventory Dashboard", 7);
            Console.WriteLine("Inventory Dashboard created.");
            return RedirectToAction("ViewDashboard");
        }

        [HttpPost]
        public IActionResult GenerateReport()
        {
            var report = _inventoryControl.GenerateReport();
            return Content(report, "text/html");
        }

        // TEST: View all products
        [HttpGet]
        public IActionResult ViewAllProducts()
        {
            var products = _inventoryControl.GetAllProducts();
            return View("ViewAllProducts", products);

        }

    }
}