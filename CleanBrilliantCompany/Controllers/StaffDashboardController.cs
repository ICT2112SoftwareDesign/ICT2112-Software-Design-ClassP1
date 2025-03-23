using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class StaffDashboardController : ApplicationController
    {
        private readonly ILogger<StaffDashboardController> _logger;
        private readonly StaffManagement _staffManagement;

        public StaffDashboardController(ILogger<StaffDashboardController> logger, StaffManagement staffManagement, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _logger = logger;
            _staffManagement = staffManagement;
        }

        public IActionResult Index()
        {
            int? staffId = base.GetLoggedInStaffId();
            if (staffId == null)
            {
                TempData["Message"] = "You must log in to access the Staff Dashboard.";
                return RedirectToAction("Login", "StaffLogin");
            }

            var staffDetails = _staffManagement.GetStaff((int)staffId);
            ViewBag.StaffName = staffDetails?.Username ?? "Unknown";
            return View("~/Views/StaffPage/Index.cshtml");
        }
    }
}
