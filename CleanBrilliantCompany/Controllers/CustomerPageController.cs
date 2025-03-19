using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly SupportManagement _supportManagement;
        private readonly ChatbotService _chatbotService;
        private readonly OrderManagement _orderManagement;
        private readonly CartManagement _cartManagement;
        private readonly IProduct _productService;


        public CustomerPageController(
            ILogger<CustomerPageController> logger, 
            CustomerManagement customerManagement, 
            SupportManagement supportManagement,
            ChatbotService chatbotService,
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IProduct productService) 
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
            _cartManagement = cartManagement;
            _productService = productService;
        }

        public IActionResult CustomerDetails()
        {
            int loggedInId = HttpContext.Session.GetInt32("LoggedInUserId") ?? -1;
            var applicationController = new ApplicationController(_customerManagement, HttpContext.RequestServices.GetService<IHttpContextAccessor>());

            // Retrieve Customer Details via Session
            var customerDetails = applicationController.GetCustomerSession();

            if (customerDetails != null)
            {
                ViewBag.CustomerId = loggedInId;
                ViewBag.Email = customerDetails.GetSession<string>("email");
                ViewBag.Password = customerDetails.GetSession<string>("password");
                ViewBag.Username = customerDetails.GetSession<string>("username");
                ViewBag.CustomerAddress = customerDetails.GetSession<string>("customerAddress");
            }
            else
            {
                ViewBag.Message = "No customer details available.";
            }

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }
        [HttpPost]
        public IActionResult updateCustomerDetails(string username, string email, string address)
        {
            int loggedInId = HttpContext.Session.GetInt32("LoggedInUserId") ?? -1;
            var applicationController = new ApplicationController(_customerManagement, HttpContext.RequestServices.GetService<IHttpContextAccessor>());
            var customerDetails = applicationController.GetCustomerSession();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                ViewBag.Message = "Username and/or email cannot be empty";
                CustomerDetails();
                return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
            }

            bool isEmailChanged = email != customerDetails.GetSession<string>("email");
            bool isUsernameChanged = username != customerDetails.GetSession<string>("username");

            if (isEmailChanged || isUsernameChanged)
            {
                var validationMessage = validateChanges(loggedInId, username, email, isEmailChanged, isUsernameChanged);
                if (validationMessage != null)
                {
                    ViewBag.Message = validationMessage;
                    CustomerDetails();
                    return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
                }
            }
            bool updateSuccessful = _customerManagement.updateCustomerDetails(username, email, address);
            if (updateSuccessful)
            {
                CustomerDetails();
                return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
            }

            ViewBag.Message = "Failed to update details.";
            CustomerDetails();
            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }

        // Check if username or email exists
        private string validateChanges(int loggedInId, string username, string email, bool isEmailChanged, bool isUsernameChanged)
        {
            if (isEmailChanged && _customerManagement.customerEmailExists(loggedInId, email))
            {
                return "Email already exists.";
            }
            if (isUsernameChanged && _customerManagement.customerUsernameExists(loggedInId, username))
            {
                return "Username already exists.";
            }

            return null; 
        }

        [HttpPost]
        public IActionResult UpdatePassword(string password)
        {

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");

        }

        // INPUT CONTROLLER METHODS

        // HelpCenterInputController Methods

        // public IActionResult submitQuery(String query)
        // {
            
        // }

        public IActionResult viewFAQs(String query)
        {
            List<String> faqs = _supportManagement.FetchFAQs();
            ViewBag.FAQs = faqs;

            return View("~/Views/Support/FAQs.cshtml");
        }

        // public IActionResult trackTicket(Int32 ticketId)
        // {
            
        // }

        // public IActionResult escalateIssue(Int32 ticketId)
        // {
            
        // }

        // // ChatbotInputController Methods

         // Method to start chat session and return current chat history
        public IActionResult startChatSession()
        {
            var chatHistory = HttpContext.Session.GetString("ChatHistory") ?? "";
            ViewBag.ChatHistory = chatHistory;
            return View("~/Views/Support/Chatbot.cshtml");
        }

        // Method to send a user message and get bot response
        [HttpPost]
        public IActionResult provideAutomatedResponse(String query)
        {
            if (string.IsNullOrEmpty(query)) return RedirectToAction("startChatSession");

            string botResponse = _supportManagement.handleCustomerChatbotQuery(query);

            // Store the conversation history in session
            var chatHistory = HttpContext.Session.GetString("ChatHistory") ?? "";
            chatHistory += $"You: {query}\nBot: {botResponse}\n";
            HttpContext.Session.SetString("ChatHistory", chatHistory);

            return RedirectToAction("startChatSession");
        }

        // public IActionResult escalateToAgent(String query)
        // {
            
        // }

        public IActionResult GetAllProducts()
        {
            var products = _orderManagement.GetAllProducts();
            var productDetails = new List<Dictionary<string, object>>();
            foreach (var product in products)
            {
                productDetails.Add(product.GetProductDetails());
            }
            return View("~/Views/Products/Index.cshtml", productDetails);
        }
        


        // CART INPUT CONTROLLER METHODS

        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            // Retrieve customer ID from the session using the correct key
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction("GetAllProducts"); // Redirect back to the product page
            }

            var success = _cartManagement.AddToCart(customerID.Value, productId, quantity);
            if (success)
            {
                TempData["Success"] = "Product added to cart successfully!";
            }
            else
            {
                TempData["Error"] = "Failed to add product to cart.";
            }

            return RedirectToAction("GetAllProducts"); // Redirect back to the product page
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Quantity must be greater than zero.";
                return RedirectToAction("ViewCart");
            }

            var success = _cartManagement.UpdateQuantity(customerID.Value, productId, quantity);
            if (success)
            {
                TempData["Success"] = "Cart updated successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to update cart.";
            }

            return RedirectToAction("ViewCart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            // Retrieve customer ID from the session
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Call the RemoveFromCart method in CartManagement
            var success = _cartManagement.RemoveFromCart(customerID.Value, productId);
            if (success)
            {
                TempData["Success"] = "Product removed from cart successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to remove product from cart.";
            }

            return RedirectToAction("ViewCart"); // Redirect back to the cart page
        }
        

        public IActionResult ViewCart()
        {
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cartData = _cartManagement.ViewCart(customerID.Value);
            var products = new Dictionary<int, Dictionary<string, object>>();

        foreach (var item in cartData)
        {
            var product = _productService.GetProductDetails(item.Key);
            if (product != null)
            {
                var productDetails = product.GetProductDetails();
                productDetails["CostPrice"] = Convert.ToDecimal(productDetails["CostPrice"]); // Ensure CostPrice is decimal
                products[item.Key] = productDetails;
            }
        }

        ViewBag.Products = products;
        ViewBag.Total = cartData.Sum(item => Convert.ToDecimal(products[item.Key]["CostPrice"]) * item.Value);

            return View("~/Views/Cart/Cart.cshtml", cartData);
        }

        
        

        public IActionResult ProductDetail(int productId)
        {
            var product = _orderManagement.GetOneProduct(productId);

            if (product == null)
            {
                return Content("Product not found");
            }

            var productDetails = product.GetProductDetails();
            return View("~/Views/Products/ProductDetails.cshtml", productDetails);
        }



        // ORDER INPUT CONTROLLER METHODS

        [HttpGet]
        public IActionResult Checkout()
        {
            // Retrieve customer ID from the session
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Load the cart for the customer
            var cart = _cartManagement.ViewCart(customerID.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            // Load product details into ViewBag
            var products = new Dictionary<int, Dictionary<string, object>>();
            foreach (var item in cart)
            {
                var product = _productService.GetProductDetails(item.Key);
                if (product != null)
                {
                    products[item.Key] = product.GetProductDetails();
                }
            }

            ViewBag.Products = products; // Product details (e.g., name, price)
            ViewBag.Cart = cart;         // Cart items (product ID and quantity)
            ViewBag.CartTotal = cart.Sum(item => Convert.ToDecimal(products[item.Key]["CostPrice"]) * item.Value);

         return View("~/Views/Order/Checkout.cshtml");
        }
        [HttpPost]
        public IActionResult Checkout(string deliveryType, string deliveryAddress)
        {
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cart = _cartManagement.ViewCart(customerID.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            // Calculate the shipping fee
            var deliveryCosts = new Dictionary<string, decimal>
            {
                { "Next Day Delivery", 10.00m },
                { "Three Day Delivery", 5.00m },
                { "One Week Delivery", 0.00m }
            };

            if (!deliveryCosts.ContainsKey(deliveryType))
            {
                TempData["Error"] = "Invalid delivery type selected.";
                return RedirectToAction("Checkout");
            }

            decimal shippingFee = deliveryCosts[deliveryType];
            decimal cartTotal = cart.Sum(item =>
            {
                var product = _productService.GetProductDetails(item.Key);
                return Convert.ToDecimal(product.GetProductDetails()["CostPrice"]) * item.Value;
            });

            ViewBag.ShippingFee = shippingFee;
            ViewBag.CartTotal = cartTotal;
            ViewBag.FinalTotal = cartTotal + shippingFee;
            ViewBag.CustomerAddress = deliveryAddress;

            TempData["Success"] = "Address and delivery type confirmed.";
            return View("~/Views/Order/Checkout.cshtml");
        }
    }
}