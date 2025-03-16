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
        [HttpGet("ticket")]
        public IActionResult Ticket()
        {
            return View(); // Renders Views/Staff/Ticket.cshtml
        }
    }
}

public class ShippingAgentController : Controller
{
    private readonly ShippingAgentDB _shippingAgentDB;

    public ShippingAgentController(ShippingAgentDB shippingAgentDB)
    {
        _shippingAgentDB = shippingAgentDB;
    }

    public IActionResult Index()
{
    var shippingAgents = _shippingAgentDB.FetchShippingAgents(); 
    return View(shippingAgents);
}

}
