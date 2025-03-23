namespace CleanBrilliantCompany.Interfaces
{
    // just for testing
    public interface ISupportTicket
    {
        bool createTicket(int customerID, int orderID, string ticketDetails);
    }
}