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
        public async Task<JsonResult> Create(GoalsSDM goal, string goalDate, double targetEmission)
        {
            if (ModelState.IsValid)
            {
                bool success = await _goalManager.CreateGoal(goal, goalDate, targetEmission);
                if (success)
                {
                    return Json(new { success = true, message = "Goal created successfully!" });
                }
                return Json(new { success = false, message = "A goal for this month and year already exists." });
            }

            return Json(new { success = false, message = "Invalid input. Please check your data." });
        }

        [HttpPost]
        public async Task<JsonResult> ModifyGoal(string goalDate, double targetEmission)
        {
            bool success = await _goalManager.ModifyGoal(goalDate, targetEmission);

            if (success)
            {
                return Json(new { success = true, message = "Goal updated successfully!" });
            }

            return Json(new { success = false, message = "Goal not found for the specified date." });
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