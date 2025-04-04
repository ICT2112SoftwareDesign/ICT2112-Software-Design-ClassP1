using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.ForecastManagement.Models;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CleanBrilliantCompany.Controllers
{
    [Route("[controller]")]
    public class ForecastController : Controller
    {
        private readonly ForecastFacade _forecastFacade;
        //private readonly IMemoryCache _cache;
        //private const string DashboardCacheKey = "Dashboard_User123"; // Adjust key for user-specific caching

        // Inject ForecastControl and IMemoryCache via DI
        public ForecastController(ForecastFacade forecastControl)
        {
            _forecastFacade = forecastControl;
            //_cache = cache;
        }

        // Handles user input actions, typically triggered from UI
        [HttpPost("handleInput")]
        public IActionResult HandleInput(int productId, int batchId)
        {
            // Implement logic to handle input if needed
            return Ok();
        }

        // Fetches data needed for dashboard view
        [HttpGet("fetchDashboardData")]
        public IActionResult FetchDashboardData()
        {
            // 1) Possibly retrieve from DB or from your facade
            // Calculate the first day of next month
            DateTime forecastMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            var dashboard = _forecastFacade.generateDashboard(forecastMonth, 0);

            // 2) Serialize
            string serialized = JsonSerializer.Serialize(dashboard);
            // Generate the trend data map: "yyyy-MM" -> (productId -> forecast value)
            // Calculate the first day of next month
            DateTime forecastTrendMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            var trendData=_forecastFacade.GenerateForecastTrendData(forecastTrendMonth, 12);



            // Pass the data via ViewBag
            ViewBag.TrendData = trendData;


            // 3) Store in Session
            HttpContext.Session.SetString("ForecastDashboard", serialized);

            // 4) Return view
            return View("FetchDashboardData", dashboard);
        }

        [HttpPost("generateForecast")]
        public IActionResult GenerateForecast(string type, string month, int priceAdjustment=0)
        {
            // Append "-01" to convert the month string into a full date (e.g., "2025-03-01")
            if (!DateTime.TryParse(month + "-01", out DateTime forecastMonth))
            {
                return BadRequest("Invalid month format.");
            }
            
            
            // Generate the forecast dashboard using ForecastControl
            ForecastDashboard dashboard= _forecastFacade.generateDashboard(
                 forecastMonth, priceAdjustment
            );
            // Generate the trend data map: "yyyy-MM" -> (productId -> forecast value)
            DateTime forecastTrendMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1);
            var trendData = _forecastFacade.GenerateForecastTrendData(forecastTrendMonth, 12);
            // Pass the data via ViewBag
            ViewBag.TrendData = trendData;

            // 2) Serialize
            string serialized = JsonSerializer.Serialize(dashboard);

            // 3) Store in Session
            HttpContext.Session.SetString("ForecastDashboard", serialized);

            return View("FetchDashboardData", dashboard);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        [HttpPost("UpdatePriceAdjustment")]
        public IActionResult UpdatePriceAdjustment(int productId, string productName, int priceAdjustment,string sortType, string sortOrder)
        {
            if (productId <= 0 || string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Invalid product ID or name.");
            }

            // 1) Pull from session
            string serialized = HttpContext.Session.GetString("ForecastDashboard");
            if (string.IsNullOrEmpty(serialized))
            {
                return BadRequest("No dashboard found in session.");
            }

            // 2) Deserialize
            ForecastDashboard dashboard = JsonSerializer.Deserialize<ForecastDashboard>(serialized);

            if (sortType== "value")
            {
                ViewBag.CurrentSortType = "";
                ViewBag.CurrentSortOrder = "ascending";
            }
            else
            {
                ViewBag.CurrentSortType = sortType;
                ViewBag.CurrentSortOrder = sortOrder;
            }
            // Update the dashboard using ForecastControl logic
            ForecastDashboard updatedDashboard = _forecastFacade.updateMetric(productId, dashboard, priceAdjustment);

            // 4) Re-serialize & store updated version
            string updatedSerialized = JsonSerializer.Serialize(updatedDashboard);
            HttpContext.Session.SetString("ForecastDashboard", updatedSerialized);

            return PartialView("_MetricsPartial", updatedDashboard);
        }

        [HttpPost("sortMetrics")]
        public IActionResult SortMetrics(string sortType, string sortOrder)
        {
            string serialized = HttpContext.Session.GetString("ForecastDashboard");
            if (string.IsNullOrEmpty(serialized))
            {
                return BadRequest("No dashboard found in session.");
            }
            ForecastDashboard dashboard = JsonSerializer.Deserialize<ForecastDashboard>(serialized);

            // Sort the metrics based on the user's requested sortType and sortOrder
            var sortedMetrics = ForecastMetricSorter.Sort(dashboard.GetMetrics(), sortType, sortOrder);
            dashboard.SetMetrics(sortedMetrics);

            string updatedSerialized = JsonSerializer.Serialize(dashboard);
            HttpContext.Session.SetString("ForecastDashboard", updatedSerialized);

            // IMPORTANT: Pass the ACTUAL current state to the partial via ViewBag
            ViewBag.CurrentSortType = sortType;
            ViewBag.CurrentSortOrder = sortOrder;

            return PartialView("_MetricsPartial", dashboard);
        }
    }
}
