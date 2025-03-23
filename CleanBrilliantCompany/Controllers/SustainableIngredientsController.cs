// -----------------------------------------------------------------
// <filename> SustainableIngredientsController.cs </filename>
// <author> Yuen Wee Kin, Edwin </author>

using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class SustainableIngredientsController : Controller
    {
        // Action method for the sustainable ingredients page.
        public IActionResult Index()
        {
            return View();
        }
    }
}