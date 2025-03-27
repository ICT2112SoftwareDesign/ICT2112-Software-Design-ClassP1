using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Interfaces.Refund
{
    public interface ISubmitRefund
    {
        Refund_RDM SubmitRefund(int orderId, string refundReason, float refundAmount, List<string> images, List<string> videos, Dictionary<int, int> refundedProducts);
    }
}