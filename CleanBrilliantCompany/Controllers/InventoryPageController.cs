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
                var chartData = _inventoryControl.GenerateStockLevelChartData();
                ViewBag.ChartData = chartData;
                return View("ViewInventoryDashboard", inventoryDashboard);
            }
            catch (InvalidOperationException ex)
            {
                //return RedirectToAction("CreateDashboard");
                ViewData["ErrorMessage"] = ex.Message;
                return View("ViewInventoryDashboard");
            }
        }

        //[HttpGet]
        //public IActionResult CreateDashboard()
        //{
        //    //ViewData["StartDate"] = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
        //    //ViewData["EndDate"] = DateTime.Now.ToString("yyyy-MM-dd");
        //    //ViewData["LevelOfDetail"] = 1;
        //    _inventoryControl.CreateDashboard("Inventory Dashboard", 30);
        //    Console.WriteLine("Inventory Dashboard created.");
        //    return View("ViewDashboard");
        //}

        [HttpPost]
        public IActionResult CreateDashboard()
        {
            _inventoryControl.CreateDashboard("Inventory Dashboard", 30);
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