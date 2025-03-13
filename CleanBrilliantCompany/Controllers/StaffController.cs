using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers.Staff
{
    [Route("staff")]
    public class StaffController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View(); // Renders Views/Staff/Index.cshtml
        }

        [HttpGet("shipping")]
        public IActionResult Shipping()
        {
            return View(); // Renders Views/Staff/Shipping.cshtml
        }

        [HttpGet("refund")]
        public IActionResult Refund()
        {
            return View(); // Renders Views/Staff/Refund.cshtml
        }

        [HttpGet("reorder")]
        public IActionResult Reorder()
        {
            return View(); // Renders Views/Staff/Reorder.cshtml
        }
        [HttpGet("ticket")]
        public IActionResult Ticket()
        {
            return View(); // Renders Views/Staff/Ticket.cshtml
        }
    }
}