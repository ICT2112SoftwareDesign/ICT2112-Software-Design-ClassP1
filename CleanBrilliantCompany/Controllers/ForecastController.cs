using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Forecast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CleanBrilliantCompany.Controllers
{
    [Route("[controller]")]
    public class ForecastController : Controller
    {
        private readonly ForecastFacade _forecastFacade;
        private readonly IMemoryCache _cache;
        private const string DashboardCacheKey = "Dashboard_User123"; // Adjust key for user-specific caching

        // Inject ForecastControl and IMemoryCache via DI
        public ForecastController(ForecastFacade forecastControl, IMemoryCache cache)
        {
            _forecastFacade = forecastControl;
            _cache = cache;
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
            // Retrieve the dashboard from cache; if not present, get it from ForecastControl
            if (!_cache.TryGetValue(DashboardCacheKey, out ForecastDashboard dashboard))
            {
                dashboard = _forecastFacade.getLatestDashboard();
                _cache.Set(DashboardCacheKey, dashboard, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }
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
            ForecastDashboard dashboard = _forecastFacade.generateDashboard(
                 forecastMonth, priceAdjustment
            );

            // Store the generated dashboard in the memory cache
            _cache.Set(DashboardCacheKey, dashboard, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            return View("FetchDashboardData", dashboard);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        [HttpPost("UpdatePriceAdjustment")]
        public IActionResult UpdatePriceAdjustment(int productId, string productName, int priceAdjustment)
        {
            if (productId <= 0 || string.IsNullOrWhiteSpace(productName))
            {
                return BadRequest("Invalid product ID or name.");
            }

            // Retrieve the dashboard from the cache
            if (!_cache.TryGetValue(DashboardCacheKey, out ForecastDashboard dashboard))
            {
                return BadRequest("Dashboard not found in cache.");
            }

            // Update the dashboard using ForecastControl logic
            ForecastDashboard updatedDashboard = _forecastFacade.updateMetric(productId, dashboard, priceAdjustment);

            // Save the updated dashboard back into the cache
            _cache.Set(DashboardCacheKey, updatedDashboard, new MemoryCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromMinutes(30)
            });

            return PartialView("_MetricsPartial", updatedDashboard);
        }
    }
}
