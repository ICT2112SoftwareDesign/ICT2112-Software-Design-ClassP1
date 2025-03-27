using System;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    // dummy class for testing only
    public class SupportTicketService : ISupportTicket
    {
        public bool createSupportTicket(int customerID, string ticketDetails)
        {
            // Logic to create a support ticket
            Console.WriteLine($"Support ticket created for Customer {customerID}. Ticket details: {ticketDetails}");
            return true; 
        }
    }

}