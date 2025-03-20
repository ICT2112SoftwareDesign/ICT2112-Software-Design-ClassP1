// using Microsoft.AspNetCore.Mvc;
// using CleanBrilliantCompany.Data; // Ensure this matches your actual namespace
// using CleanBrilliantCompany.Models;
// using System.Linq;
// using System.Threading.Tasks;
// using System.Collections.Generic;

// namespace CleanBrilliantCompany.Controllers
// {
//     public class ShippingAgentController : Controller
//     {
//         private readonly ApplicationDbContext _context;

//         public ShippingAgentController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         // GET: /ShippingAgent/List
//         public IActionResult List()
//         {
//             var shippingAgents = _context.ShippingAgents.ToList();
//             return View(shippingAgents);
//         }

//         // GET: /ShippingAgent/Details/{id}
//         public IActionResult Details(int id)
//         {
//             var agent = _context.ShippingAgents.Find(id);
//             if (agent == null) return NotFound();
//             return View(agent);
//         }

//         // GET: /ShippingAgent/Create
//         public IActionResult Create()
//         {
//             return View();
//         }

//         // POST: /ShippingAgent/Create
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public IActionResult Create(ShippingAgent model)
//         {
//             if (ModelState.IsValid)
//             {
//                 _context.ShippingAgents.Add(model);
//                 _context.SaveChanges();
//                 return RedirectToAction(nameof(List));
//             }
//             return View("Create");
//         }
//     }
// }


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
            var agents = await _shippingAgentService.GetShippingAgentsAsync();

            // Ensure agents is not null
            if (agents == null)
            {
                agents = new List<ShippingAgent>(); // ✅ Prevents null errors
            }

            var model = new ShippingAgentViewModel
            {
                ShippingAgents = agents
            };

            return View(model);
        }
    }
}
