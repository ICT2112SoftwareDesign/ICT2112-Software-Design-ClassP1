using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Data.SupportTicket;

namespace CleanBrilliantCompany.Models.SupportTicket
{
    public class SupportTicketManagement
    {
        private readonly SupportTicketTableDataGateway tableModule = new SupportTicketTableDataGateway();

        // Create a new support ticket
        public bool createSupportTicket(int customerId)
        {
            return tableModule.createSupportTicket(customerId);
        }

        // Delete a ticket by ID
        public void deleteTicket(int ticketId)
        {
            tableModule.deleteTicket(ticketId);
        }

        // Update resolution details and mark ticket as closed
        public void updateSupportTicket(int ticketId, string resolutionDetails)
        {
            tableModule.updateSupportTicket(ticketId, resolutionDetails);
        }

        // View all tickets
        public List<SupportTicketSDM> viewAllTickets()
        {
            return tableModule.fetchAllSupportTickets();
        }

        // View tickets filtered by status
        public List<SupportTicketSDM> viewTicketsByStatus(string status)
        {
            return tableModule
                .fetchAllSupportTickets()
                .Where(t => t.GetStatus().Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        // View details of a specific ticket
        public SupportTicketSDM viewTicketDetails(int ticketId)
        {
            var ticket = tableModule.fetchSupportTicket(ticketId);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }
            return ticket;
        }

        // checks if there are any Support Ticket stored in db
        public bool checkSupportTicketQuery()
        {
            return tableModule.fetchAllSupportTickets().Count > 0;
        }
    }
}
