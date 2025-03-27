using CleanBrilliantCompany.Models;
using System;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundDatabase
    {
        Refund_RDM InsertRefund(int orderId, string refundReason, float refundAmount, List<string> images, List<string> videos, Dictionary<int, int> refundedProducts);
        Refund_RDM ViewRefund(int refundId);
        List<Refund_RDM> GetAllRefunds();
        void UpdateRefundStatus(int refundId, string status, DateTime processedDate);
        public int GetTotalRefundCount();
        public int GetPendingRefundCount();
        public decimal GetTotalRefundAmount();
    }
}