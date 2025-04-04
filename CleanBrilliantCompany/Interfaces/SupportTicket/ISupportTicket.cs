using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Interfaces.SupportTicket
{
    public interface ISupportTicket
    {
        bool CreateSupportTicket(int customerId, string ticketDetails);
        List<SupportTicketSDM> ViewTicketByCustomer(int customerId);
    }
}
