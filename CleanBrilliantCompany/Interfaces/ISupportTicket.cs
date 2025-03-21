public interface ISupportTicket
{
    SupportTicketDTO FetchTicket(int ticketId);
    List<SupportTicketDTO> FetchAllTickets();
    void UpdateStatus(int ticketId, string status);
    void AssignToAgent(int ticketId, int assignee);
    void ResolveTicket(int ticketId, string resolutionDetails);
    int CreateTicket(int customerId, int orderId, string ticketDetails);
}
