using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Refund;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class RefundManagement : IRefundQuery, ISubmitRefund
    {   
        private readonly IRefundDatabase _refundDatabase;

        public RefundManagement(IRefundDatabase refundDatabase)
        {
            _refundDatabase = refundDatabase;
        }

        public Refund_RDM ProcessRefund(int orderId, string refundReason, float refundAmount, List<string> images, List<string> videos, Dictionary<int, int> refundedProducts)
        {
            return _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, images, videos, refundedProducts);
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
        }

        public Refund_RDM SubmitRefund(int orderId, string refundReason, float refundAmount, List<string> images, List<string> videos, Dictionary<int, int> refundedProducts)
        {
            return _refundDatabase.InsertRefund(orderId, refundReason, refundAmount, images, videos, refundedProducts);
        }

    }
}
