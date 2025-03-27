using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Interfaces.SupportTicket
{
    public interface ISupportTicket
    {
        bool createSupportTicket(int customerId, string ticketDetails);
        List<SupportTicketSDM> viewTicketByCustomer(int customerId);
    }
}
