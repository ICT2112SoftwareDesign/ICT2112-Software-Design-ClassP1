using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers
{
    public class DashboardPageController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
