using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Interfaces;
using System.Threading.Tasks;

namespace YourProject.Controllers
{
    public class GoalsPageController : Controller
    {
        private readonly IGoalsDB _goalDb = new GoalsGateway();

        public ActionResult Index()
        {
            var goals = _goalDb.GetAllGoals();
            return View(goals);
        }
    }
}