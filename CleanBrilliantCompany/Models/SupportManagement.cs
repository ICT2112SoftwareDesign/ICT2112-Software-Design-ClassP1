using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.SupportTicket;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Models
{
    public class SupportManagement
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

        public string handleQuery(Int32 customerID, String query)
        {
            var (response, parameters) = _chatBotService.submitQuery(query);

            if (parameters.ContainsKey("orderID"))
            {
                int orderId;
                OrderRDM? orderDetails = null;
                if (int.TryParse(parameters["orderID"], out orderId))
                {
                    var orderHistory = _orderService.getOrderHistory(customerID);
                    foreach (var order in orderHistory)
                    {
                        if (order.RetrieveOrderID() == orderId) // Use RetrieveOrderID() instead of OrderID
                        {
                            orderDetails = order;
                            break;
                        }
                    }
                    if (orderDetails != null)
                    {
                        return $"Order ID: {orderId}\nStatus: {orderDetails.RetrieveStatus()}\nOrder Total: ${orderDetails.RetrieveOrderTotal()}"; // Use GetStatus() and GetOrderTotal()
                    }
                }
                return "You have entered an invalid order ID. Please enter a valid order ID.";
            }
            return response;
        }

        public bool createSupportTicket(Int32 customerID, String ticketDetails)
        {
            bool success = _supportTicketService.createSupportTicket(customerID, ticketDetails);
            

            return success;
        }

        public List<SupportTicketSDM> viewTicketByCustomer(int customerId) 
        {
            List<SupportTicketSDM> allCustomerSupportTicket = _supportTicketService.viewTicketByCustomer(customerId);
            return allCustomerSupportTicket;
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
    }
}