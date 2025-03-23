using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;


public class AgingPageController : Controller 
{
    private readonly AgingControl agingControl; 
    private readonly ILogger<AgingPageController> logger; 


    public AgingPageController(ILogger<AgingPageController> logger, AgingRepo agingMapper) 
    {
        this.logger = logger;
        agingControl = new AgingControl(agingMapper);
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

    // [HttpPost]
    // public IActionResult GenerateDashboard()
    // {
    //     var newDashboard = agingControl.generateNewDashboard();

    //     //might wanna post the data to the datebase 

    //     // log the new dashboard 
    //     logger.LogInformation("New dashboard generated: {0}", newDashboard.Name); 
    //     return Json(new { success = true, message = "New dashboard generated!", data = newDashboard });


    // }

    [HttpPost]
    public IActionResult GenerateDashboard(AgingDashboardInputDTO dto)
    {
        if (!ModelState.IsValid)
            return View(); // or handle the error gracefully

        agingControl.generateNewDashboard(dto);

        logger.LogInformation("New dashboard generated");
        return RedirectToAction("Index"); // or return a partial, etc.
    }

}