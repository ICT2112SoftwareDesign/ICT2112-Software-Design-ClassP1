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

        [HttpGet("")]
        public IActionResult Index()
        {
            int? staffId = base.GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Staff Dashboard.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var staffDetails = _staffManagement.GetStaffDetails((int)staffId);
            ViewBag.StaffName = staffDetails?.Username ?? "Unknown";
            return View("~/Views/StaffPage/Index.cshtml");
        }
    }
}
