using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Models.SupportTicket
{
    public class SupportTicketSDM {
    public int TicketId { get; set; }
    public int CustomerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string TicketDetails { get; set; } = string.Empty;
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
