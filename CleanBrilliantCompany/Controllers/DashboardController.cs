using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CleanBrilliantCompany.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundDatabase _refundDatabase;

        public DashboardController(IOrderDatabase orderDatabase, IRefundDatabase refundDatabase)
        {
            _orderDatabase = orderDatabase ?? throw new ArgumentNullException(nameof(orderDatabase));
            _refundDatabase = refundDatabase ?? throw new ArgumentNullException(nameof(refundDatabase));
        }

        public IActionResult Index()
        {
            try
            {
                var summary = new DashboardSummary
                {
                    TotalOrders = _orderDatabase.GetTotalOrderCount(),
                    TotalRefunds = _refundDatabase.GetTotalRefundCount(),
                    NetRevenue = _orderDatabase.GetTotalOrderValue() - _refundDatabase.GetTotalRefundAmount(),
                    PendingRefunds = _refundDatabase.GetPendingRefundCount()
                };

                return View(summary);
            }
            catch (Exception ex)
            {
                // Log the error (add your logging here)
                Console.WriteLine($"Dashboard error: {ex}");

                // Return view with default values
                return View(new DashboardSummary());
            }
        }
    }
}