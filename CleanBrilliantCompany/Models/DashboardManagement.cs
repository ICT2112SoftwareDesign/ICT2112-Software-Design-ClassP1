using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.IO;

namespace CleanBrilliantCompany.Models
{
    public class DashboardManagement : IOrder, IRefundQuery
    {
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundDatabase _refundDatabase;

        public DashboardManagement(IOrderDatabase orderDatabase, IRefundDatabase refundDatabase)
        {
            _orderDatabase = orderDatabase;
            _refundDatabase = refundDatabase;
            
            // Log connection information
            LogDiagnosticInfo("Dashboard initialized with order and refund databases");
        }

        // Added method to get order items
        public List<int> getOrderItemIds(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null)
            {
                LogDiagnosticInfo($"Order with ID {orderId} not found.");
                return new List<int>();
            }

            return order.GetOrderItems(); 
        }

        // Method to check if data is available
        public bool IsDashboardDataAvailable()
        {
            try {
                LogDiagnosticInfo("Checking dashboard data availability");
                
                var orders = getAllOrders();
                LogDiagnosticInfo($"Orders retrieved: {(orders != null ? orders.Count : 0)}");
                
                var refunds = GetAllRefunds();
                LogDiagnosticInfo($"Refunds retrieved: {(refunds != null ? refunds.Count : 0)}");
                
                // Log to a file for debugging
                File.AppendAllText("dashboard_log.txt", 
                    $"[{DateTime.Now}] Orders: {orders?.Count ?? 0}, Refunds: {refunds?.Count ?? 0}\n");
                
                bool hasData = (orders != null && orders.Any()) || (refunds != null && refunds.Any());
                LogDiagnosticInfo($"Data available: {hasData}");
                return hasData;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error checking dashboard data: {ex.Message}", isError: true);
                File.AppendAllText("dashboard_error_log.txt", 
                    $"[{DateTime.Now}] Error: {ex.Message}\n{ex.StackTrace}\n");
                return false;
            }
        }

        // Existing implementation
        public OrderRDM getOrderDetails(int orderId)
        {
            try {
                var order = _orderDatabase.getOrderById(orderId);
                LogDiagnosticInfo($"Retrieved order details for ID {orderId}: {(order != null ? "Found" : "Not found")}");
                return order;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error retrieving order {orderId}: {ex.Message}", isError: true);
                return null;
            }
        }

        public List<OrderRDM> getOrderHistory(int customerId)
        {
            try {
                var orders = _orderDatabase.getOrdersByCustomerId(customerId);
                LogDiagnosticInfo($"Retrieved {orders?.Count ?? 0} orders for customer {customerId}");
                return orders;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error retrieving orders for customer {customerId}: {ex.Message}", isError: true);
                return new List<OrderRDM>();
            }
        }

        public List<OrderRDM> getAllOrders()
        {
            try {
                LogDiagnosticInfo("Attempting to retrieve all orders");
                var orders = _orderDatabase.getAllOrders();
                LogDiagnosticInfo($"Retrieved {orders?.Count ?? 0} orders from database");
                
                // For debugging, log order IDs
                if (orders != null && orders.Any()) {
                    var orderIds = string.Join(", ", orders.Take(5).Select(o => o.GetOrderID()));
                    LogDiagnosticInfo($"Sample order IDs: {orderIds}...");
                }
                
                return orders;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error retrieving all orders: {ex.Message}", isError: true);
                return new List<OrderRDM>();
            }
        }

        public bool cancelOrder(int orderId)
        {
            try {
                var order = _orderDatabase.getOrderById(orderId);
                if (order == null || order.GetStatus() == "Cancelled") {
                    LogDiagnosticInfo($"Cannot cancel order {orderId}: {(order == null ? "Not found" : "Already cancelled")}");
                    return false;
                }

                order.SetStatus("Cancelled");
                bool success = _orderDatabase.updateOrder(order);
                LogDiagnosticInfo($"Order {orderId} cancellation result: {success}");
                return success;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error canceling order {orderId}: {ex.Message}", isError: true);
                return false;
            }
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            try {
                bool success = _orderDatabase.updateOrderStatus(orderId, status);
                LogDiagnosticInfo($"Updated order {orderId} status to {status}: {success}");
                return success;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error updating order {orderId} status: {ex.Message}", isError: true);
                return false;
            }
        }

        // IRefundQuery implementation with logging
        public Refund_RDM GetRefundDetails(int refundId)
        {
            try {
                LogDiagnosticInfo($"Retrieving refund details for ID {refundId}");
                var refund = _refundDatabase.ViewRefund(refundId);
                LogDiagnosticInfo($"Retrieved refund {refundId}: Status = {refund?.Status ?? "Not Found"}");
                return refund;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error retrieving refund {refundId}: {ex.Message}", isError: true);
                return null;
            }
        }

