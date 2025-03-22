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

        private readonly IShippingAgents _shippingAgents;


        public CustomerPageController(
            ILogger<CustomerPageController> logger, 
            CustomerManagement customerManagement, 
            SupportManagement supportManagement,
            ChatbotService chatbotService,
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IProduct productService,
            IShippingAgents shippingAgents) 
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
            _cartManagement = cartManagement;
            _productService = productService;
            _shippingAgents = shippingAgents;
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

        public IActionResult GetAllProducts(string query = "", string filters = "All", string sortOrder = "asc")
        {
            List<Product> products = _productService.getAllProducts();
            
            // Apply search filter
            if (!string.IsNullOrEmpty(query))
            {
                products = products.Where(p => p.GetProductDetails()["ProductName"].ToString().Contains(query, System.StringComparison.OrdinalIgnoreCase)).ToList();
            }
            
            // Apply category filter
            if (filters != "All")
            {
                products = products.Where(p => p.GetProductDetails()["Category"].ToString() == filters).ToList();
            }
            
            // Apply sorting
            products = sortOrder == "asc" 
                ? products.OrderBy(p => float.Parse(p.GetProductDetails()["CostPrice"].ToString())).ToList() 
                : products.OrderByDescending(p => float.Parse(p.GetProductDetails()["CostPrice"].ToString())).ToList();
            
            // Convert to a list of dictionaries
            var productDetails = products.Select(product => product.GetProductDetails()).ToList();
            
            return View("~/Views/Products/Index.cshtml", productDetails);
        }

        public List<Product> FilterProducts(List<string> categories)
        {
            var allProducts = _productService.getAllProducts();
            return allProducts.FindAll(p => categories.Contains(p.GetProductDetails()["Category"].ToString()));
        }
        


        // CART INPUT CONTROLLER METHODS

        [HttpPost]
        public IActionResult addToCart(int productId, int quantity)
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

            var success = _cartManagement.addToCart(customerID.Value, productId, quantity);
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
        public IActionResult updateQuantity(int productId, int quantity)
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

            var success = _cartManagement.updateQuantity(customerID.Value, productId, quantity);
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
        public IActionResult removeFromCart(int productId)
        {
            // Retrieve customer ID from the session
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Call the RemoveFromCart method in CartManagement
            var success = _cartManagement.removeFromCart(customerID.Value, productId);
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
        

       public IActionResult viewCart()
        {
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Retrieve the cart data
            var cartData = _cartManagement.viewCart(customerID.Value);

            // Retrieve product details for the cart
            var products = _cartManagement.getCartProductDetails(cartData);

            // Calculate the total cost of the cart
            var cartTotal = _cartManagement.calculateCartTotal(cartData, products);

            // Pass data to the view
            ViewBag.Products = products;
            ViewBag.Total = cartTotal;

            return View("~/Views/Cart/Cart.cshtml", cartData);
        }




        // ORDER INPUT CONTROLLER METHODS

        // Display checkout Page (After pressing Checkout button)
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

            // Retrieve customer details using CustomerManagement
            var customer = _customerManagement.getCustomer(customerID.Value);
            string customerAddress = customer?.GetSession<string>("customerAddress") ?? string.Empty;

            // Load the cart for the customer
            var cart = _cartManagement.viewCart(customerID.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            // Load product details into ViewBag
            var products = _cartManagement.getCartProductDetails(cart);

            // Fetch available shipping options
            var serviceTypes = _shippingAgents.getServiceTypes();
            var shippingMethods = _shippingAgents.getShippingMethods();
            var defaultServiceType = serviceTypes.FirstOrDefault() ?? "1 Day"; // Use the first available service type
            var shippingAgents = _shippingAgents.getShippingAgentList(Enum.TryParse<Service>(defaultServiceType, out var serviceEnum) ? serviceEnum : Service.OneDay);
            var defaultShippingAgent = shippingAgents.FirstOrDefault() ?? "DHL"; // Use the first available shipping agent
            var defaultShippingType = shippingMethods.FirstOrDefault() ?? "Air"; // Use the first available shipping method

            // Calculate the cart total
            decimal cartTotal = _cartManagement.calculateCartTotal(cart, products);

            // Calculate the shipping fee based on the default service type
            decimal shippingFee = _orderManagement.calculateShippingFee(defaultServiceType);

            // Pass data to the view
            ViewBag.Products = products; // Product details (e.g., name, price)
            ViewBag.Cart = cart;         // Cart items (product ID and quantity)
            ViewBag.CartTotal = cartTotal;
            ViewBag.CustomerAddress = customerAddress; // Pass the customer's address (empty if missing)
            ViewBag.ServiceTypes = serviceTypes;       // Available service types
            ViewBag.ShippingMethods = shippingMethods; // Available shipping methods
            ViewBag.ShippingAgents = shippingAgents;   // Available shipping agents
            ViewBag.SelectedServiceType = defaultServiceType; // Dynamically determined default value
            ViewBag.SelectedShippingType = defaultShippingType; // Dynamically determined default value
            ViewBag.SelectedShippingAgent = defaultShippingAgent; // Dynamically determined default value
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = cartTotal + shippingFee;

            return View("~/Views/Order/Checkout.cshtml");
        }

        // Process the checkout form (Update shipping details)
        [HttpPost]
        public IActionResult Checkout(string deliveryAddress, string serviceType, string shippingType, string shippingAgent)
        {
            int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerID == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var cart = _cartManagement.viewCart(customerID.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            // Save the address if provided
            if (!string.IsNullOrEmpty(deliveryAddress))
            {
                var customer = _customerManagement.getCustomer(customerID.Value);
                customer.SetSession("customerAddress", deliveryAddress);
                _customerManagement.updateCustomerDetails(customer.GetSession<string>("username"), customer.GetSession<string>("email"), deliveryAddress);
            }

            // Calculate the shipping fee
            decimal shippingFee;
            try
            {
                shippingFee = _orderManagement.calculateShippingFee(serviceType);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Checkout");
            }

            // Fetch product details for the cart
            var products = _cartManagement.getCartProductDetails(cart);

            // Calculate the cart total
            decimal cartTotal = cart.Sum(item => Convert.ToDecimal(products[item.Key]["CostPrice"]) * item.Value);
            
            // Fetch available shipping options
            var selectedServiceEnum = Enum.TryParse<Service>(serviceType, out var serviceEnum) ? serviceEnum : Service.OneDay;
            var shippingAgents = _shippingAgents.getShippingAgentList(selectedServiceEnum);

            // Pass updated values back to the view
            ViewBag.Products = products;
            ViewBag.Cart = cart;
            ViewBag.CartTotal = cartTotal;
            ViewBag.CustomerAddress = deliveryAddress;
            ViewBag.ServiceTypes = _shippingAgents.getServiceTypes();
            ViewBag.ShippingMethods = _shippingAgents.getShippingMethods();
            ViewBag.ShippingAgents = shippingAgents;
            ViewBag.SelectedServiceType = serviceType;
            ViewBag.SelectedShippingType = shippingType;
            ViewBag.SelectedShippingAgent = shippingAgent;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = cartTotal + shippingFee;

            return View("~/Views/Order/Checkout.cshtml");
        }   


        [HttpPost]
        public IActionResult placeOrder(string deliveryAddress, string serviceType, string shippingType, string shippingAgent)
        {
            // Retrieve customer ID from the session
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("login", "BeforeLoginPage");
            }

            // Retrieve the cart from the database
            var cart = _cartManagement.viewCart(customerId.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("getAllProducts", "CustomerPage");
            }

            // Fetch product details for the cart
            var products = _cartManagement.getCartProductDetails(cart);

            // Calculate the cart total
            decimal cartTotal = _cartManagement.calculateCartTotal(cart, products);

            // Calculate the shipping fee
            decimal shippingFee;
            try
            {
                shippingFee = _orderManagement.calculateShippingFee(serviceType);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Checkout");
            }
            
            decimal finalTotal = cartTotal + shippingFee;

            // Pass order details to the Payment view
            ViewBag.CartTotal = cartTotal;
            ViewBag.ShippingFee = shippingFee;
            ViewBag.FinalTotal = finalTotal;
            ViewBag.OrderDetails = new Dictionary<string, string>
            {
                { "DeliveryAddress", deliveryAddress },
                { "serviceType", serviceType },
                { "shippingType", shippingType },
                { "shippingAgent", shippingAgent }
            };

            // Pass the cart to the Payment view
            ViewBag.Cart = cart;

            return View("~/Views/Order/Payment.cshtml");
        }

       [HttpPost]
        public IActionResult processPayment(string deliveryAddress, string serviceType, string shippingType, string shippingAgent, 
                                            string cardName, string cardNumber, string expiryDate, string cvv)
        {
            // Retrieve customer ID from the session
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("login", "BeforeLoginPage");
            }

            // Retrieve the cart from the database
            var cart = _cartManagement.viewCart(customerId.Value);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("getAllProducts", "CustomerPage");
            }

            // Validate payment details (mocked for now)
            if (string.IsNullOrWhiteSpace(cardName) || string.IsNullOrWhiteSpace(cardNumber) || 
                string.IsNullOrWhiteSpace(expiryDate) || string.IsNullOrWhiteSpace(cvv))
            {
                TempData["Error"] = "Please enter all payment details.";
                return RedirectToAction("placeOrder", new { deliveryAddress, serviceType, shippingType, shippingAgent });
            }

            // Create the order
            var orderId = _orderManagement.createOrder(
                customerId.Value,
                deliveryAddress,
                serviceType,
                shippingType,
                shippingAgent,
                cart
            );

            if (orderId > 0)
            {
                // Clear the cart after the order is successfully created
                var cartCleared = _cartManagement.clearCart(customerId.Value);
                if (!cartCleared)
                {
                    TempData["Error"] = "Order placed, but failed to clear the cart. Please contact support.";
                }

                TempData["Success"] = "Payment processed and order placed successfully!";
                return RedirectToAction("orderConfirmation", new { orderId });
            }
            else
            {
                TempData["Error"] = "Failed to process payment. Please try again.";
                return RedirectToAction("placeOrder", new { deliveryAddress, serviceType, shippingType, shippingAgent });
            }
        }

        [HttpGet]
        public IActionResult orderConfirmation(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View("~/Views/Order/OrderConfirmation.cshtml");
        }

        [HttpGet]
        public IActionResult ToShip()
        {
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Fetch orders with status "Pending"
            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                        .Where(o => o.Status == "Pending")
                                        .ToList();

            // Fetch product details for each order
            foreach (var order in orders)
            {
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);
            }

            return View("~/Views/Order/ToShip.cshtml", orders);
        }

        [HttpGet]
        public IActionResult ToReceive()
        {
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Fetch orders with status "Shipped"
            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                        .Where(o => o.Status == "Shipped")
                                        .ToList();

            // Fetch product details for each order
            foreach (var order in orders)
            {
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);
            }

            return View("~/Views/Order/ToReceive.cshtml", orders);
        }

        [HttpGet]
        public IActionResult Completed()
        {
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Fetch orders with status "Completed" or "Canceled"
            var orders = _orderManagement.getOrderHistory(customerId.Value)
                                        .Where(o => o.Status == "Completed" || o.Status == "Cancelled")
                                        .ToList();

            // Fetch product details for each order
            foreach (var order in orders)
            {
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);
            }

            return View("~/Views/Order/Completed.cshtml", orders);
        }
        [HttpPost]
        public IActionResult CancelOrder(int orderId)
        {
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var success = _orderManagement.cancelOrder(orderId, customerId.Value);
            if (success)
            {
                TempData["Success"] = "Order cancelled successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to cancel the order. Please try again.";
            }

            return RedirectToAction("ToShip");
        }

        [HttpPost]
        public IActionResult RequestRefund(int orderId)
        {
            int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            var success = _orderManagement.requestRefund(orderId, customerId.Value);
            if (success)
            {
                TempData["Success"] = "Refund request submitted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to submit refund request. Please try again.";
            }

            return RedirectToAction("Completed");
        }





























    }
}