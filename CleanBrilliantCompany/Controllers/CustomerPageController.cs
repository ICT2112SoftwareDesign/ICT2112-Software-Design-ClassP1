using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;

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
            string loggedInEmail = HttpContext.Session.GetString("LoggedInUserEmail");
            var applicationController = new ApplicationController(_customerManagement, HttpContext.RequestServices.GetService<IHttpContextAccessor>());

            // Retrieve Customer Details via Session
            var customerDetails = applicationController.GetCustomerSession();

            if (customerDetails != null)
            {
                ViewBag.CustomerId = customerDetails.GetSession<int>("customerId");
                ViewBag.Email = loggedInEmail;
                ViewBag.Username = customerDetails.GetSession<string>("username");
                ViewBag.CustomerAddress = customerDetails.GetSession<string>("customerAddress");
            }
            else
            {
                ViewBag.Message = "No customer details available.";
            }

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }
    }
}
