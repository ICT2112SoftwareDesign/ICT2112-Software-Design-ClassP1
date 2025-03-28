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

        // Redirect to Login
        public IActionResult Login()
        {
            ViewBag.Message = TempData["Message"];
            return View("~/Views/BeforeLogin/Login.cshtml"); 
        }

        // Redirect to Register
        public IActionResult RegisterCustomer()
        {
            ViewBag.Message = TempData["Message"];
            return View("~/Views/BeforeLogin/RegisterCustomer.cshtml"); 
        }
    }
}