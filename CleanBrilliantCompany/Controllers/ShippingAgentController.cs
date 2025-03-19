using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Models;
using System.Collections.Generic;

namespace CleanBrilliantCompany.Controllers
{
    public class ShippingAgentController : Controller
    {
        private readonly IShippingAgentService _shippingAgentService;

        public ShippingAgentController(IShippingAgentService shippingAgentService)
        {
            _shippingAgentService = shippingAgentService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new ShippingAgentViewModel
            {
                ShippingAgents = await _shippingAgentService.GetShippingAgentsAsync() ?? new List<ShippingAgent>()
            };

            return View(model);
        }
    }
}
