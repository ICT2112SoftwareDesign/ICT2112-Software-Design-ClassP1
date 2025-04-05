using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers.Reorder
{
    [Route("staff/reorder")]
    public class ReorderRequestController : ApplicationController
    {
        private readonly IReorderQuery _reorderManagement;
        

        public ReorderRequestController(IReorderQuery reorderManagement, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _reorderManagement = reorderManagement;
            
        }

        [HttpGet("")]
        public IActionResult Reorder()
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Reorder Requests page.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var reorders = _reorderManagement.displayListOfReorders();
            return View("~/Views/Reorder/ListOfReorders.cshtml", reorders);
        }

        [HttpGet("details/{id}")]
        public IActionResult ReorderDetails(int id)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Support Tickets page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            var reorder = _reorderManagement.getReorderRequestDetails(id);

            if (reorder == null)
            {
                return NotFound("Reorder Request record not found.");
            }

            return View("~/Views/Reorder/ReorderDetails.cshtml", reorder);
        }

        [HttpGet("new")]
        public IActionResult CreateReorderForm()
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Support Tickets page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            IProduct productService = new ProductManagement();
            IManufacturer manufacturerService = new ProductManufacturerManagement(); 
             
            var products = productService.getAllProducts();
            var manufacturers = manufacturerService.getAllManufacturers();
            
            ViewBag.Products = products;
            ViewBag.Manufacturers = manufacturers;
            
            return View("~/Views/Reorder/ReorderForm.cshtml", new ReorderRequest_RDM());
        }

        [HttpPost("submitreorderform")]
        public IActionResult SubmitReorderForm(ReorderRequest_RDM reorder)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Support Tickets page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            if (!ModelState.IsValid)
            {
                return View("~/Views/Reorder/ReorderForm.cshtml", reorder);
            }
            
            // Set default values
            reorder.ExpectedDeliveryDate = null;
            reorder.Status = "Pending";
            
            // Ensure DefectQuantity is null for all products
            if (reorder.Products != null)
            {
                foreach (var product in reorder.Products)
                {
                    product.DefectQuantity = null;
                }
            }
            
            _reorderManagement.createReorderRequest(reorder);
            
            TempData["SuccessMessage"] = "Reorder request created successfully.";
            return RedirectToAction("Reorder");
        }

        [HttpPost("cancel_reorder/{reorderId}")]
        [ValidateAntiForgeryToken]
        public IActionResult CancelReorder(int reorderId)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Support Tickets page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            var reorder = _reorderManagement.getReorderRequestDetails(reorderId);
            if (reorder == null)
            {
                TempData["ErrorMessage"] = "Reorder request not found.";
                return RedirectToAction("Reorder");
            }

            // Delete the reorder and associated products
            _reorderManagement.cancelReorderRequest(reorderId);

            TempData["SuccessMessage"] = "Reorder request and products deleted successfully.";
            return RedirectToAction("Reorder");
        }

[HttpPost("update/{id}")]
[ValidateAntiForgeryToken]
public IActionResult UpdateReorderDetails(int id, ReorderRequest_RDM reorder)
{
    int? staffId = GetLoggedInStaffId();
    if (staffId == null)
    {
        TempData["Message"] = "You must log in to access the Support Tickets page.";
        return RedirectToAction("Login", "StaffLogin");
    }
    
    // Ensure reorder.Products is initialized
    if (reorder.Products == null)
    {
        reorder.Products = new List<ReorderRequest_Products>();
    }
    
    // Log the incoming data
    Console.WriteLine($"DEBUG CONTROLLER: Reorder ID: {reorder.ReorderId}, Manufacturer ID: {reorder.ManufacturerId}");
    Console.WriteLine($"DEBUG CONTROLLER: Number of products: {reorder.Products.Count}");
    
    foreach (var product in reorder.Products)
    {
        Console.WriteLine($"DEBUG CONTROLLER: Product - ReorderProductId: {product.ReorderProductId}, ProductId: {product.ProductId}, Quantity: {product.Quantity}");
    }

    if (!ModelState.IsValid)
    {
        // Repopulate ViewBag data for the view
        IProduct productService = new ProductManagement();
        IManufacturer manufacturerService = new ProductManufacturerManagement();

        ViewBag.Products = productService.getAllProducts();
        ViewBag.Manufacturers = manufacturerService.getAllManufacturers();
        
        return View("~/Views/Reorder/UpdateReorderDetails.cshtml", reorder);
    }

    try
    {
        // If we don't have any products in the form submission, fetch the existing ones
        if (reorder.Products.Count == 0)
        {
            var existingReorder = _reorderManagement.getReorderRequestDetails(id);
            if (existingReorder != null && existingReorder.Products != null)
            {
                reorder.Products = existingReorder.Products;
                Console.WriteLine($"DEBUG CONTROLLER: Loaded {reorder.Products.Count} existing products");
            }
        }

        // Call your service to update the reorder request
        _reorderManagement.updateReorderRequest(reorder);

        // Set a success message
        TempData["SuccessMessage"] = "Reorder request updated successfully.";

        // Redirect to ReorderDetails page after successful update
        return RedirectToAction("ReorderDetails", new { id = reorder.ReorderId });
    }
    catch (Exception ex)
    {
        // Log the exception
        Console.WriteLine($"DEBUG CONTROLLER: Exception in UpdateReorderDetails: {ex.Message}");
        TempData["ErrorMessage"] = $"Error updating reorder request: {ex.Message}";
        
        // Repopulate ViewBag data for the view
        IProduct productService = new ProductManagement();
        IManufacturer manufacturerService = new ProductManufacturerManagement();

        ViewBag.Products = productService.getAllProducts();
        ViewBag.Manufacturers = manufacturerService.getAllManufacturers();
        
        return View("~/Views/Reorder/UpdateReorderDetails.cshtml", reorder);
    }
}

        [HttpPost("delete_product")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int reorder_product_Id, int reorderId)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Support Tickets page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            try
            {
                _reorderManagement.deleteProductFromReorder(reorder_product_Id);

                TempData["SuccessMessage"] = "Product deleted successfully.";

                return RedirectToAction("ReorderDetails", new { id = reorderId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error deleting product: {ex.Message}";
                return RedirectToAction("ReorderDetails", new { id = reorderId });
            }
        }

        // GET: /Reorder/edit/{id}
        [HttpGet("edit/{id}")]
        public IActionResult EditReorderDetails(int id)
        {
            int? staffId = GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Support Tickets page.";
                return RedirectToAction("Login", "StaffLogin");
            }
            
            var reorder = _reorderManagement.getReorderRequestDetails(id);
            if (reorder == null)
            {
                return NotFound("Reorder Request record not found.");
            }

            // Fetch products and manufacturers for the edit view
            IProduct productService = new ProductManagement();
            IManufacturer manufacturerService = new ProductManufacturerManagement();

            ViewBag.Products = productService.getAllProducts();
            ViewBag.Manufacturers = manufacturerService.getAllManufacturers();

            return View("~/Views/Reorder/UpdateReorderDetails.cshtml", reorder);
        }



    }
}