using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Entity;

public class TransferController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View("Transfer");
    }

    [HttpPost]
    public IActionResult SaveTransfer(Transfer model)
    {
        if (ModelState.IsValid)
        {
            // Save transfer data to the database (implement logic here)
            return RedirectToAction("Index"); // Redirect to a list page
        }
        return View("Create", model);
    }
}
