using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerRegisterInputController : Controller
    {
        private readonly ILogger<CustomerRegisterInputController> _logger;
        private readonly CustomerManagement _customerManagement;

        public CustomerRegisterInputController(ILogger<CustomerRegisterInputController> logger, CustomerManagement customerManagement)
        {
            _logger = logger;
            _customerManagement = customerManagement;
        }

        // Input Controller methods (Register)

        [HttpPost]
        public IActionResult RegisterNewCustomer(string username, string password, string confirmPassword, string email)
        {
            // Check if password input fields match
            if (password != confirmPassword)
            {
                TempData["Message"] = "Passwords do not match.";
                return RedirectToAction("RegisterCustomer", "BeforeLoginPage");
            }
            
            bool isRegistered = _customerManagement.createAccount(username, password, email);
            if (isRegistered)
            {
                return RedirectToAction("Login", "BeforeLoginPage");
            }
            else
            {
                TempData["Message"] = "Registration failed. Email/Username already be in use.";
                return RedirectToAction("RegisterCustomer", "BeforeLoginPage");
            }
        }
    }
}