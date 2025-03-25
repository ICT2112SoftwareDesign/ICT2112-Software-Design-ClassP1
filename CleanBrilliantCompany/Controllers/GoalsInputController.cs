using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    public class GoalsInputController : Controller
    {
        private readonly IGoalsDB _goalDb;

        // Constructor Injection of IGoalsDB
        public GoalsInputController(IGoalsDB goalDb)
        {
            _goalDb = goalDb;
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(GoalsSDM goal)
        {
            if (ModelState.IsValid)
            {
                await _goalDb.InsertGoal(goal);
                return RedirectToAction("Index", "GoalsPage");
            }
            return View(goal);
        }
    }
}