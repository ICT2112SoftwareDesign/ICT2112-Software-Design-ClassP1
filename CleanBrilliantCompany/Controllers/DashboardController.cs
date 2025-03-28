using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CleanBrilliantCompany.Controllers 
{
    public class DashboardController : Controller 
    {
        private readonly DashboardManagement _dashboardManagement;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            DashboardManagement dashboardManagement, 
            ILogger<DashboardController> logger)
        {
            _dashboardManagement = dashboardManagement ?? 
                throw new ArgumentNullException(nameof(dashboardManagement));
            _logger = logger ?? 
                throw new ArgumentNullException(nameof(logger));
        }

        // GET: /Dashboard/Index
        public IActionResult Index()
        {
            try 
            {
                var summary = _dashboardManagement.GetDashboardSummary();

                // Log dashboard summary for monitoring
                _logger.LogInformation(
                    "Dashboard Summary - " +
                    "Total Orders: {TotalOrders}, " +
                    "Total Refunds: {TotalRefunds}, " +
                    "Pending Refunds: {PendingRefunds}, " +
                    "Net Revenue: {NetRevenue}",
                    summary.TotalOrders,
                    summary.TotalRefunds,
                    summary.PendingRefunds,
                    summary.NetRevenue
                );

                // Return view with summary
                return View(summary);
            }
            catch (Exception ex)
            {
                // Log the full error
                _logger.LogError(ex, "Error generating dashboard summary");

                // Return view with default values and an error flag
                ModelState.AddModelError("", "Unable to generate dashboard summary");
                return View(new DashboardSummary 
                { 
                    TotalOrders = -1,
                    TotalRefunds = -1,
                    PendingRefunds = -1,
                    NetRevenue = -1
                });
            }
        }

        // Optional: Standard MVC action method for dashboard summary
        public IActionResult Summary()
        {
            try 
            {
                var summary = _dashboardManagement.GetDashboardSummary();
                return Json(summary);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving dashboard summary");
                return Json(new { 
                    success = false,
                    message = "Unable to retrieve dashboard summary", 
                    error = ex.Message 
                });
            }
        }
    }
}