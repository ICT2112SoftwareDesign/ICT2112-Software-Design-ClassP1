using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text.Json;

namespace CleanBrilliantCompany.Controllers
{
    [Route("staff/dashboard")]
    public class DashboardController : Controller
    {
        private readonly DashboardManagement _dashboard;
        private readonly ILogger<DashboardController> _logger;
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundDatabase _refundDatabase;

        public DashboardController(
            IOrderDatabase orderDatabase,
            IRefundDatabase refundDatabase,
            ILogger<DashboardController> logger)
        {
            _orderDatabase = orderDatabase;
            _refundDatabase = refundDatabase;
            _dashboard = new DashboardManagement(orderDatabase, refundDatabase);
            _logger = logger;

            _logger.LogInformation("DashboardController initialized");
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            try
            {
                // Log database types to verify correct implementations
                _logger.LogInformation("Order Database Type: {Type}", _orderDatabase.GetType().FullName);
                _logger.LogInformation("Refund Database Type: {Type}", _refundDatabase.GetType().FullName);

                // Check if data is available first
                bool dataAvailable = _dashboard.IsDashboardDataAvailable();
                _logger.LogInformation("Dashboard data availability: {Available}", dataAvailable);

                // Get all metrics at once
                var (totalOrders, totalRefunds, pendingRefunds, netRevenue) = _dashboard.GetDashboardMetrics();
                _logger.LogInformation("Dashboard metrics: Orders={Orders}, Refunds={Refunds}, Pending={Pending}, Revenue=${Revenue}",
                    totalOrders, totalRefunds, pendingRefunds, netRevenue);

                // Get the latest order ID
                var allOrders = _dashboard.getAllOrders();
                _logger.LogInformation("Retrieved {OrderCount} orders for dashboard", allOrders?.Count ?? 0);

                var latestOrder = allOrders?.OrderByDescending(o => o.GetOrderDate()).FirstOrDefault();
                if (latestOrder != null)
                {
                    _logger.LogInformation("Latest order: ID={Id}, Date={Date}, Total={Total}",
                        latestOrder.GetOrderID(), latestOrder.GetOrderDate(), latestOrder.GetOrderTotal());
                }

                // Prepare ViewData/ViewBag
                ViewBag.DataAvailable = dataAvailable;
                ViewBag.Metrics = new
                {
                    TotalOrders = totalOrders,
                    TotalRefunds = totalRefunds,
                    PendingRefunds = pendingRefunds,
                    NetRevenue = netRevenue.ToString("C"),
                    LatestOrderId = latestOrder?.GetOrderID(),
                    LatestOrderDate = latestOrder?.GetOrderDate().ToShortDateString() ?? "No orders found"
                };

                // Add raw data for debugging
                ViewBag.Debug = new
                {
                    OrderDatabaseType = _orderDatabase.GetType().Name,
                    RefundDatabaseType = _refundDatabase.GetType().Name,
                    OrderCount = allOrders?.Count ?? 0,
                    RefundCount = _dashboard.GetAllRefunds()?.Count ?? 0
                };

                _logger.LogInformation("Dashboard loaded successfully with {OrderCount} orders, {RefundCount} refunds",
                    totalOrders, totalRefunds);

                // Explicitly specify view path to ensure correct view is loaded
                return View("~/Views/Staff/index.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load dashboard");

                // Fallback values
                ViewBag.DataAvailable = false;
                ViewBag.Metrics = new
                {
                    TotalOrders = 0,
                    TotalRefunds = 0,
                    PendingRefunds = 0,
                    NetRevenue = "$0.00",
                    LatestOrderId = (int?)null,
                    LatestOrderDate = "Error retrieving data"
                };

                ViewBag.ErrorMessage = $"Unable to load dashboard data: {ex.Message}";
                ViewBag.ErrorDetails = ex.StackTrace;
                ModelState.AddModelError("", "Unable to load dashboard data");
                return View("~/Views/Staff/index.cshtml");
            }
        }
    }
}