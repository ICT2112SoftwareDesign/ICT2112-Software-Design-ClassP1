using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace YourProject.Controllers
{
    public class GoalsInputController : Controller
    {
        private readonly IGoalsDB _goalDb = new GoalsGateway();

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(GoalsSDM goal)
        {
            if (ModelState.IsValid)
            {
                _goalDb.AddGoal(goal);
                return RedirectToAction("Index", "GoalsPage");
            }
            return View(goal);
        }
    }
}