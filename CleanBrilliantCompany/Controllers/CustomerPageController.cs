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
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
            _orderManagement = orderManagement;
        }

        public IActionResult CustomerDetails()
        {
            int loggedInId = HttpContext.Session.GetInt32("LoggedInUserId") ?? -1;;
            var applicationController = new ApplicationController(_customerManagement, HttpContext.RequestServices.GetService<IHttpContextAccessor>());

            // Retrieve Customer Details via Session
            var customerDetails = applicationController.GetCustomerSession();

            if (customerDetails != null)
            {
                ViewBag.CustomerId = loggedInId;
                ViewBag.Email = customerDetails.GetSession<string>("email");
                ViewBag.Username = customerDetails.GetSession<string>("username");
                ViewBag.CustomerAddress = customerDetails.GetSession<string>("customerAddress");
            }
            else
            {
                ViewBag.Message = "No customer details available.";
            }

            return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        }

        // [HttpPost]
        // public IActionResult UpdateCustomerAddress(string fieldName, string fieldValue)
        // {
        //     if(fieldValue != null){
        //         if(fieldName == "email"){
                    
        //         }
        //         bool updateSuccessful = _customerManagement.UpdateCustomer(fieldName, fieldValue);
        //     }
            
        //     string loggedInEmail = HttpContext.Session.GetString("LoggedInUserEmail");
        //     var applicationController = new ApplicationController(_customerManagement, HttpContext.RequestServices.GetService<IHttpContextAccessor>());

        //     // Retrieve Customer Details via Session
        //     var customerDetails = applicationController.GetCustomerSession();

        //     if (customerDetails != null)
        //     {
        //         ViewBag.CustomerId = customerDetails.GetSession<int>("customerId");
        //         ViewBag.Email = loggedInEmail;
        //         ViewBag.Username = customerDetails.GetSession<string>("username");
        //         ViewBag.CustomerAddress = customerDetails.GetSession<string>("customerAddress");
        //     }
        //     else
        //     {
        //         ViewBag.Message = "No customer details available.";
        //     }

        //     return View("~/Views/CustomerPage/Profile/CustomerDetails.cshtml");
        // }

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
            return View("~/Views/TestProduct.cshtml", productDetails);
        }
    }
}
