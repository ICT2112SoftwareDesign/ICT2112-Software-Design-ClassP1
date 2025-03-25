using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Data.SupportTicket;
using CleanBrilliantCompany.Models.SupportTicket;
using CleanBrilliantCompany.Interfaces.SupportTicket;

namespace CleanBrilliantCompany.Models.SupportTicket
{
    public class SupportTicketManagement : ISupportTicket, iSupportTicketQuery
    {
        private readonly SupportTicketTableDataGateway _gateway;

        public SupportTicketManagement(SupportTicketTableDataGateway gateway)
        {
            _gateway = gateway;
        }

        public bool createSupportTicket(int customerId)
        {
            return _gateway.CreateSupportTicket(customerId);
        }

        public void deleteTicket(int ticketId)
        {
            _gateway.DeleteTicket(ticketId);
        }

        public void updateSupportTicket(int ticketId, string resolutionDetails)
        {
            _gateway.UpdateSupportTicket(ticketId, resolutionDetails);
        }

        public List<SupportTicketSDM> viewAllTickets()
        {
            return _gateway.FetchAllSupportTickets();
        }

        public List<SupportTicketSDM> viewTicketsByStatus(string status)
        {
            return _gateway
                .FetchAllSupportTickets()
                .FindAll(t => t.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
        }

        public SupportTicketSDM viewTicketDetails(int ticketId)
        {
            var ticket = _gateway.FetchSupportTicket(ticketId);
            if (ticket == null)
            {
                throw new Exception("Ticket not found");
            }
            return ticket;
        }

        public bool checkSupportTicketQuery()
        {
            return _gateway.FetchAllSupportTickets().Count > 0;
        }
    }
}
