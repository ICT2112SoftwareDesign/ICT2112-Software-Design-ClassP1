using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Control;

namespace CleanBrilliantCompany.Controllers
{
    public class InventoryPageController : Controller
    {
        private InventoryControl _inventoryControl;

        public InventoryPageController(InventoryControl inventoryControl)
        {
            _inventoryControl = inventoryControl;
        }

        // Show the latest dashboard generated
        public IActionResult ViewDashboard()
        {
            try
            {
                var inventoryDashboard = _inventoryControl.FetchDashboard();
                ViewData["Dashboard"] = inventoryDashboard; // Pass the InventoryDashboardRDM directly
                return View("ViewInventoryDashboard");
            }
            catch (InvalidOperationException)
            {
                return RedirectToAction("CreateDashboard");
            }
        }

        [HttpGet]
        public IActionResult CreateDashboard()
        {
            return View("CreateInventoryDashboard");
        }

        [HttpPost]
        public IActionResult CreateDashboard(DateTime startDate, DateTime endDate, int levelOfDetail)
        {
            if (endDate < startDate)
            {
                ModelState.AddModelError("", "End date must be after start date.");
                return View("CreateInventoryDashboard");
            }
            _inventoryControl.CreateDashboard("Inventory Dashboard", startDate, endDate, 30);
            Console.WriteLine("Inventory Dashboard created.");
            return RedirectToAction("ViewDashboard");
        }

        [HttpPost]
        public IActionResult GenerateReport()
        {
            var report = _inventoryControl.GenerateReport();
            return Content(report, "text/html");
        }
    }
}