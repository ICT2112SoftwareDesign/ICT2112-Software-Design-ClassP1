using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

public class ManufacturerPageController : Controller
{
    private readonly ManufacturerControl manufacturerControl; 
    private readonly ILogger<ManufacturerPageController> logger; 

    public ManufacturerPageController(
        ILogger<ManufacturerPageController> logger, 
        ManufacturerControl manufacturerControl
    ) 
    {
        this.logger = logger;
        this.manufacturerControl = manufacturerControl;
    }

    public IActionResult Index()
    {
        var latestDashboard = manufacturerControl.GetLatestDashboard(); 
        if (latestDashboard == null) 
        {
            Console.WriteLine("No manufacturer dashboard found.");
            return View();
        } 
        return View(latestDashboard); 
    }

    [HttpPost]
    public IActionResult GenerateDashboard(DashboardDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return View(); // Handle the error gracefully, or return the user to the same form if the model is invalid
        }

        // Step 1: Generate the new dashboard
        manufacturerControl.GenerateNewDashboard(dto);

        // Log the dashboard generation and redirect
        logger.LogInformation("New manufacturer dashboard generated.");
        return RedirectToAction("Index"); // Redirect back to the dashboard index or another page
    }
}
