using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces.StaffAuth;

namespace CleanBrilliantCompany.Controllers
{
    public class StaffLoginController : ApplicationController
    {
        private readonly IStaffAuthentication _staffAuthentication;

        public StaffLoginController(IStaffAuthentication staffAuthentication, IHttpContextAccessor httpContextAccessor)
            : base(httpContextAccessor)
        {
            _staffAuthentication = staffAuthentication;
        }

        public IActionResult Login()
        {
            return View("~/Views/StaffLogin/Login.cshtml");
        }

        [HttpPost]
        public IActionResult AuthenticateStaff(string email, string password)
        {
            bool isAuthenticated = _staffAuthentication.Login(email, password);
            int loggedInStaffId = _staffAuthentication.GetIdByEmail(email);

            if (isAuthenticated && loggedInStaffId > 0)
            {
                // Store the staff ID and role in session
                HttpContext.Session.SetInt32("LoggedInStaffId", loggedInStaffId);

                // Retrieve the role from the database
                string role = _staffAuthentication.GetStaffRole(loggedInStaffId);
                HttpContext.Session.SetString("StaffRole", role);

                return RedirectToAction("Index", "Staff");
            }

            ViewBag.Message = "Invalid email or password.";
            return View("~/Views/StaffLogin/Login.cshtml");
        }

        public IActionResult StaffLogout()
        {
            _staffAuthentication.Logout(HttpContext);
            return RedirectToAction("Login");
        }
    }
}