        public List<Refund_RDM> GetAllRefunds()
        {
            try {
                LogDiagnosticInfo("Attempting to retrieve all refunds");
                var refunds = _refundDatabase.GetAllRefunds();
                LogDiagnosticInfo($"Retrieved {refunds?.Count ?? 0} refunds from database");
                
                // For debugging, log refund IDs
                if (refunds != null && refunds.Any()) {
                    var refundIds = string.Join(", ", refunds.Take(5).Select(r => r.RefundId));
                    LogDiagnosticInfo($"Sample refund IDs: {refundIds}...");
                }
                
                return refunds;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error retrieving all refunds: {ex.Message}", isError: true);
                return new List<Refund_RDM>();
            }
        }

        public void UpdateRefund(int refundId, string status)
        {
            try {
                _refundDatabase.UpdateRefundStatus(refundId, status, DateTime.Now);
                LogDiagnosticInfo($"Updated refund {refundId} status to {status}");
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error updating refund {refundId}: {ex.Message}", isError: true);
            }
        }

        public Refund_RDM ProcessRefund(int orderId, string refundReason, float refundAmount, 
            List<string> images, List<string> videos, Dictionary<int, int> refundedProducts)
        {
            try {
                LogDiagnosticInfo($"Processing refund for order {orderId} with amount {refundAmount}");
                var refund = _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, images, videos, refundedProducts);
                LogDiagnosticInfo($"Processed refund: ID = {refund?.RefundId ?? -1}");
                return refund;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error processing refund for order {orderId}: {ex.Message}", isError: true);
                return null;
            }
        }

        // Dashboard metrics with additional logging
        public int TotalOrderCount {
            get {
                try {
                    var orders = getAllOrders();
                    var count = orders?.Count ?? 0;
                    LogDiagnosticInfo($"Total order count: {count}");
                    return count;
                }
                catch (Exception ex) {
                    LogDiagnosticInfo($"Error getting total order count: {ex.Message}", isError: true);
                    return 0;
                }
            }
        }
        
        public int TotalRefundCount {
            get {
                try {
                    var refunds = GetAllRefunds();
                    var count = refunds?.Count ?? 0;
                    LogDiagnosticInfo($"Total refund count: {count}");
                    return count;
                }
                catch (Exception ex) {
                    LogDiagnosticInfo($"Error getting total refund count: {ex.Message}", isError: true);
                    return 0;
                }
            }
        }
        
        public int PendingRefundCount {
            get {
                try {
                    var refunds = GetAllRefunds();
                    var count = refunds?.Count(r => r.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase)) ?? 0;
                    LogDiagnosticInfo($"Pending refund count: {count}");
                    return count;
                }
                catch (Exception ex) {
                    LogDiagnosticInfo($"Error getting pending refund count: {ex.Message}", isError: true);
                    return 0;
                }
            }
        }
        
        public decimal NetRevenue => CalculateNetRevenue();

        private decimal CalculateNetRevenue()
        {
            try {
                var orders = getAllOrders();
                var refunds = GetAllRefunds();

                decimal revenue = 0;
                if (orders != null) {
                    revenue = orders.Sum(o => o.GetOrderTotal());
                    LogDiagnosticInfo($"Calculated total revenue: {revenue}");
                }

                decimal refunded = 0;
                if (refunds != null) {
                    refunded = refunds
                        .Where(r => r.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                        .Sum(r => (decimal)r.RefundAmount);
                    LogDiagnosticInfo($"Calculated total refunded: {refunded}");
                }
                
                decimal netRevenue = revenue - refunded;
                LogDiagnosticInfo($"Net revenue calculation: Total Revenue = {revenue}, Total Refunded = {refunded}, Net = {netRevenue}");
                return netRevenue;
            }
            catch (Exception ex) {
                LogDiagnosticInfo($"Error calculating net revenue: {ex.Message}", isError: true);
                return 0;
            }
        }

        // Method to get all metrics at once with detailed logging
        public (int orders, int refunds, int pendingRefunds, decimal netRevenue) GetDashboardMetrics()
        {
            try {
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
            catch (Exception ex) {
                LogDiagnosticInfo($"Error retrieving dashboard metrics: {ex.Message}", isError: true);
                return (0, 0, 0, 0);
            }
        }
        
        // Helper method for consistent logging
        private void LogDiagnosticInfo(string message, bool isError = false)
        {
            var logMessage = $"[{DateTime.Now}] [DashboardManagement] {message}";
            
            if (isError) {
                Console.Error.WriteLine(logMessage);
                File.AppendAllText("dashboard_errors.txt", logMessage + Environment.NewLine);
            } else {
                Console.WriteLine(logMessage);
                File.AppendAllText("dashboard_debug.txt", logMessage + Environment.NewLine);
            }
        }
    }
}