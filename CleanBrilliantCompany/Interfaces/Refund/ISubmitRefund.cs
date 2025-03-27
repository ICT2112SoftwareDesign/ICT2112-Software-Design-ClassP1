using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ISubmitRefund
    {
        Refund_RDM SubmitRefund(int orderId, string refundReason, float refundAmount, Dictionary<int, int> refundedProducts);
    }
}
