using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly CartManagement _cartManagement;
        private readonly SupportManagement _supportManagement;
        private readonly ChatbotService _chatbotService;
        private readonly OrderManagement _orderManagement;

        public CustomerPageController(
            ILogger<CustomerPageController> logger, 
            CustomerManagement customerManagement, 
            SupportManagement supportManagement,
            ChatbotService chatbotService,
            OrderManagement orderManagement) 
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _cartManagement = cartManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
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
    }
}