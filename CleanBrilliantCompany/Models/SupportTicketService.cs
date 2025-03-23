using System;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    // dummy class for testing only
    public class SupportTicketService : ISupportTicket
    {
        public bool createTicket(int customerID, int orderID, string ticketDetails)
        {
            // Logic to create a support ticket
            Console.WriteLine($"Support ticket created for Customer {customerID} and Order {orderID}. Ticket details: {ticketDetails}");
            return true; 
        }
    }

}