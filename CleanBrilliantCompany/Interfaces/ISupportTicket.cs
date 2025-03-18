namespace CleanBrilliantCompany.Interfaces
{
    public interface ISupportTicket
    {
        bool createSupportTicket(int custID, int orderID);
        string getTicketOutcome(int tickID);
    }
}