using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

// For all other pages
namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;

        public CustomerPageController(ILogger<CustomerPageController> logger)
        {
            _logger = logger;
        }

        public IActionResult CustomerDetails()
        {
            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml"); 
        }
    }
}