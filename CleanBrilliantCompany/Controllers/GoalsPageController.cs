using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interfaces;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    public class GoalsPageController : Controller
    {
        public IActionResult GoalsManagement()
        {
            return View();
        }

        public IActionResult GoalsCreation()
        {
            return View();
        }

        public IActionResult GoalsModification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ModifyGoal(int goalId, float targetEmission, int goalYear, int goalMonth)
        {
            // TODO: Add logic to update the goal in GoalsGateway or database
            return RedirectToAction("GoalsManagement");
        }
    }
}