using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class ShippingAgentController : Controller
{
    private readonly ShippingAgentDB _shippingAgentDB;

    public ShippingAgentController(ShippingAgentDB shippingAgentDB)
    {
        _shippingAgentDB = shippingAgentDB;
    }

    public IActionResult Index()
    {
        var model = new ShippingAgentViewModel
        {
            ShippingAgents = _shippingAgentDB.FetchShippingAgents()
        };

        return View(model);
    }
}
