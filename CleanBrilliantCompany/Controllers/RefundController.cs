using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers.Refund
{
    [Route("staff/refund")]
    public class RefundController : ApplicationController
    {
        private readonly IRefundQuery _refundManagement;

        public RefundController(IRefundQuery refundManagement, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _refundManagement = refundManagement;
        }

        [HttpGet("")]
        public IActionResult Refund()
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access refunds.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var refunds = _refundManagement.GetAllRefunds();
            return View("~/Views/StaffRefund/refund.cshtml", refunds);
        }

        [HttpGet("details/{id}")]
        public IActionResult RefundDetails(int id)
        {
            var refund = _refundManagement.GetRefundDetails(id);

            if (refund == null || refund.Status == "Not Found")
            {
                return NotFound("Refund record not found.");
            }

            return View("~/Views/StaffRefund/refund-details.cshtml", refund);
        }

        [HttpPost("update-status")]
        public IActionResult UpdateRefundStatus(int refundId, string status, [FromServices] IRefundDetails refundDetails)
        {
            var refund = _refundManagement.GetRefundDetails(refundId);
            if (refund == null)
            {
                return NotFound();
            }

            if (refund.Status == "Approved" || refund.Status == "Rejected")
            {
                TempData["ErrorMessage"] = "This refund has already been processed.";
                return RedirectToAction("RefundDetails", new { id = refundId });
            }

            if (status == "Approved")
            {
                List<int> itemIds = new List<int>(refund.RefundedProducts.Keys);
                refundDetails.ReturnItemToInventory(itemIds, refund.RefundReason);
                Console.WriteLine($"{refund.RefundAmount} has been refunded to the customer in Order {refund.OrderId}.");
            }

            _refundManagement.UpdateRefund(refundId, status);

            TempData["SuccessMessage"] = "Refund status updated successfully.";
            return RedirectToAction("Refund");
        }

        [HttpPost("CreateRefund")]
        public IActionResult CreateRefund([FromServices] ISubmitRefund submitRefund)
        {
            

            Refund_RDM newRefund = submitRefund.SubmitRefund(
                28,
                "Wrong Items Sent",
                3.00f,
                new Dictionary<int, int>()
                {
                    { 1, 1 },
                    { 2, 1 }
                }
            );

            if (newRefund != null)
            {
                TempData["SuccessMessage"] = "Refund request submitted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to process refund request.";
            }

            return RedirectToAction("Refund");
        }
    }
}
