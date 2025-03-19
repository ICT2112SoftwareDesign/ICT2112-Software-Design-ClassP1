using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using System.Text.Json;

namespace CleanBrilliantCompany.Controllers
{
    public class CustomerPageController : Controller
    {
        private readonly ILogger<CustomerPageController> _logger;
        private readonly CustomerManagement _customerManagement;
        private readonly SupportManagement _supportManagement;
        private readonly ChatbotService _chatbotService;

        public CustomerPageController(
            ILogger<CustomerPageController> logger, 
            CustomerManagement customerManagement, 
            SupportManagement supportManagement,
            ChatbotService chatbotService) 
        {
            _logger = logger;
            _customerManagement = customerManagement;
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
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
    }
}
