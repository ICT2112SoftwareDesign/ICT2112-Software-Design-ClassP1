using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

// For all other pages
namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;

        public CustomerPageController(ILogger<CustomerPageController> logger, CustomerManagement customerManagement)
        {
            _logger = logger;
            _customerManagement = customerManagement;
        }

        public IActionResult CustomerDetails()
        {
            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }
    }
}