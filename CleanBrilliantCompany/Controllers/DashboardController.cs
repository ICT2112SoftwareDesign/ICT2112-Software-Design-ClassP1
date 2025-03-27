using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Management;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace CleanBrilliantCompany.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardManagement _dashboardManagement;

        public DashboardController(DashboardManagement dashboardManagement)
        {
            _dashboardManagement = dashboardManagement;
        }

        public IActionResult Index()
        {
            var metrics = _dashboardManagement.GetDashboardMetrics();
            return View(metrics); // Pass the tuple directly to the view
        }
    }

    // [HttpGet]
    // public IActionResult GetDashboardData()
    // {
    //     try
    //     {
    //         // Get all orders and refunds
    //         var allOrders = _orderDatabase.getAllOrders();
    //         var allRefunds = _refundDatabase.GetAllRefunds();

    //         // Calculate metrics
    //         var totalOrders = allOrders.Count;
    //         var totalOrderValue = allOrders.Sum(o => o.GetOrderTotal());
    //         var pendingOrders = allOrders.Count(o => o.GetStatus() == "Pending");
    //         var completedOrders = allOrders.Count(o => o.GetStatus() == "Completed");

    //         var totalRefunds = allRefunds.Count;
    //         var totalRefundValue = allRefunds.Sum(r => (decimal)r.RefundAmount);
    //         var pendingRefunds = allRefunds.Count(r => r.Status == "Pending");
    //         var approvedRefunds = allRefunds.Count(r => r.Status == "Approved");

    //         // Group orders by month for the chart
    //         var monthlyOrders = allOrders
    //             .GroupBy(o => new { o.GetOrderDate().Year, o.GetOrderDate().Month })
    //             .Select(g => new
    //             {
    //                 Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
    //                 Count = g.Count(),
    //                 Value = g.Sum(o => o.GetOrderTotal())
    //             })
    //             .OrderBy(x => x.Month)
    //             .ToList();

    //         return Json(new
    //         {
    //             success = true,
    //             summary = new
    //             {
    //                 totalOrders,
    //                 totalOrderValue,
    //                 pendingOrders,
    //                 completedOrders,
    //                 totalRefunds,
    //                 totalRefundValue,
    //                 pendingRefunds,
    //                 approvedRefunds
    //             },
    //             monthlyOrders,
    //             recentOrders = allOrders.OrderByDescending(o => o.GetOrderDate()).Take(5),
    //             recentRefunds = allRefunds.OrderByDescending(r => r.RefundRequestDate).Take(5)
    //         });
    //     }
    //     catch (Exception ex)
    //     {
    //         return Json(new
    //         {
    //             success = false,
    //             error = ex.Message
    //         });
    //     }
    // }
}
