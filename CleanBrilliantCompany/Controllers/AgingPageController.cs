using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;


public class AgingPageController : Controller 
{
    private readonly AgingControl agingControl; 
    private readonly ILogger<AgingPageController> logger; 


    public AgingPageController(ILogger<AgingPageController> logger, AgingMapper agingMapper) 
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
}