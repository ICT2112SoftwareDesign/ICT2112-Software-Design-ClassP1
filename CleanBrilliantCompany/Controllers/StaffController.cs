using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers.Staff
{
    [Route("staff")]
    public class StaffController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(); // Renders Views/Staff/Index.cshtml
        }

        [HttpGet("shipping")]
        public IActionResult Shipping()
        {
            return View(); // Renders Views/Staff/Shipping.cshtml
        }

        [HttpGet("refund")]
        public IActionResult Refund([FromServices] IRefundQuery refundService)
        {
            var refunds = refundService.GetAllRefunds();
            return View(refunds);
        }

        [HttpGet("refund/details/{id}")]
        public IActionResult RefundDetails(int id, [FromServices] IRefundQuery refundService)
        {
            var refund = refundService.GetRefundDetails(id);
            if (refund == null)
            {
                return NotFound();
            }
            return View("refund-details", refund);
        }

        [HttpPost("refund/update-status")]
        public IActionResult UpdateRefundStatus(int refundId, string status, [FromServices] IRefundQuery refundService, [FromServices] IRefundDetails refundDetails)
        {
            var refund = refundService.GetRefundDetails(refundId);
            if (refund == null)
            {
                return NotFound();
            }

            // Prevent updates if refund is already Approved or Rejected
            if (refund.Status == "Approved" || refund.Status == "Rejected")
            {
                TempData["ErrorMessage"] = "This refund has already been processed and cannot be updated.";
                return RedirectToAction("RefundDetails", new { id = refundId });
            }

            if (status == "Approved")
            {
                List<int> itemIds = new List<int>(refund.RefundedProducts.Keys);
                refundDetails.ReturnItemToInventory(itemIds, refund.RefundReason);
                Console.WriteLine($"{refund.RefundAmount} has been refunded to the customer in Order {refund.OrderId}.");

            }
            // Update refund status
            refund.Status = status;
            refund.RefundProcessedDate = DateTime.Now;

            TempData["SuccessMessage"] = "Refund status updated successfully.";
            return RedirectToAction("Refund");
        }



        [HttpGet("reorder")]
        public IActionResult Reorder()
        {
            return View(); // Renders Views/Staff/Reorder.cshtml
        }
        [HttpGet("ticket")]
        public IActionResult Ticket()
        {
            return View(); // Renders Views/Staff/Ticket.cshtml
        }
    }
}