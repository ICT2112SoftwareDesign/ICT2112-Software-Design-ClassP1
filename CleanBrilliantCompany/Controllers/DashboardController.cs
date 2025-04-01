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
        private readonly IOrder _order;
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundQuery _refundQuery;

        public DashboardController(
            IOrder order,
            IRefundQuery refundQuery,
            IOrderDatabase orderDatabase, // remove later when getOrderById() is available in IOrder
            ILogger<DashboardController> logger)
        {
            _order = order;
            _orderDatabase = orderDatabase;
            _refundQuery = refundQuery;
            _dashboard = new DashboardManagement(order, refundQuery, orderDatabase);
            _logger = logger;

            _logger.LogInformation("DashboardController initialized");
        }

        [HttpGet("")]
        public IActionResult Index(int? year = null, int? month = null)
        {
            try
            {
                // Log database types to verify correct implementations
                _logger.LogInformation("Order Database Type: {Type}", _order.GetType().FullName);
                _logger.LogInformation("Refund Database Type: {Type}", _refundQuery.GetType().FullName);

                // Check if data is available first
                bool dataAvailable = _dashboard.IsDashboardDataAvailable();
                _logger.LogInformation("Dashboard data availability: {Available}", dataAvailable);

                // Get all metrics at once
                var (totalOrders, totalRefunds, pendingRefunds, netRevenue) = _dashboard.GetDashboardMetrics();
                _logger.LogInformation("Dashboard metrics: Orders={Orders}, Refunds={Refunds}, Pending={Pending}, Revenue=${Revenue}",
                    totalOrders, totalRefunds, pendingRefunds, netRevenue);

                // Get the latest order ID
                var allOrders = _order.getAllOrders();
                _logger.LogInformation("Retrieved {OrderCount} orders for dashboard", allOrders?.Count ?? 0);

                var latestOrder = allOrders?.OrderByDescending(o => o.RetrieveOrderDate()).FirstOrDefault();
                if (latestOrder != null)
                {
                    _logger.LogInformation("Latest order: ID={Id}, Date={Date}, Total={Total}",
                        latestOrder.RetrieveOrderID(), latestOrder.RetrieveOrderDate(), latestOrder.RetrieveOrderTotal());
                }
                // Determine selected month (default to current month if not specified)
                var currentDate = DateTime.Now;
                var selectedYear = year ?? currentDate.Year;
                var selectedMonth = month ?? currentDate.Month;
                var selectedDate = new DateTime(selectedYear, selectedMonth, 1);

                // Generate list of all months for dropdown (last 12 months)
                var availableMonths = Enumerable.Range(0, 12)
                    .Select(i => currentDate.AddMonths(-i))
                    .Select(d => new
                    {
                        Year = d.Year,
                        Month = d.Month,
                        DisplayName = d.ToString("MMMM yyyy")
                    })
                    .OrderByDescending(m => m.Year)
                    .ThenByDescending(m => m.Month)
                    .ToList();

                // Get chart data for selected month (will return empty if no data)
                var dailyOrderCounts = _dashboard.GetDailyOrderCountsForMonth(selectedDate);
                var dailyRefundCounts = _dashboard.GetDailyRefundCountsForMonth(selectedDate);
                var profitComponents = _dashboard.GetProfitComponentsForMonths(selectedDate);

                // Prepare ViewData/ViewBag
                ViewBag.DataAvailable = dataAvailable;
                ViewBag.Metrics = new
                {
                    TotalOrders = totalOrders,
                    TotalRefunds = totalRefunds,
                    PendingRefunds = pendingRefunds,
                    NetRevenue = netRevenue.ToString("C"),
                    LatestOrderId = latestOrder?.RetrieveOrderID(),
                    LatestOrderDate = latestOrder?.RetrieveOrderDate().ToShortDateString() ?? "No orders found"
                };
                ViewBag.ChartData = new
                {
                    DailyOrderCounts = dailyOrderCounts ?? new Dictionary<DateTime, int>(),
                    DailyRefundCounts = dailyRefundCounts ?? new Dictionary<DateTime, int>(),
                    CurrentMonth = selectedDate.ToString("MMMM yyyy"),
                    SelectedYear = selectedYear,
                    SelectedMonth = selectedMonth,
                    ProfitMetrics = new {
                    GrossProfit = profitComponents.GrossProfit,
                    RefundAmount = profitComponents.RefundAmount,
                    NetProfit = profitComponents.NetProfit
                    }
                };

                ViewBag.AvailableMonths = availableMonths;

                // Add raw data for debugging
                ViewBag.Debug = new
                {
                    OrderDatabaseType = _order.GetType().Name,
                    RefundDatabaseType = _refundQuery.GetType().Name,
                    OrderCount = allOrders?.Count ?? 0,
                    RefundCount = _refundQuery.GetAllRefunds()?.Count ?? 0
                };

                _logger.LogInformation("Dashboard loaded successfully with {OrderCount} orders, {RefundCount} refunds",
                    totalOrders, totalRefunds);

                return View("~/Views/StaffPage/Index.cshtml");
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
        private List<MonthOption> GetAvailableMonths()
        {
            try
            {
                var orders = _order.getAllOrders();
                if (orders == null || !orders.Any())
                {
                    return new List<MonthOption>
            {
                new MonthOption(DateTime.Now.Year, DateTime.Now.Month)
            };
                }

                var minDate = orders.Min(o => o.RetrieveOrderDate());
                var maxDate = orders.Max(o => o.RetrieveOrderDate());

                var months = new List<MonthOption>();

                for (var date = minDate; date <= maxDate; date = date.AddMonths(1))
                {
                    months.Add(new MonthOption(date.Year, date.Month));
                }

                // Add current month if not already included
                if (!months.Any(m => m.Year == DateTime.Now.Year && m.Month == DateTime.Now.Month))
                {
                    months.Add(new MonthOption(DateTime.Now.Year, DateTime.Now.Month));
                }

                return months.OrderByDescending(m => m.Year).ThenByDescending(m => m.Month).ToList();
            }
            catch
            {
                return new List<MonthOption>
        {
            new MonthOption(DateTime.Now.Year, DateTime.Now.Month)
        };
            }
        }

        // Helper class for month options
        public class MonthOption
        {
            public int Year { get; }
            public int Month { get; }
            public string DisplayName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");

            public MonthOption(int year, int month)
            {
                Year = year;
                Month = month;
            }
        }
    }
}