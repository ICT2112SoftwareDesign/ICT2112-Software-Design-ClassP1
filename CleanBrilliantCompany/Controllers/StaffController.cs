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
        [HttpGet("shippingagent")]
        public IActionResult ShippingAgent()
        {
            return View(); // Renders Views/Staff/ShippingAgent.cshtml
        }

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
    }
}