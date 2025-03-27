using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class RefundManagement : IRefundQuery, ISubmitRefund
    {   
        private readonly IRefundDatabase _refundDatabase;
        private readonly IOrder _order;

        public RefundManagement(IRefundDatabase refundDatabase, IOrder order)
        {
            _refundDatabase = refundDatabase;
            _order = order;
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
            string orderStatus = "";
            if (status == "Approved")
            {
                orderStatus = "Refunded";
            }
            else {
                orderStatus = "Rejected";
            }
            _order.updateOrderStatus(refundId, orderStatus);
        }

        public Refund_RDM SubmitRefund(int orderId, string refundReason, float refundAmount, Dictionary<int, int> refundedProducts)
        {
            return _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, refundedProducts);
        }

    }
}
