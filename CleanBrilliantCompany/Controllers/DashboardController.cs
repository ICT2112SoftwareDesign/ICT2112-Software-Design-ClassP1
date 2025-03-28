using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace CleanBrilliantCompany.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardManagement _dashboard;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(
            IOrderDatabase orderDatabase,
            IRefundDatabase refundDatabase,
            ILogger<DashboardController> logger)
        {
            _dashboard = new DashboardManagement(orderDatabase, refundDatabase);
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                // Get all metrics at once
                var (totalOrders, totalRefunds, pendingRefunds, netRevenue) = _dashboard.GetDashboardMetrics();
                
                // Get the latest order ID
                var latestOrder = _dashboard.getAllOrders()?.OrderByDescending(o => o.GetOrderDate()).FirstOrDefault();
                
                // Prepare ViewData/ViewBag
                ViewBag.Metrics = new {
                    TotalOrders = totalOrders,
                    TotalRefunds = totalRefunds,
                    PendingRefunds = pendingRefunds,
                    NetRevenue = netRevenue.ToString("C"),
                    LatestOrderId = latestOrder?.GetOrderID(),
                    LatestOrderDate = latestOrder?.GetOrderDate().ToShortDateString()
                };

                _logger.LogInformation("Dashboard loaded successfully with {OrderCount} orders", totalOrders);
                
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load dashboard");
                
                // Fallback values
                ViewBag.Metrics = new {
                    TotalOrders = -1,
                    TotalRefunds = -1,
                    PendingRefunds = -1,
                    NetRevenue = "$0.00",
                    LatestOrderId = (int?)null,
                    LatestOrderDate = "N/A"
                };
                
                ModelState.AddModelError("", "Unable to load dashboard data");
                return View();
            }
        }

        public IActionResult GetOrderDetails(int id)
        {
            try
            {
                var order = _dashboard.getOrderDetails(id);
                if (order == null)
                {
                    return NotFound();
                }
                return Json(new {
                    success = true,
                    orderId = order.GetOrderID(),
                    customerId = order.GetCustomerID(),
                    status = order.GetStatus()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get order {id}");
                return Json(new {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult UpdateOrderStatus(int orderId, string status)
        {
            try
            {
                var success = _dashboard.updateOrderStatus(orderId, status);
                return Json(new { success });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update order {orderId}");
                return Json(new {
                    success = false,
                    error = ex.Message
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { 
                RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}