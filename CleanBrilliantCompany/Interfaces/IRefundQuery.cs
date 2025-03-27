using CleanBrilliantCompany.Models;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IRefundQuery
    {
        Refund_RDM GetRefundDetails(int refundId);
        List<Refund_RDM> GetAllRefunds();
        void UpdateRefund(int refundId, string status);
        Refund_RDM ProcessRefund(int orderId, string refundReason, float refundAmount, List<string> images, List<string> videos, Dictionary<int, int> refundedProducts);

    }
}