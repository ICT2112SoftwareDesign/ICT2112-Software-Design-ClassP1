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
            return View(); // Serves Login Page
        }

        public IActionResult RegisterCustomer()
        {
            return View(); // Serves Registration Page
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
                // Redirect to a secure area or dashboard
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
        public IActionResult RegisterNewCustomer(string username, string password, string email)
        {
            // Placeholder logic for now
            ViewBag.Message = "Registration functionality will be implemented later.";
            return View("~/Views/BeforeLogin/Login.cshtml");
        }
    }
}