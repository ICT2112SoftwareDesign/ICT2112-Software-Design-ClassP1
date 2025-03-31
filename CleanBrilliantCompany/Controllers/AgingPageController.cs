using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;


public class AgingPageController : Controller 
{
    private readonly AgingControl agingControl; 
    private readonly ILogger<AgingPageController> logger; 

    // create 1 thats injected with the aging control 
    public AgingPageController(
        ILogger<AgingPageController> logger, 
        AgingControl agingControl
        ) 
        {
            this.logger = logger;
            this.agingControl = agingControl;
        }
        
    public IActionResult Index()
    {
        var latestDashboard = agingControl.GetLatestDashboard(); 
        if (latestDashboard == null) 
        {
            logger.LogWarning("No dashboard found.");
            return View();
        } 
        return View(latestDashboard); 
    } 



    [HttpPost]
    public IActionResult GenerateDashboard(DashboardDTO dto)
    {
        if (!ModelState.IsValid)
            return View(); // or handle the error gracefully

        agingControl.generateNewDashboard(dto);

        logger.LogInformation("New dashboard generated");
        return RedirectToAction("Index"); // or return a partial, etc.
    }

}