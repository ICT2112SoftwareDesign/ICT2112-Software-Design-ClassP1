using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace CleanBrilliantCompany.Models
{
    public class DashboardManagement
    {
        private readonly IOrder _order;
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundQuery _refundQuery;

        public DashboardManagement(IOrder order, IRefundQuery refundQuery, IOrderDatabase orderDatabase)
        {
            _order = order;
            _orderDatabase = orderDatabase;
            _refundQuery = refundQuery;

            // Log connection information
            LogDiagnosticInfo("Dashboard initialized with order and refund databases");
        }

        // Added method to get order items
        public List<int> getOrderItemIds(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId); // needs to be added to IOrder
            if (order == null)
            {
                LogDiagnosticInfo($"Order with ID {orderId} not found.");
                return new List<int>();
            }

            return order.RetrieveOrderItems();
        }

        // Method to check if data is available
        public bool IsDashboardDataAvailable()
        {
            try
            {
                LogDiagnosticInfo("Checking dashboard data availability");

                var orders = _order.getAllOrders();
                LogDiagnosticInfo($"Orders retrieved: {(orders != null ? orders.Count : 0)}");

                var refunds = _refundQuery.GetAllRefunds();
                LogDiagnosticInfo($"Refunds retrieved: {(refunds != null ? refunds.Count : 0)}");

                // Log to a file for debugging
                File.AppendAllText("dashboard_log.txt",
                    $"[{DateTime.Now}] Orders: {orders?.Count ?? 0}, Refunds: {refunds?.Count ?? 0}\n");

                bool hasData = (orders != null && orders.Any()) || (refunds != null && refunds.Any());
                LogDiagnosticInfo($"Data available: {hasData}");
                return hasData;
            }
            catch (Exception ex)
            {
                LogDiagnosticInfo($"Error checking dashboard data: {ex.Message}", isError: true);
                File.AppendAllText("dashboard_error_log.txt",
                    $"[{DateTime.Now}] Error: {ex.Message}\n{ex.StackTrace}\n");
                return false;
            }
        }

        // Dashboard metrics with additional logging
        public int TotalOrderCount
        {
            get
            {
                try
                {
                    var orders = _order.getAllOrders();
                    var count = orders?.Count ?? 0;
                    LogDiagnosticInfo($"Total order count: {count}");
                    return count;
                }
                catch (Exception ex)
                {
                    LogDiagnosticInfo($"Error getting total order count: {ex.Message}", isError: true);
                    return 0;
                }
            }
        }

        public int TotalRefundCount
        {
            get
            {
                try
                {
                    var refunds = _refundQuery.GetAllRefunds();
                    var count = refunds?.Count ?? 0;
                    LogDiagnosticInfo($"Total refund count: {count}");
                    return count;
                }
                catch (Exception ex)
                {
                    LogDiagnosticInfo($"Error getting total refund count: {ex.Message}", isError: true);
                    return 0;
                }
            }
        }

        public int PendingRefundCount
        {
            get
            {
                try
                {
                    var refunds = _refundQuery.GetAllRefunds();
                    var count = refunds?.Count(r => r.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase)) ?? 0;
                    LogDiagnosticInfo($"Pending refund count: {count}");
                    return count;
                }
                catch (Exception ex)
                {
                    LogDiagnosticInfo($"Error getting pending refund count: {ex.Message}", isError: true);
                    return 0;
                }
            }
        }

        public decimal NetRevenue => CalculateNetRevenue();

        private decimal CalculateNetRevenue()
        {
            try
            {
                var orders = _order.getAllOrders();
                var refunds = _refundQuery.GetAllRefunds();

                decimal revenue = 0;
                if (orders != null)
                {
                    revenue = orders.Sum(o => o.RetrieveOrderTotal());
                    LogDiagnosticInfo($"Calculated total revenue: {revenue}");
                }

                decimal refunded = 0;
                if (refunds != null)
                {
                    refunded = refunds
                        .Where(r => r.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                        .Sum(r => (decimal)r.RefundAmount);
                    LogDiagnosticInfo($"Calculated total refunded: {refunded}");
                }

                decimal netRevenue = revenue - refunded;
                LogDiagnosticInfo($"Net revenue calculation: Total Revenue = {revenue}, Total Refunded = {refunded}, Net = {netRevenue}");
                return netRevenue;
            }
            catch (Exception ex)
            {
                LogDiagnosticInfo($"Error calculating net revenue: {ex.Message}", isError: true);
                return 0;
            }
        }

        // Method to get all metrics at once with detailed logging
        public (int orders, int refunds, int pendingRefunds, decimal netRevenue) GetDashboardMetrics()
        {
            try
            {
                LogDiagnosticInfo("Retrieving all dashboard metrics");

                var metrics = (
                    TotalOrderCount,
                    TotalRefundCount,
                    PendingRefundCount,
                    NetRevenue
                );

                LogDiagnosticInfo($"Dashboard metrics: Orders={metrics.Item1}, Refunds={metrics.Item2}, Pending={metrics.Item3}, Revenue=${metrics.Item4}");
                return metrics;
            }
            catch (Exception ex)
            {
                LogDiagnosticInfo($"Error retrieving dashboard metrics: {ex.Message}", isError: true);
                return (0, 0, 0, 0);
            }
        }

        // Helper method for consistent logging
        private void LogDiagnosticInfo(string message, bool isError = false)
        {
            var logMessage = $"[{DateTime.Now}] [DashboardManagement] {message}";

            if (isError)
            {
                Console.Error.WriteLine(logMessage);
                File.AppendAllText("dashboard_errors.txt", logMessage + Environment.NewLine);
            }
            else
            {
                Console.WriteLine(logMessage);
                File.AppendAllText("dashboard_debug.txt", logMessage + Environment.NewLine);
            }
        }

        public Dictionary<DateTime, int> GetDailyOrderCountsForMonth(DateTime selectedDate)
        {
            try
            {
                var orders = _order.getAllOrders();

                var startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                // Initialize dictionary with all days in month set to 0
                var dailyCounts = new Dictionary<DateTime, int>();
                for (var day = startDate; day <= endDate; day = day.AddDays(1))
                {
                    dailyCounts[day] = 0;
                }

                // Populate with actual orders if they exist
                var ordersInMonth = orders
                    .Where(o => o.RetrieveOrderDate() >= startDate && o.RetrieveOrderDate() <= endDate)
                    .GroupBy(o => o.RetrieveOrderDate().Date);

                foreach (var group in ordersInMonth)
                {
                    dailyCounts[group.Key] = group.Count();
                }

                return dailyCounts;
            }
            catch
            {
                return new Dictionary<DateTime, int>();
            }
        }

        public Dictionary<DateTime, int> GetDailyRefundCountsForMonth(DateTime selectedDate)
        {
            try
            {
                var refund = _refundQuery.GetAllRefunds();

                var startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                // Initialize dictionary with all days in month set to 0
                var dailyCounts = new Dictionary<DateTime, int>();
                for (var day = startDate; day <= endDate; day = day.AddDays(1))
                {
                    dailyCounts[day] = 0;
                }

                // Populate with actual orders if they exist
                var refundsInMonth = refund
                    .Where(r => r.RefundRequestDate >= startDate && r.RefundRequestDate <= endDate)
                    .GroupBy(r => r.RefundRequestDate);

                foreach (var group in refundsInMonth)
                {
                    dailyCounts[group.Key] = group.Count();
                }

                return dailyCounts;
            }
            catch
            {
                return new Dictionary<DateTime, int>();
            }
        }

        public (decimal GrossProfit, decimal RefundAmount, decimal NetProfit) GetProfitComponentsForMonths(DateTime selectedDate)
        {
            try
            {
                var startDate = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                var orders = _order.getAllOrders()
                    .Where(o => o.RetrieveOrderDate() >= startDate && o.RetrieveOrderDate() <= endDate)
                    .ToList();
                var refunds = _refundQuery.GetAllRefunds()
                    .Where(r => r.RefundRequestDate >= startDate && r.RefundRequestDate <= endDate)
                    .ToList();

                decimal grossProfit = orders.Sum(o => o.RetrieveOrderTotal());
                decimal refundAmount = refunds
                    .Where(r => r.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                    .Sum(r => (decimal)r.RefundAmount);
                decimal netProfit = grossProfit - refundAmount;

                return (grossProfit, refundAmount, netProfit);
            }
            catch (Exception ex)
            {
                LogDiagnosticInfo($"Error calculating profit components: {ex.Message}", isError: true);
                return (0, 0, 0);
            }
        }

    }
}