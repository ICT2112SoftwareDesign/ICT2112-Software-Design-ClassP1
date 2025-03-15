using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;

public class TransferController : Controller
{
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Transfer());
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
