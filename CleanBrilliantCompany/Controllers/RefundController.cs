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
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Staff Dashboard.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            var refund = _refundManagement.GetRefundDetails(id);

            if (refund == null || refund.Status == "Not Found")
            {
                return NotFound("Refund record not found.");
            } 

            return View("~/Views/StaffRefund/refund-details.cshtml", refund);
        }

        [HttpPost("update-status")]
        public IActionResult UpdateRefundStatus(int refundId, string status)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Staff Dashboard.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            _refundManagement.UpdateRefund(refundId, status);

            TempData["SuccessMessage"] = "Refund status updated successfully.";
            return RedirectToAction("Refund");
        }        
    }
}
