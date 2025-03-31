using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.DatabaseEntities;
using CleanBrilliantCompany.DTO;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class InventoryPageController : Controller
    {
        private InventoryControl _inventoryControl;

        public InventoryPageController(InventoryControl inventoryControl)
        {
            _inventoryControl = inventoryControl;
        }

        [Route("InventoryPage/ViewInventoryDashboard")]
        public IActionResult ViewDashboard(string category)
        {
            try
            {
                var inventoryDashboard = _inventoryControl.FetchDashboard();
                var chartData = _inventoryControl.GenerateStockLevelChartData(category);
                var lowStock = _inventoryControl.CheckLowStock(category);
                var overStock = _inventoryControl.CheckOverStock(category);
                var toReplenish = _inventoryControl.GenerateReplenishmentActions(category);
                var alertStreaks = _inventoryControl.GetWeeklyConsecutiveAlertCounts();

                // Send categories to dropdown
                var categories = _inventoryControl.GetAllProducts()
                                    .Select(p => p.productCategory)
                                    .Distinct()
                                    .ToList();

                var productLookup = _inventoryControl.GetProductLookup();

                ViewBag.Categories = categories;
                ViewBag.SelectedCategory = category;
                ViewBag.ChartData = chartData;
                ViewBag.AlertStreaks = alertStreaks;
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
            try
            {
                // Initialize thresholds first to ensure all products have them
                _inventoryControl.InitializeMissingThresholds();

                // Create new dashboard with current data
                _inventoryControl.CreateDashboard("Inventory Dashboard", 7);

                // Redirect to view the newly created dashboard
                return RedirectToAction("ViewInventoryDashboard");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Failed to create dashboard: {ex.Message}";
                return View("ViewInventoryDashboard");
            }
        }

        [HttpPost]
        public IActionResult GenerateReport()
        {
            var report = _inventoryControl.GenerateReport();
            return Content(report, "text/html");
        }

        [HttpGet]
        public IActionResult ViewThresholds()
        {
            _inventoryControl.InitializeMissingThresholds();
            var model = _inventoryControl.GetProductThresholdsWithInfo();
            return View("ViewThresholds", model);
        }

        [HttpGet]
        public IActionResult EditThreshold(int id)
        {
            var product = _inventoryControl.GetProductLookup()[id];
            var threshold = _inventoryControl.GetThresholdById(id);

            var model = new InventoryDTO
            {
                ProductId = id,
                ProductName = product.ProductName,
                ProductCategory = product.ProductCategory,
                Threshold = threshold?.Threshold ?? 100,
                LastUpdated = threshold?.LastUpdated
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditThreshold(InventoryDTO model)
        {
            var thresholdModel = new ProductThresholdTable
            {
                ProductId = model.ProductId,
                Threshold = model.Threshold,
                LastUpdated = DateTime.Now
            };

            _inventoryControl.UpdateThreshold(thresholdModel);
            return RedirectToAction("ViewThresholds");
        }

    }
}