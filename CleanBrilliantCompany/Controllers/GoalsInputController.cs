using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;

namespace CleanBrilliantCompany.Controllers
{
    public class GoalsInputController : Controller
    {
        private readonly GoalManagement _goalManager;
        private readonly ILogger<GoalsInputController> _logger;

        public GoalsInputController(GoalManagement goalManager, ILogger<GoalsInputController> logger)
        {
            _goalManager = goalManager;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> Create(GoalsSDM goal, string goalDate)
        {
            if (ModelState.IsValid && await _goalManager.CreateGoal(goal, goalDate))
            {
                return RedirectToAction("GoalsManagement", "GoalsPage");
            }

            ModelState.AddModelError("", "Invalid date format or error creating goal.");
            return View(goal);
        }

        [HttpPost]
        public async Task<ActionResult> ModifyGoal(string goalDate, double targetEmission)
        {
            if (await _goalManager.ModifyGoal(goalDate, targetEmission))
            {
                return RedirectToAction("GoalsManagement", "GoalsPage");
            }

            ModelState.AddModelError("", "Goal not found or invalid date format.");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteGoal(string goalDate)
        {
            if (await _goalManager.DeleteGoal(goalDate))
            {
                return RedirectToAction("GoalsManagement", "GoalsPage");
            }

            ModelState.AddModelError("", "Goal not found for deletion.");
            return View("Error");
        }
    }
}
