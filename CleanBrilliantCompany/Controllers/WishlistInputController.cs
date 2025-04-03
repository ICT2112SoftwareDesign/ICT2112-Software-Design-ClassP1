using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class WishlistInputController : ApplicationController
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly SupportManagement _supportManagement;
        private readonly IChatbot _chatbotService;
        private readonly OrderManagement _orderManagement;
        private readonly CartManagement _cartManagement;
        private readonly IProduct _productService;

        private readonly ReviewManagement _reviewManagement;

        private readonly IShippingAgent _shippingAgent;
        private readonly IWishlistManagement _wishlistManagement;


        public WishlistInputController(
            ILogger<CustomerPageController> logger,
            CustomerManagement customerManagement,
            SupportManagement supportManagement,
            IChatbot chatbotService,
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IProduct productService,
            IShippingAgent shippingAgent,
            ReviewManagement reviewManagement,
            IWishlistManagement wishlistManagement, 
            IHttpContextAccessor httpContextAccessor
            ) : base(customerManagement, httpContextAccessor)
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
            _cartManagement = cartManagement;
            _productService = productService;
            _shippingAgent = shippingAgent;
            _reviewManagement = reviewManagement;
            _wishlistManagement = wishlistManagement;
        } 

        // View return is done here
        // Customer Pages
        public IActionResult CustomerDetails()
        {
            ViewBag.CustomerId = TempData["CustomerId"];
            ViewBag.Email = TempData["Email"];
            ViewBag.Password = TempData["Password"];
            ViewBag.Username = TempData["Username"];
            ViewBag.CustomerAddress = TempData["CustomerAddress"];
            ViewBag.Message = TempData["Message"];

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }        

      
        // part of ProductInputController
        [HttpPost]
        public IActionResult AddToWishlist(int productId)
        {
            // Retrieve customer ID from the session
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }


            // Call the wishlist management service
            var success = _wishlistManagement.addToWishlist(customerId.Value, productId);

            if (success)
            {
                TempData["Success"] = "Product added to wishlist successfully!";
            }
            else
            {
                TempData["Info"] = "Product is already in your wishlist.";
            }

            return RedirectToAction("viewProducts", "ProductInput");
        }

        // part of ProductInputController
        public IActionResult viewWishlist()
        {
            // Retrieve customer ID from the session
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }


            // Get customer details to retrieve the name
            var customer = _customerManagement.getCustomer(customerId.Value);
            var customerDetails =  ((ISession)this).GetCustomerSession();
            
            string customerName = customerDetails?.getSession<string>("username") ?? "My";
            // Get wishlist items from wishlist management service
            var productIds = _wishlistManagement.viewWishlist(customerId.Value);

            // Get detailed product information for each wishlist item
            var wishlistProducts = new Dictionary<int, Dictionary<string, object>>();

            foreach (var productId in productIds)
            {
                var product = _productService.getProductDetails(productId);
                if (product != null)
                {
                    wishlistProducts.Add(productId, product.GetProductDetails());
                }
            }
            // Pass the customer name to the view
            ViewBag.CustomerName = customerName;
            return View("~/Views/Wishlist/WishlistIndex.cshtml", wishlistProducts);
        }

        [HttpPost]
        public IActionResult removeFromWishlist(int productId)
        {
            // Retrieve customer ID from the session
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Call the wishlist management service
            var success = _wishlistManagement.removeFromWishlist(customerId.Value, productId);

            if (success)
            {
                TempData["Success"] = "Product removed from wishlist successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to remove product from wishlist.";
            }

            return RedirectToAction("viewWishlist");
        }

    }
}