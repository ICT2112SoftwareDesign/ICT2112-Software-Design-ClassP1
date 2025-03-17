using System;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class SupportManagement : IChatbotQuery
    {
        private readonly ISupportTicket _ticketService;
        private readonly IChatbot _chatBotService;
        // private readonly IOrder _orderService;

        public string HandleCustomerChatbotQuery(String query)
        {
            bool handled = handleQuery(query);
            
            if (handled)
            {
                return "Chatbot handled the query.";
            }
            else
            {
                return "Chatbot cannot handle the query!!!!";
            }
        }

        public bool handleQuery(String query)
        {
            return _chatBotService.submitQuery(query);
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