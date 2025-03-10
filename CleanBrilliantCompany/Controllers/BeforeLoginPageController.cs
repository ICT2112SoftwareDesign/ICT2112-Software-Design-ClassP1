using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class BeforeLoginPageController : Controller
    {
        private readonly ILogger<BeforeLoginPageController> _logger;
        private readonly CustomerManagement _customerManagement;

        public BeforeLoginPageController(ILogger<BeforeLoginPageController> logger, CustomerManagement customerManagement)
        {
            _logger = logger;
            _customerManagement = customerManagement;
        }

        // PAGE CONTROLLER METHODS
        public IActionResult Login()
        {
            return View("~/Views/BeforeLogin/Login.cshtml"); // Serves Login Page
        }

        public IActionResult RegisterCustomer()
        {
            return View("~/Views/BeforeLogin/RegisterCustomer.cshtml"); // Serves Registration Page
        }

        // INPUT CONTROLLER METHODS
        
        [HttpPost]
        public IActionResult LoginCustomer(string email, string password)
        {
            //Console.WriteLine($"Received Email: {email}");
            //Console.WriteLine($"Received Password: {password}");
            bool isAuthenticated = _customerManagement.AuthenticateCustomer(email, password);
            if (isAuthenticated)
            {
                // After user logs in stores email in session
                HttpContext.Session.SetString("LoggedInUserEmail", email);
                
                return RedirectToAction("Index", "Home");
            }
            else
            {
                // Show an error message
                ViewBag.Message = "Invalid email or password.";
                return View("~/Views/BeforeLogin/Login.cshtml");
            }
        }

        [HttpPost]
        public IActionResult RegisterNewCustomer(string username, string password, string confirmPassword, string email)
        {
            if (password != confirmPassword)
            {
                ViewBag.Message = "Passwords do not match.";
                return View("~/Views/BeforeLogin/RegisterCustomer.cshtml");
            }
            
            bool isRegistered = _customerManagement.createAccount(username, password, email);
            if (isRegistered)
            {
                return View("~/Views/BeforeLogin/Login.cshtml");
            }
            else
            {
                ViewBag.Message = "Registration failed. Email already be in use.";
                return View("~/Views/BeforeLogin/RegisterCustomer.cshtml");
            }
        }
    }
}