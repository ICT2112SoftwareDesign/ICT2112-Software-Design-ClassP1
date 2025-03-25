using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Controllers.SupportTicket
{
    [Route("supportticket")]
    public class SupportTicketController : Controller
    {
        private readonly SupportTicketManagement _manager;

        public SupportTicketController(SupportTicketManagement manager)
        {
            _manager = manager;
        }

        [Route("")]
        public IActionResult Index()
        {
            var tickets = _manager.viewAllTickets();
            return View("~/Views/SupportTicket/supportticket-index.cshtml", tickets);
        }

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

        [HttpPost]
        [Route("UpdateSupportTicket/{ticketId}")]
        public IActionResult updateSupportTicket(int ticketId, string resolutionDetails)
        {
            _manager.updateSupportTicket(ticketId, resolutionDetails);
            return RedirectToAction("Index");
        }

        // for testing ISupportTicket interface 
        // [HttpPost]
        // [Route("CreateSupportTicket")]
        // public IActionResult createSupportTicket(int customerId)
        // {
        //     _manager.createSupportTicket(customerId);
        //     return RedirectToAction("Index");
        // }
    }
}
