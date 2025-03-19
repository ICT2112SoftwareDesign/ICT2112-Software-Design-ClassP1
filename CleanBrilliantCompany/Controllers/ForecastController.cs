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

    [HttpPost("generateForecast")]
    public IActionResult GenerateForecast(string type, string month, int? priceAdjustment)
    {
        // Append "-01" to convert the month string into a full date (e.g., "2025-03-01")
        if (!DateTime.TryParse(month + "-01", out DateTime forecastMonth))
        {
            return BadRequest("Invalid month format.");
        }

        // Generate your forecast dashboard using forecastMonth
        ForecastDashboard dashboard = _forecastControl.generateDashboard(
            type, forecastMonth, forecastMonth.AddMonths(1).AddDays(-1), priceAdjustment
        );

        return View("FetchDashboardData", dashboard);
    }


}
