using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
/*
namespace CleanBrilliantCompany.Controllers
{
    public class GoalsInputController : Controller
    {
        private readonly IGoalsDB _goalDb;
        private readonly ILogger<GoalsInputController> _logger;

        // Constructor Injection of IGoalsDB
        public GoalsInputController(IGoalsDB goalDb, ILogger<GoalsInputController> logger)
        {
            _goalDb = goalDb;
            _logger = logger;
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

        [HttpPost]
        public async Task<ActionResult> ModifyGoal(string goalDate, float targetEmission)
        {
            _logger.LogInformation("ModifyGoal action started."); // Log method execution start

            if (DateTime.TryParseExact(goalDate, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                int goalYear = parsedDate.Year;
                int goalMonth = parsedDate.Month;

                _logger.LogInformation($"Searching for goal with Year={goalYear} and Month={goalMonth}.");

                var existingGoal = await _goalDb.FindGoalByDate(goalYear, goalMonth);

                if (existingGoal == null)
                {
                    _logger.LogWarning($"Goal not found for Year={goalYear}, Month={goalMonth}");
                    ModelState.AddModelError("", "Goal not found.");
                    return View();
                }

                _logger.LogInformation($"Updating Goal: Year={goalYear}, Month={goalMonth}, TargetEmission={targetEmission}");

                existingGoal.UpdateTargetEmission(targetEmission);
                await _goalDb.UpdateGoal(existingGoal);

                return RedirectToAction("Index", "GoalsPage");
            }

            _logger.LogError("Invalid date format received.");
            ModelState.AddModelError("", "Invalid date format.");
            return View();
        }
    }
}
*/