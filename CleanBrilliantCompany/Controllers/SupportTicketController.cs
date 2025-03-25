using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Controllers.SupportTicket
{
    [Route("supportticket")]
    public class SupportTicketController : Controller
    {
        private readonly SupportTicketManagement _manager = new SupportTicketManagement();

        // Admin: View all tickets
        [Route("")]
        public IActionResult Index()
        {
            var tickets = _manager.viewAllTickets();
            return View("~/Views/SupportTicket/supportticket-index.cshtml", tickets);
        }

        // Admin: View details of a specific ticket
        [Route("Details/{ticketId}")]
        public IActionResult displayTicketDetails(int ticketId)
        {
            try
            {
                var ticket = _manager.viewTicketDetails(ticketId);
                return View("~/Views/SupportTicket/supportticket-details.cshtml", ticket);
            }
            catch
            {
                return NotFound();
            }
        }

        // Admin: Update support ticket
        [HttpPost]
        [Route("UpdateSupportTicket/{ticketId}")]
        public IActionResult updateSupportTicket(int ticketId, string resolutionDetails)
        {
            _manager.updateSupportTicket(ticketId, resolutionDetails);
            return RedirectToAction("Index");
        }
    }
}
