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

        public IActionResult GoalsGraph()
        {
            return View();
        }

    }
}
