using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.StaffAuth;

namespace CleanBrilliantCompany.Controllers
{
    [Route("staff")] // Ensure a single consistent route
    public class StaffController : ApplicationController
    {
        private readonly ILogger<StaffController> _logger;
        private readonly StaffManagement _staffManagement;

        public StaffController(ILogger<StaffController> logger, StaffManagement staffManagement, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _logger = logger;
            _staffManagement = staffManagement;
        }

        // Helper method to check if the logged-in staff is ManagementStaff
        private bool IsManagementStaff()
        {
            string role = HttpContext.Session.GetString("StaffRole");
            return role == "management";
        }

        // Helper method to check if the logged-in staff is GeneralStaff
        private bool IsGeneralStaff()
        {
            string role = HttpContext.Session.GetString("StaffRole");
            return role == "general";
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            int? staffId = base.GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Staff Dashboard.";
                return RedirectToAction("Login", "StaffLogin");
            }

            // Get the role from StaffManagement (which uses StaffMapper)
            string role = _staffManagement.GetStaffRole((int)staffId);
            _logger.LogInformation("Fetched role for staffId " + staffId + ": " + role); // Log role

            // Set the role in the session
            HttpContext.Session.SetString("StaffRole", role);

            var staffDetails = _staffManagement.GetStaffDetails((int)staffId);
            ViewBag.StaffName = staffDetails?.Username ?? "Unknown";
            
            return RedirectToAction("Index", "Dashboard");
        }

        [HttpGet("feedback")]
        public IActionResult Feedback()
        {
            string role = HttpContext.Session.GetString("StaffRole");
            _logger.LogInformation("Staff role retrieved from session: " + role); // Log session role

            if (!IsGeneralStaff()) // Check if the logged-in staff is not GeneralStaff
            {
                _logger.LogWarning("Access denied for Feedback page. Staff role: " + role);
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index");
            }

            // Logic to load and display the feedback page
            return RedirectToAction("Index", "Feedback");
        }

        [HttpGet("managementfeedback")]
        public IActionResult ManagementFeedback()
        {
            if (!IsManagementStaff()) // Check if the logged-in staff is not ManagementStaff
            {
                TempData["ErrorMessage"] = "You do not have access to this page.";
                return RedirectToAction("Index");
            }

            // Redirect to the correct controller and action for manager feedback
            return RedirectToAction("Index", "ManagerFeedback");
        }
    }
}
