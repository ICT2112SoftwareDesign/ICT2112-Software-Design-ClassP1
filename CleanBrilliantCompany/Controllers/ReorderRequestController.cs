using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Data; // Ensure this matches your actual namespace
using CleanBrilliantCompany.Models;
using System.Linq;

namespace CleanBrilliantCompany.Controllers
{
    public class ReorderRequestController : Controller
    {
        private readonly AppDbContext _context;

        public ReorderRequestController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /ReorderRequest/ListOfReorders
        public IActionResult ListOfReorders()
        {
            var reorderRequests = _context.ReorderRequests.ToList();
            return View(reorderRequests);
        }

        // GET: /ReorderRequest/ReorderDetails/{id}
        public IActionResult ReorderDetails(int id)
        {
            var order = _context.ReorderRequests.Find(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // GET: /ReorderRequest/ReorderForm
        public IActionResult ReorderForm()
        {
            return View();
        }

        // POST: /ReorderRequest/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ReorderRequest model)
        {
            if (ModelState.IsValid)
            {
                _context.ReorderRequests.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(ListOfReorders));
            }
            return View("ReorderForm");
        }
    }
}
