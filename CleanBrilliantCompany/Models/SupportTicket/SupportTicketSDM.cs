using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models.SupportTicket
{
    public class SupportTicketSDM {
        [Column("ticketId")]
        public int TicketId { get; set; }
        [Column("customerId")]
        public int CustomerId { get; set; }
        [Column("ticketStatus")]
        public string Status { get; set; } = string.Empty;
        [Column("ticketCreatedAt")]
        public DateTime CreatedAt { get; set; }
        [Column("ticketDetails")]
        public string TicketDetails { get; set; } = string.Empty;
        [Column("resolutionDetails")]
        public string ResolutionDetails { get; set; } = string.Empty;

        // Getters and Setters
        public int GetTicketId() => TicketId;
        public int GetCustomerId() => CustomerId;
        public DateTime GetCreatedAt() => CreatedAt;
        public string GetStatus() => Status;
        public string GetTicketDetails() => TicketDetails;
        public string GetResolutionDetails() => ResolutionDetails;

        public void SetTicketId(int id) => TicketId = id;
        public void SetCustomerId(int id) => CustomerId = id;
        public void SetCreatedAt(DateTime date) => CreatedAt = date;
        public void SetStatus(string status) => Status = status;
        public void SetResolutionDetails(string details) => ResolutionDetails = details;
    }
}
