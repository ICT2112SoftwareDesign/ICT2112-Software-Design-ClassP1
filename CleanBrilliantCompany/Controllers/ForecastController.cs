using System;
using System.Collections.Generic;
using CleanBrilliantCompany.DataSource.Interface;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Forecast;
using Microsoft.AspNetCore.Mvc;

namespace CleanBrilliantCompany.Controllers;

[Route("[controller]")]
public class ForecastController : Controller
{
    private readonly ForecastControl _forecastControl;
    private readonly IForecastRepository forecastRepository;
    private readonly IForecastingFacade forecastingFacade;
    // Inject ForecastControl via DI
    public ForecastController(ForecastControl forecastControl)
    {
        _forecastControl = forecastControl;
    }
    // Handles user input actions, typically triggered from UI
    [HttpPost("handleInput")]
    public IActionResult HandleInput(int productId, int batchId)
    {
        // Implement logic to handle input
        return Ok();
    }

    // Fetches data needed for dashboard view
    [HttpGet("fetchDashboardData")]
    public IActionResult FetchDashboardData(
    )
    {
        ForecastDashboard dashboard = _forecastControl.GetDashboard();
        // Implement logic to fetch dashboard data
        return View("FetchDashboardData", dashboard);
    }

    // Generates forecast based on provided dates and detail level
    [HttpPost("generateForecast")]
    public IActionResult GenerateForecast(string type, DateTime startDate, DateTime endDate)
    {

        ForecastDashboard dashboard = _forecastControl.generateDashboard(type, startDate, endDate);
        return View("ForecastDashboard", dashboard);
    }

}
