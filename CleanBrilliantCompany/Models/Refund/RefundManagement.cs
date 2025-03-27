using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class RefundManagement : IRefundQuery, ISubmitRefund
    {   
        private readonly IRefundDatabase _refundDatabase;
        private readonly Func<IOrder> _orderFactory;

        public RefundManagement(IRefundDatabase refundDatabase, Func<IOrder> orderFactory)
        {
            _refundDatabase = refundDatabase;
            _orderFactory = orderFactory;
        }
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

            string orderStatus = status == "Approved" ? "Refunded" : "Rejected";

            var refundDetails = _refundDatabase.ViewRefund(refundId);
            int orderId = refundDetails.OrderId;

            // Resolve IOrder only when needed
            var orderService = _orderFactory();
            orderService.updateOrderStatus(orderId, orderStatus);
        }

        public Refund_RDM SubmitRefund(int orderId, string refundReason, float refundAmount, Dictionary<int, int> refundedProducts)
        {
            return _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, refundedProducts);
        }

    }
}
