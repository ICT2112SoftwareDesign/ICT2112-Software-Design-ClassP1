using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Text.Json;

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
            string chatHistoryJson = HttpContext.Session.GetString("ChatHistory");

            // Check if JSON exists and is not empty
            List<Dictionary<string, string>> chatHistory = !string.IsNullOrWhiteSpace(chatHistoryJson) 
                ? JsonSerializer.Deserialize<List<Dictionary<string, string>>>(chatHistoryJson) 
                : new List<Dictionary<string, string>>();

            // Pass the chat history to the ViewBag
            ViewBag.ChatHistory = chatHistory;
            
            return View("~/Views/Support/Chatbot.cshtml");
        }

        // Method to send a user message and get bot response
        [HttpPost]
        public IActionResult provideAutomatedResponse(String query)
        {
            if (string.IsNullOrEmpty(query)) return RedirectToAction("startChatSession");

            string botResponse = _supportManagement.handleCustomerChatbotQuery(query);

            string chatHistoryJson = HttpContext.Session.GetString("ChatHistory");

            // Check if JSON is null or empty before deserialization
            List<Dictionary<string, string>> chatHistory = !string.IsNullOrWhiteSpace(chatHistoryJson) 
                ? JsonSerializer.Deserialize<List<Dictionary<string, string>>>(chatHistoryJson) 
                : new List<Dictionary<string, string>>();

            // Create separate dictionaries for user and bot messages
            var userMessage = new Dictionary<string, string> { { "user", query } };
            var botMessage = new Dictionary<string, string> { { "bot", botResponse } };

            // Add both messages to chat history
            chatHistory.Add(userMessage);
            chatHistory.Add(botMessage);

            // Print for debugging
            Console.WriteLine("Chat History: " + JsonSerializer.Serialize(chatHistory));

            // Store updated chat history back in session
            HttpContext.Session.SetString("ChatHistory", JsonSerializer.Serialize(chatHistory));


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
    }
}