using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Controllers.SupportTicket
{
    [Route("staff/supportticket")]
    public class SupportTicketController : Controller
    {
        private readonly SupportTicketManagement _manager;

        public SupportTicketController(SupportTicketManagement manager)
        {
            _manager = manager;
        }

        [Route("")]
        public IActionResult SupportTicket()
        {
            var tickets = _manager.ViewAllTickets();
            return View("~/Views/SupportTicket/supportticket-index.cshtml", tickets);
        }

        [Route("Details/{ticketId}")]
        public IActionResult DisplayTicketDetails(int ticketId)
        {
            try
            {
                var ticket = _manager.ViewTicketDetails(ticketId);
                return View("~/Views/SupportTicket/supportticket-details.cshtml", ticket);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("UpdateSupportTicket/{ticketId}")]
        public IActionResult UpdateSupportTicket(int ticketId, string resolutionDetails)
        {
            _manager.UpdateSupportTicket(ticketId, resolutionDetails);
            return RedirectToAction("SupportTicket");
        }

        // for testing ISupportTicket interface - NOT IN CLASS DIAGRAM
        // [HttpPost]
        // [Route("CreateSupportTicket")]
        // public IActionResult CreateSupportTicket(int customerId, string ticketDetails)
        // {
        //     bool success = _manager.createSupportTicket(customerId, ticketDetails);

        //     if (success)
        //     {
        //         TempData["SuccessMessage"] = "Your ticket has been successfully created!";
        //         return RedirectToAction("SupportTicket");
        //     }
        //     else
        //     {
        //         TempData["ErrorMessage"] = "Failed to create support ticket. Please try again.";
        //         return RedirectToAction("SupportTicket");
        //     }
        // }
    }
}
