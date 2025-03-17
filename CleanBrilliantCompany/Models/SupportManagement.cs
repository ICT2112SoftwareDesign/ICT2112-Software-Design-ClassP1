using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class SupportManagement : IChatbotQuery
    {
        private readonly ISupportTicket _ticketService;
        private readonly IChatbot _chatBotService;
        // private readonly IOrder _orderService;

        public SupportManagement(ChatbotService chatBotService)
        {
            _chatBotService = chatBotService;
        }

        public string handleCustomerChatbotQuery(String query)
        {
            String response = handleQuery(query);
            return response;
        }

        public string handleQuery(String query)
        {
            String response = _chatBotService.submitQuery(query);
            return response;
        }

        public List<String> FetchFAQs()
        {
            return new List<String>
            {
                "How do I track my order?",
                "What is your return policy?",
                "How do I reset my password?",
                "How do I contact support?"
            };
        }
    }
}