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

        private readonly ReviewManagement _reviewManagement; 

        private readonly IShippingAgents _shippingAgents;
        private readonly IWishlistManagement _wishlistManagement;


        public CustomerPageController(
            ILogger<CustomerPageController> logger, 
            CustomerManagement customerManagement, 
            SupportManagement supportManagement,
            ChatbotService chatbotService,
            OrderManagement orderManagement,
            CartManagement cartManagement,
            IProduct productService,
            IShippingAgents shippingAgents,
            ReviewManagement reviewManagement,
            IWishlistManagement wishlistManagement) 
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
            _cartManagement = cartManagement;
            _productService = productService;
            _shippingAgents = shippingAgents;
            _reviewManagement = reviewManagement;
            _wishlistManagement = wishlistManagement;
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

            if (!serviceTypes.Any() || !shippingMethods.Any())
            {
                TempData["Error"] = "No shipping options are available at the moment.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            var defaultServiceType = serviceTypes.First(); // Use the first available service type
            var shippingAgents = _shippingAgents.getShippingAgentList(Enum.TryParse<Service>(defaultServiceType, out var serviceEnum) ? serviceEnum : Service.OneDay);

            if (!shippingAgents.Any())
            {
                TempData["Error"] = "No shipping agents are available at the moment.";
                return RedirectToAction("GetAllProducts", "CustomerPage");
            }

            var defaultShippingAgent = shippingAgents.First(); // Use the first available shipping agent
            var defaultShippingType = shippingMethods.First(); // Use the first available shipping method

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
            if (!string.IsNullOrWhiteSpace(deliveryAddress))
            {
                // Optionally save the new address to the customer's profile
                var customer = _customerManagement.getCustomer(customerID.Value);
                customer.SetSession("customerAddress", deliveryAddress); // Save to session
            }
            else
            {
                TempData["Error"] = "Delivery address is required.";
                return RedirectToAction("Checkout");
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
            ViewBag.CustomerAddress = deliveryAddress; // Pass the updated address back to the view
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
                deliveryAddress, // Ensure this is passed correctly
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

            // Initialize the shippingDetails dictionary
            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            // Process each order
            foreach (var order in orders)
            {
                // Fetch product details
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);

                // Deserialize shipping details from OrderShipping
                if (!string.IsNullOrEmpty(order.OrderShipping))
                {
                    try
                    {
                        var details = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(order.OrderShipping);
                        if (details != null)
                        {
                            // Dynamically calculate the shipping fee
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.OrderID] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error deserializing OrderShipping for OrderID {order.OrderID}: {ex.Message}");
                    }
                }
            }

            // Pass orders and shipping details to the view
            ViewBag.ShippingDetails = shippingDetails;
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

            // Initialize the shippingDetails dictionary
            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            // Process each order
            foreach (var order in orders)
            {
                // Fetch product details
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);

                // Deserialize shipping details from OrderShipping
                if (!string.IsNullOrEmpty(order.OrderShipping))
                {
                    try
                    {
                        var details = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(order.OrderShipping);
                        if (details != null)
                        {
                            // Dynamically calculate the shipping fee
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.OrderID] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error deserializing OrderShipping for OrderID {order.OrderID}: {ex.Message}");
                    }
                }
            }

            // Pass orders and shipping details to the view
            ViewBag.ShippingDetails = shippingDetails;
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

            // Fetch all orders for the customer
            var orders = _orderManagement.getOrderHistory(customerId.Value);

            // Filter only Completed orders
            var completedOrders = orders.Where(o => o.Status == "Completed").ToList();

            // Initialize the shippingDetails dictionary
            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            // Process each completed order to fetch shipping details
            foreach (var order in completedOrders)
            {
                // Fetch product details
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);

                // Deserialize shipping details from OrderShipping
                if (!string.IsNullOrEmpty(order.OrderShipping))
                {
                    try
                    {
                        var details = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(order.OrderShipping);
                        if (details != null)
                        {
                            // Dynamically calculate the shipping fee
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.OrderID] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error deserializing OrderShipping for OrderID {order.OrderID}: {ex.Message}");
                    }
                }
            }

            // Pass the completed orders and shipping details to the view
            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/Completed.cshtml", completedOrders);
        }

        [HttpGet]
        public IActionResult Cancelled()
        {
           int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Fetch all orders for the customer
            var orders = _orderManagement.getOrderHistory(customerId.Value);

            // Filter only Completed orders
            var cancelledOrders = orders.Where(o => o.Status == "Cancelled").ToList();

            // Initialize the shippingDetails dictionary
            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            // Process each completed order to fetch shipping details
            foreach (var order in cancelledOrders)
            {
                // Fetch product details
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);

                // Deserialize shipping details from OrderShipping
                if (!string.IsNullOrEmpty(order.OrderShipping))
                {
                    try
                    {
                        var details = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(order.OrderShipping);
                        if (details != null)
                        {
                            // Dynamically calculate the shipping fee
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.OrderID] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error deserializing OrderShipping for OrderID {order.OrderID}: {ex.Message}");
                    }
                }
            }
            // Pass the cancelled orders and shipping details to the view
            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/Cancelled.cshtml", cancelledOrders);
        }

        [HttpGet]
        public IActionResult Refund()
        {
           int? customerId = HttpContext.Session.GetInt32("LoggedInUserId");
            if (customerId == null)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            // Fetch all orders for the customer
            var orders = _orderManagement.getOrderHistory(customerId.Value);

            // Filter orders with statuses "Refunded" or "Refund Requested"
            var refundOrders = orders.Where(o => o.Status == "Refunded" || o.Status == "RefundRequested").ToList();

            // Initialize the shippingDetails dictionary
            var shippingDetails = new Dictionary<int, Dictionary<string, string>>();

            // Process each completed order to fetch shipping details
            foreach (var order in refundOrders)
            {
                // Fetch product details
                order.OrderProductsDetails = _cartManagement.getCartProductDetails(order.OrderProducts);

                // Deserialize shipping details from OrderShipping
                if (!string.IsNullOrEmpty(order.OrderShipping))
                {
                    try
                    {
                        var details = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(order.OrderShipping);
                        if (details != null)
                        {
                            // Dynamically calculate the shipping fee
                            if (details.ContainsKey("ServiceType"))
                            {
                                details["ShippingFee"] = _orderManagement.calculateShippingFee(details["ServiceType"]).ToString("F2");
                            }

                            shippingDetails[order.OrderID] = details;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error deserializing OrderShipping for OrderID {order.OrderID}: {ex.Message}");
                    }
                }
            }
            // Pass the refund orders and shipping details to the view
            ViewBag.ShippingDetails = shippingDetails;
            return View("~/Views/Order/Refund.cshtml", refundOrders);
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


        //REVIEW INPUT CONTROLLER METHODS 

        [HttpPost]
        public IActionResult SubmitReivew (string reviewText, int rating, int productId)
        { 
            if(!_reviewManagement.WriteReview(reviewText, rating, productId))
            { 
                TempData["Error"] = "Failed to submit review. Make sure all fields are valid.";
            }
            else
            {
                TempData["Success"] = "Review submitted successfully!";
            }

            return RedirectToAction("GetAllProducts"); // gotta check where to go next. 
        }

        [HttpPost]
        public IActionResult EditReview(int reviewId, string reviewText, int rating)
        {
            if (!_reviewManagement.EditReview(reviewId, reviewText, rating))
            {
                TempData["Error"] = "Failed to edit review. Please try again.";
            }
            else
            {
                TempData["Success"] = "Review updated successfully!";
            }

            return RedirectToAction("GetAllProducts");
        }
        [HttpPost]
        public IActionResult RemoveReview(int reviewId)
        {
            if (!_reviewManagement.DeleteReview(reviewId))
            {
                TempData["Error"] = "Failed to delete review.";
            }
            else
            {
                TempData["Success"] = "Review deleted successfully!";
            }

            return RedirectToAction("GetAllProducts");
        }

        [HttpGet]
        public IActionResult ViewReviews()
        {
            var reviews = _reviewManagement.ViewReviews();
            return View("~/Views/Review/ReviewHTML.cshtml", reviews);
        }

    // part of ProductInputController
    [HttpPost]
    public IActionResult AddToWishlist(int productId)
    {
        // Retrieve customer ID from the session
        int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
        if (customerID == null)
        {
            TempData["Error"] = "User not logged in.";
            return RedirectToAction("Login", "BeforeLoginPage");
        }
        

        // Call the wishlist management service
        var success = _wishlistManagement.addToWishlist(customerID.Value, productId);
        
        if (success)
        {
            TempData["Success"] = "Product added to wishlist successfully!";
        }
        else
        {
            TempData["Info"] = "Product is already in your wishlist.";
        }

        return RedirectToAction("GetAllProducts");
    }

// part of ProductInputController
public IActionResult viewWishlist()
{
    // Retrieve customer ID from the session
    int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
    if (customerID == null)
    {
        TempData["Error"] = "User not logged in.";
        return RedirectToAction("Login", "BeforeLoginPage");
    }


    // Get customer details to retrieve the name
    var customer = _customerManagement.getCustomer(customerID.Value);
    string customerName = customer?.GetSession<string>("username") ?? "My";
    // Get wishlist items from wishlist management service
    var productIds = _wishlistManagement.viewWishlist(customerID.Value);
    
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
    int? customerID = HttpContext.Session.GetInt32("LoggedInUserId");
    if (customerID == null)
    {
        TempData["Error"] = "User not logged in.";
        return RedirectToAction("Login", "BeforeLoginPage");
    }

    // Call the wishlist management service
    var success = _wishlistManagement.removeFromWishlist(customerID.Value, productId);
    
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