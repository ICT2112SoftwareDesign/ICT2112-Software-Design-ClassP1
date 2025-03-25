using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers.Reorder
{
    [Route("staff/reorder-requests")]
    public class ReorderRequestController : Controller
    {
        private readonly IReorderQuery _reorderManagement;

        public ReorderRequestController(IReorderQuery reorderManagement)
        {
            _reorderManagement = reorderManagement;
        }

        [HttpGet("")]
        public IActionResult Reorder()
        {
            var reorders = _reorderManagement.displayListOfReorders();
            return View("~/Views/Staff/ListOfReorders.cshtml", reorders);
        }

        [HttpGet("details/{id}")]
        public IActionResult ReorderDetails(int id)
        {
            var reorder = _reorderManagement.getReorderRequestDetails(id);

            if (reorder == null || reorder.Status == "Not Found")
            {
                return NotFound("Reorder Request record not found.");
            }

            return View("~/Views/Staff/ReorderDetails.cshtml", reorder);
        }

        [HttpGet("new")]
        public IActionResult CreateReorderForm()
        {
            return View("~/Views/Staff/ReorderForm.cshtml");
        }

        [HttpPost("new")]
        public IActionResult SubmitReorderForm(ReorderRequest_RDM reorder)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Staff/ReorderForm.cshtml", reorder);
            }

            reorder.ExpectedDeliveryDate = null;
            reorder.DefectQuantity = null;
            reorder.Status = "Pending";

            _reorderManagement.createReorderRequest(reorder);

            TempData["SuccessMessage"] = "Reorder request created successfully.";
            return RedirectToAction("Reorder");
        }


        [HttpPost("update_details")]
        public IActionResult UpdateReorderDetails(ReorderRequest_RDM updated)
        {
            var existing = _reorderManagement.getReorderRequestDetails(updated.ReorderId);
            if (existing == null) return NotFound();

            // apply logic
            if (existing.Status == "Pending")
            {
                existing.ProductId = updated.ProductId;
                existing.Quantity = updated.Quantity;
                existing.ManufacturerId = updated.ManufacturerId;
            }

            if (existing.Status == "Delivered")
            {
                existing.DefectQuantity = updated.DefectQuantity;
            }

            _reorderManagement.updateReorderRequest(existing); // overload this to accept full model

            TempData["SuccessMessage"] = "Reorder details updated.";
            return RedirectToAction("Reorder");
        }

        [HttpPost("cancel")]
        [ValidateAntiForgeryToken]
        public IActionResult CancelReorder(int reorderId)
        {
            var reorder = _reorderManagement.getReorderRequestDetails(reorderId);
            if (reorder == null || reorder.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Only pending orders can be cancelled.";
                return RedirectToAction("Reorder");
            }

            _reorderManagement.cancelReorderRequest(reorderId);
            TempData["SuccessMessage"] = "Reorder request has been cancelled.";
            return RedirectToAction("Reorder");
        }



    }
}
