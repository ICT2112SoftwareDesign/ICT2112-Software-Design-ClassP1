using System;
using System.Linq;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using Microsoft.Extensions.Logging;

namespace CleanBrilliantCompany.Models
{
    public class DashboardManagement : IOrder, IRefundQuery
    {
        private readonly IOrderDatabase _orderDatabase;
        private readonly IRefundDatabase _refundDatabase;
        private readonly ILogger<DashboardManagement> _logger;

        public DashboardManagement(
            IOrderDatabase orderDatabase,
            IRefundDatabase refundDatabase,
            ILogger<DashboardManagement> logger)
        {
            _orderDatabase = orderDatabase ??
                throw new ArgumentNullException(nameof(orderDatabase));
            _refundDatabase = refundDatabase ??
                throw new ArgumentNullException(nameof(refundDatabase));
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public DashboardSummary GetDashboardSummary()
        {
            try
            {
                // Consistent with your data access patterns
                var allOrders = _orderDatabase.getAllOrders();
                var allRefunds = _refundDatabase.GetAllRefunds();

                // Following your defensive programming approach
                allOrders = allOrders ?? new List<OrderRDM>();
                allRefunds = allRefunds ?? new List<Refund_RDM>();

                // Logging similar to your OrderMapper
                _logger.LogInformation($"Retrieved {allOrders.Count} orders and {allRefunds.Count} refunds");

                return new DashboardSummary
                {
                    TotalOrders = allOrders.Count,
                    TotalRefunds = allRefunds.Count,
                    PendingRefunds = CalculatePendingRefunds(allRefunds),
                    NetRevenue = CalculateNetRevenue(allOrders, allRefunds)
                };
            }
            catch (Exception ex)
            {
                // Consistent with your error handling pattern
                _logger.LogError(ex, "Failed to generate dashboard summary");
                return new DashboardSummary
                {
                    TotalOrders = -1,
                    TotalRefunds = -1,
                    PendingRefunds = -1,
                    NetRevenue = -1
                };
            }
        }

        private int CalculatePendingRefunds(List<Refund_RDM> refunds)
        {
            // Matching your null safety patterns from RefundMapper
            if (refunds == null || refunds.Count == 0)
            {
                _logger.LogDebug("No refunds available for pending count calculation");
                return 0;
            }

            // Using same status comparison as in RefundMapper
            return refunds.Count(r => 
                r != null && 
                !string.IsNullOrWhiteSpace(r.Status) && 
                r.Status.Trim().Equals("Pending", StringComparison.OrdinalIgnoreCase));
        }

        private decimal CalculateNetRevenue(List<OrderRDM> orders, List<Refund_RDM> refunds)
        {
            // Following your order total calculation from OrderManagement
            decimal totalRevenue = orders?
                .Where(o => o != null && o.GetOrderTotal() > 0)
                .Sum(o => o.GetOrderTotal()) ?? 0;

            // Consistent with your refund processing in RefundManagement
            decimal totalApprovedRefunds = refunds?
                .Where(r => r != null && 
                           !string.IsNullOrWhiteSpace(r.Status) &&
                           r.Status.Trim().Equals("Approved", StringComparison.OrdinalIgnoreCase))
                .Sum(r => (decimal)r.RefundAmount) ?? 0;

            _logger.LogDebug($"Calculated net revenue: {totalRevenue} - {totalApprovedRefunds}");

            return totalRevenue - totalApprovedRefunds;
        }

        // IOrder interface implementation
        public OrderRDM getOrderDetails(int orderId)
        {
            try
            {
                var order = _orderDatabase.getOrderById(orderId);
                if (order == null)
                {
                    _logger.LogWarning($"Order {orderId} not found");
                    throw new KeyNotFoundException($"Order {orderId} not found");
                }
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get order details for {orderId}");
                throw new Exception($"Failed to fetch order details: {ex.Message}");
            }
        }

        public List<OrderRDM> getOrderHistory(int customerId)
        {
            try
            {
                var orders = _orderDatabase.getOrdersByCustomerId(customerId);
                return orders ?? new List<OrderRDM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get order history for customer {customerId}");
                throw new Exception($"Failed to fetch order history: {ex.Message}");
            }
        }

        public List<OrderRDM> getAllOrders()
        {
            try
            {
                var orders = _orderDatabase.getAllOrders();
                
                if (orders == null)
                {
                    _logger.LogWarning("No orders found in database");
                    throw new Exception("No orders found");
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all orders");
                throw new Exception($"Failed to fetch all orders: {ex.Message}");
            }
        }

        public bool cancelOrder(int orderId)
        {
            try
            {
                var order = _orderDatabase.getOrderById(orderId);
                if (order == null || order.GetStatus() == "Cancelled")
                {
                    _logger.LogWarning($"Order {orderId} not found or already cancelled");
                    return false;
                }

                order.SetStatus("Cancelled");
                bool success = _orderDatabase.updateOrder(order);
                
                if (success)
                {
                    _logger.LogInformation($"Successfully cancelled order {orderId}");
                }
                else
                {
                    _logger.LogWarning($"Failed to cancel order {orderId}");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error cancelling order {orderId}");
                throw new Exception($"Failed to cancel order: {ex.Message}");
            }
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            try
            {
                bool success = _orderDatabase.updateOrderStatus(orderId, status);
                
                if (success)
                {
                    _logger.LogInformation($"Updated order {orderId} status to {status}");
                }
                else
                {
                    _logger.LogWarning($"Failed to update status for order {orderId}");
                }

                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating status for order {orderId}");
                return false;
            }
        }

        // IRefundQuery interface implementation
        public Refund_RDM ProcessRefund(int orderId, string refundReason, float refundAmount, 
            List<string> images, List<string> videos, Dictionary<int, int> refundedProducts)
        {
            try
            {
                var refund = _refundDatabase.InsertRefund(
                    orderId, refundReason, refundAmount, images, videos, refundedProducts);
                
                _logger.LogInformation($"Processed refund {refund?.RefundId} for order {orderId}");
                return refund;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to process refund for order {orderId}");
                throw new Exception($"Failed to process refund: {ex.Message}");
            }
        }

        public Refund_RDM GetRefundDetails(int refundId)
        {
            try
            {
                var refund = _refundDatabase.ViewRefund(refundId);
                if (refund == null)
                {
                    _logger.LogWarning($"Refund {refundId} not found");
                    throw new KeyNotFoundException($"Refund {refundId} not found");
                }
                return refund;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get refund details for {refundId}");
                throw new Exception($"Failed to fetch refund details: {ex.Message}");
            }
        }

        public List<Refund_RDM> GetAllRefunds()
        {
            try
            {
                var refunds = _refundDatabase.GetAllRefunds();
                return refunds ?? new List<Refund_RDM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get all refunds");
                throw new Exception($"Failed to fetch all refunds: {ex.Message}");
            }
        }

        public void UpdateRefund(int refundId, string status)
        {
            try
            {
                _refundDatabase.UpdateRefundStatus(refundId, status, DateTime.Now);
                _logger.LogInformation($"Updated refund {refundId} status to {status}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to update refund {refundId}");
                throw new Exception($"Failed to update refund: {ex.Message}");
            }
        }
    }

    public class DashboardSummary
    {
        public int TotalOrders { get; set; }
        public decimal NetRevenue { get; set; }
        public int TotalRefunds { get; set; }
        public int PendingRefunds { get; set; }

        public bool IsValid() =>
            TotalOrders >= 0 &&
            NetRevenue >= 0 &&
            TotalRefunds >= 0 &&
            PendingRefunds >= 0;
    }
}