public class SupportTicketDTO
{
    public int TicketId { get; set; }
    public int CustomerId { get; set; }
    public int OrderId { get; set; }
    public string? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? TicketDetails { get; set; }
    public string? ResolutionDetails { get; set; }

    // Getters and Setters
    public int GetTicketId() => TicketId;
    public int GetCustomerId() => CustomerId;
    public int GetOrderId() => OrderId;
    public DateTime GetCreatedAt() => CreatedAt;
    public string GetStatus() => Status;
    public string GetTicketDetails() => TicketDetails;
    public string GetResolutionDetails() => ResolutionDetails;

    public void SetTicketId(int id) => TicketId = id;
    public void SetCustomerId(int id) => CustomerId = id;
    public void SetOrderId(int id) => OrderId = id;
    public void SetCreatedAt(DateTime date) => CreatedAt = date;
    public void SetStatus(string status) => Status = status;
    public void SetResolutionDetails(string details) => ResolutionDetails = details;
}
