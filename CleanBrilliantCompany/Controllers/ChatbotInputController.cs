using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Text.Json;

namespace CleanBrilliantCompany.Controllers
{
    public class ChatbotInputController : ApplicationController 
    {
        private readonly SupportManagement _supportManagement;
        private readonly IChatbot _chatbotService;
        private readonly CustomerManagement _customerManagement;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ChatbotInputController(SupportManagement supportManagement, IChatbot chatbotService, CustomerManagement customerManagement, IHttpContextAccessor httpContextAccessor)
        : base(customerManagement, httpContextAccessor)
        {
            _supportManagement = supportManagement;
            _chatbotService = chatbotService;
        } 

        public IActionResult startChatSession()
        {
            string chatHistoryJson = HttpContext.Session.GetString("ChatHistory");

            // Check if JSON exists and is not empty
            List<Dictionary<string, string>> chatHistory = !string.IsNullOrWhiteSpace(chatHistoryJson)
                ? JsonSerializer.Deserialize<List<Dictionary<string, string>>>(chatHistoryJson)
                : new List<Dictionary<string, string>>();

            ViewBag.ChatHistory = chatHistory;

            return View("~/Views/Support/Chatbot.cshtml");
        }

        [HttpGet]
        public IActionResult provideAutomatedResponse(String query)
        {
            // Retrieve customer ID from the session using the correct key
            int? customerId = base.getLoggedInCustomerId();
            if (customerId == -1)
            {
                TempData["Error"] = "User not logged in.";
                return RedirectToAction("Login", "BeforeLoginPage");
            }

            if (string.IsNullOrEmpty(query)) return RedirectToAction("startChatSession");

            string botResponse = _supportManagement.handleQuery(customerId ?? -1, query);

            string chatHistoryJson = HttpContext.Session.GetString("ChatHistory");

            // Check if JSON is null or empty before deserialization
            List<Dictionary<string, string>> chatHistory = !string.IsNullOrWhiteSpace(chatHistoryJson)
                ? JsonSerializer.Deserialize<List<Dictionary<string, string>>>(chatHistoryJson)
                : new List<Dictionary<string, string>>();

            var userMessage = new Dictionary<string, string> { { "user", query } };
            var botMessage = new Dictionary<string, string> { { "bot", botResponse } };

            chatHistory.Add(userMessage);
            chatHistory.Add(botMessage);

            // Store updated chat history back in session
            HttpContext.Session.SetString("ChatHistory", JsonSerializer.Serialize(chatHistory));

            return RedirectToAction("startChatSession");
        }
    }
}