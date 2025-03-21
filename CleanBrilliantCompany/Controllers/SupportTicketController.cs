using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers.SupportTicket
{
    [Route("staff/supportticket")]
    public class SupportTicketController : Controller
    {
        private static List<SupportTicketDTO> tickets = new List<SupportTicketDTO>
        {
            new SupportTicketDTO { TicketId = 1, CustomerId = 101, OrderId = 1001, Status = "Open", CreatedAt = DateTime.Now, TicketDetails = "Issue with order", ResolutionDetails = "" },
            new SupportTicketDTO { TicketId = 2, CustomerId = 102, OrderId = 1002, Status = "Closed", CreatedAt = DateTime.Now.AddHours(-1), TicketDetails = "Payment failed", ResolutionDetails = "Refund processed" },
        };

        // Fetch all tickets
        [Route("")]
        public IActionResult Index()
        {
            return View("~/Views/Staff/SupportTicket/supportticket-index.cshtml", tickets); // Correct path
        }

        // Fetch details of a specific ticket
        [Route("Details/{ticketId}")]
        public IActionResult Details(int ticketId)
        {
            var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket == null)
            {
                return NotFound();
            }
            return View("~/Views/Staff/SupportTicket/supportticket-details.cshtml", ticket); // Correct path
        }

        // Update ticket status and add resolution details
        [HttpPost]
        [Route("UpdateSupportTicket/{ticketId}")]
        public IActionResult UpdateSupportTicket(int ticketId, string resolutionDetails)
        {
            var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                // Add resolution details and update the status to Closed
                ticket.SetResolutionDetails(resolutionDetails);
                ticket.SetStatus("Closed");
            }
            return RedirectToAction("Index");
        }

        // Update ticket status (e.g., when status is changed, not resolved)
        [HttpPost]
        [Route("UpdateStatus/{ticketId}")]
        public IActionResult UpdateStatus(int ticketId, string status)
        {
            var ticket = tickets.FirstOrDefault(t => t.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.SetStatus(status);
            }
            return RedirectToAction("Index");
        }
    }
}
