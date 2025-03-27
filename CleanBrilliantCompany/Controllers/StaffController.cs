using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers.Staff
{
    [Route("staff")]
    public class StaffController : Controller
    {
        private readonly _IShippingAgentDB _IShippingAgentDB2;

        // Add constructor with dependency injection
        public StaffController(_IShippingAgentDB ShippingAgentDB)
        {
            _IShippingAgentDB2 = ShippingAgentDB;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View(); // Renders Views/Staff/Index.cshtml
        }

        // Add a redirect for the old route to maintain compatibility
        // [HttpGet("shippingagent")]
        // public IActionResult ShippingAgent()
        // {
        //     // Redirect to the new controller
        //     return RedirectToAction("Index", "ShippingAgent");
        // }

        [HttpGet("orderfufilment")]
        public IActionResult OrderFufilment()
        {
            return View(); // Renders Views/Staff/OrderFufilment.cshtml
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

        // Add this method for debugging - access it via /staff/debug
        [HttpGet("debug")]
        public IActionResult Debug()
        {
            return Content("StaffController is working!");
        }
    }
}