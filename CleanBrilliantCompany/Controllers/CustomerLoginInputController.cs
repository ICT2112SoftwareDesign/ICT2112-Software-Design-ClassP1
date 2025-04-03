using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerLoginInputController : Controller
    {
        private readonly ILogger<CustomerLoginInputController> _logger;
        private readonly CustomerManagement _customerManagement;

        public CustomerLoginInputController(ILogger<CustomerLoginInputController> logger, CustomerManagement customerManagement)
        {
            _logger = logger;
            _customerManagement = customerManagement;
        }

        // INPUT CONTROLLER METHODS (Login)
        [HttpPost]
        public IActionResult LoginCustomer(string email, string password)
        {
            // Calls Customer Management Control Class 
            bool isAuthenticated = _customerManagement.authenticateCustomer(email, password);
            int loggedInCustomerId = _customerManagement.getIdByEmail(email);

            if (isAuthenticated && loggedInCustomerId > 0)
            {
                // After user logs in stores customer id in session
                HttpContext.Session.SetInt32("LoggedInUserId", loggedInCustomerId);
                
                return RedirectToAction("viewProducts", "ProductInput");
            }
            else
            {
                // Show an error message
                TempData["Message"] = "Invalid email or password.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }
        }
    }
}