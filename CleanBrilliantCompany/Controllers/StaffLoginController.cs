using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class StaffLoginController : ApplicationController
    {
        private readonly ILogger<StaffLoginController> _logger;
        private readonly StaffManagement _staffManagement;

        public StaffLoginController(ILogger<StaffLoginController> logger, StaffManagement staffManagement, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _logger = logger;
            _staffManagement = staffManagement;
        }

        // Serves Staff Login Page
        public IActionResult Login()
        {
            return View("~/Views/StaffLogin/Login.cshtml");
        }

        [HttpPost]
        public IActionResult AuthenticateStaff(string email, string password)
        {
            bool isAuthenticated = _staffManagement.AuthenticateStaff(email, password);
            int loggedInStaffId = _staffManagement.GetIdByEmail(email);

            if (isAuthenticated && loggedInStaffId > 0)
            {
                HttpContext.Session.SetInt32("LoggedInStaffId", loggedInStaffId);
                return RedirectToAction("Index", "StaffDashboard");
            }

            ViewBag.Message = "Invalid email or password.";
            return View("~/Views/StaffLogin/Login.cshtml");
        }

        public IActionResult StaffLogout()
        {
            base.Logout();
            return RedirectToAction("Login");
        }
    }
}
