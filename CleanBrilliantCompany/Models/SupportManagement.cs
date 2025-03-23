using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class SupportManagement : IChatbotQuery
    {
        private readonly IChatbot _chatBotService;
        private readonly ISupportTicket _supportTicketService;
        private readonly IOrder _orderService;

        public SupportManagement(IChatbot chatBotService, ISupportTicket supportTicketService, IOrder orderService)
        {
            _chatBotService = chatBotService;
            _supportTicketService = supportTicketService;
            _orderService = orderService;
        }

        public string handleCustomerChatbotQuery(Int32 customerID, String query)
        {
            var (response, parameters) = handleQuery(query);

            if (parameters.ContainsKey("orderID"))
            {
                int orderId;
                if (int.TryParse(parameters["orderID"], out orderId))
                {
                    var orderDetails = _orderService.getOrderDetails(orderId);
                    if (orderDetails != null)
                    {
                        return $"Order ID: {orderId}\nStatus: {orderDetails.Status}\nOrder Total: ${orderDetails.OrderTotal}";
                    }
                }
                return "You have entered an invalid  order ID. Please enter a valid order ID.";
            }
            else if (parameters.ContainsKey("issueDescription"))
            {
                string issueDescription = parameters["issueDescription"];
                Console.WriteLine($"this is the issue description: {issueDescription}");
                if (escalateToHumanAgent(customerID, issueDescription))
                {
                    return "Your issue has been escalated. Please wait for the agent to contact you!";
                }
            }

            return response;
        }

        public (string responseText, Dictionary<string, string> parameters) handleQuery(String query)
        {
            return _chatBotService.submitQuery(query);
        }

        public Dictionary<string, string> FetchFAQs()
        {
            return new Dictionary<string, string>
            {
                { "How do I track my order?", "You can track your order by logging into your account and checking the 'Order Status' section." },
                { "What is your return policy?", "We accept returns within 30 days of purchase. Items must be unused and in original packaging. Visit our Returns page for details." },
                { "How do I reset my password?", "Go to the login page, click 'Forgot Password,' and follow the instructions to reset your password via email." },
                { "How do I contact support?", "You can contact our support team via email at support@example.com or call us at +1-800-123-4567." }
            };
        }

        public bool createSupportTicket(Int32 customerID, Int32 orderID, String ticketDetails)
        {
            bool success = _supportTicketService.createTicket(customerID, orderID, ticketDetails);
            if (success)
            {
                Console.WriteLine("Support ticket created successfully!");
            }
            else
            {
                Console.WriteLine("Failed to create support ticket.");
                return false;
            }

            return success;
        }

        public bool escalateToHumanAgent(Int32 customerID, String query)
        {
            bool success = _supportTicketService.createTicket(customerID, 0, query);
            if (success)
            {
                Console.WriteLine("Successfully escalated the issue!");
            }
            else
            {
                Console.WriteLine("Failed to escalate the issue.");
                return false;
            }
            
            return success;
        }
        
    }
}