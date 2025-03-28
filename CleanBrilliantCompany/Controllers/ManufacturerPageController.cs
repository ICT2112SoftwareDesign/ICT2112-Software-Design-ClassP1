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
        Console.WriteLine("HomeController initialized!");
        this.logger = logger;
        this.manufacturerControl = manufacturerControl;
    }

    public IActionResult Index()
    {

        Console.WriteLine("Index action called!");
        var latestDashboard = manufacturerControl.GetLatestDashboard(); 
        if (latestDashboard == null) 
        {
            Console.WriteLine("No manufacturer dashboard found.");
            return View();
        } 
        return View(latestDashboard); 
    }
}
