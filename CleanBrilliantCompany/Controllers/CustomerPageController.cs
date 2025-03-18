using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Control;


namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly CartManagement _cartManagement;

        public CustomerPageController(ILogger<CustomerPageController> logger, CustomerManagement customerManagement)
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _cartManagement = cartManagement;
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
        public IActionResult ToPay()
        {
            var cartItems = _cartManagement.ViewCart().Select(item => new CartItem
            {
                ProductImage = _cartManagement.GetProductImage(item.Key),
                ProductName = _cartManagement.GetProductName(item.Key),
                Quantity = item.Value,
                Price = _cartManagement.GetProductPrice(item.Key)
            }).ToList();

            return View(cartItems);
        }
    }
}