using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Models.SupportTicket;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Data.SupportTicket
{
    public class SupportTicketTableDataGateway
    {
        private static List<SupportTicketSDM> db = new List<SupportTicketSDM>
        {
            new SupportTicketSDM { TicketId = 1, CustomerId = 101, Status = "Open", CreatedAt = DateTime.Now, TicketDetails = "Issue with order", ResolutionDetails = "" },
            new SupportTicketSDM { TicketId = 2, CustomerId = 102, Status = "Closed", CreatedAt = DateTime.Now.AddHours(-1), TicketDetails = "Payment failed", ResolutionDetails = "Refund processed" }
        };

        private static int nextTicketId = 3;

        public SupportTicketSDM? fetchSupportTicket(int ticketId)
        {
            return db.FirstOrDefault(t => t.TicketId == ticketId);
        }

        public List<SupportTicketSDM> fetchAllSupportTickets()
        {
            return db;
        }

        public bool createSupportTicket(int customerId)
        {
            var ticket = new SupportTicketSDM
            {
                TicketId = nextTicketId++,
                CustomerId = customerId,
                Status = "Open",
                CreatedAt = DateTime.Now,
                TicketDetails = "Pending customer input",
                ResolutionDetails = ""
            };

            db.Add(ticket);
            return true;
        }

        public void deleteTicket(int ticketId)
        {
            var ticket = db.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                db.Remove(ticket);
            }
        }

        public void updateSupportTicket(int ticketId, string resolutionDetails)
        {
            var ticket = db.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.SetResolutionDetails(resolutionDetails);
                ticket.SetStatus("Closed");
            }
        }
    }
}
