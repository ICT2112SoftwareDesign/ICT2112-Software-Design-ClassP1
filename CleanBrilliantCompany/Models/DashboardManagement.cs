using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        }

        // IOrder implementation (same as OrderFulfilmentManagement)
        public OrderRDM getOrderDetails(int orderId)
        {
            return _orderDatabase.getOrderById(orderId);
        }

        public List<OrderRDM> getOrderHistory(int customerId)
        {
            return _orderDatabase.getOrdersByCustomerId(customerId);
        }

        public List<OrderRDM> getAllOrders()
        {
            return _orderDatabase.getAllOrders();
        }

        public bool cancelOrder(int orderId)
        {
            var order = _orderDatabase.getOrderById(orderId);
            if (order == null || order.GetStatus() == "Cancelled")
                return false;

            order.SetStatus("Cancelled");
            return _orderDatabase.updateOrder(order);
        }

        public bool updateOrderStatus(int orderId, string status)
        {
            return _orderDatabase.updateOrderStatus(orderId, status);
        }

        // IRefundQuery implementation
        public Refund_RDM GetRefundDetails(int refundId)
        {
            return _refundDatabase.ViewRefund(refundId);
        }

        public List<Refund_RDM> GetAllRefunds()
        {
            return _refundDatabase.GetAllRefunds();
        }

        public void UpdateRefund(int refundId, string status)
        {
            _refundDatabase.UpdateRefundStatus(refundId, status, DateTime.Now);
        }

        public Refund_RDM ProcessRefund(int orderId, string refundReason, float refundAmount, 
            List<string> images, List<string> videos, Dictionary<int, int> refundedProducts)
        {
            return _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, images, videos, refundedProducts);
        }

        // Dashboard metrics as properties
        public int TotalOrderCount => getAllOrders()?.Count ?? 0;
        
        public int TotalRefundCount => GetAllRefunds()?.Count ?? 0;
        
        public int PendingRefundCount => GetAllRefunds()?
            .Count(r => r.Status.Equals("Pending", StringComparison.OrdinalIgnoreCase)) ?? 0;
        
        public decimal NetRevenue => CalculateNetRevenue();

        private decimal CalculateNetRevenue()
        {
            var orders = getAllOrders();
            var refunds = GetAllRefunds();

            decimal revenue = orders?.Sum(o => o.GetOrderTotal()) ?? 0;
            decimal refunded = refunds?
                .Where(r => r.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
                .Sum(r => (decimal)r.RefundAmount) ?? 0;
            
            return revenue - refunded;
        }

        // Method to get all metrics at once
        public (int orders, int refunds, int pendingRefunds, decimal netRevenue) GetDashboardMetrics()
        {
            return (
                TotalOrderCount,
                TotalRefundCount,
                PendingRefundCount,
                NetRevenue
            );
        }
    }
}