using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;

public class AnalyticsController : Controller
{
    private readonly CarbonOrderAnalyticManager _analyticManager;
    private readonly ILogger<AnalyticsController> _logger;

    public AnalyticsController(IGoals goalService, ILogger<AnalyticsController> logger)
    {
        _analyticManager = new CarbonOrderAnalyticManager(goalService);
        _logger = logger;
    }

    public async Task<IActionResult> DisplayGraph(DateTime? startDate, DateTime? endDate)
    {
        _logger.LogInformation("DisplayGraph action started.");

        // Fetch goals for the graph with optional date range filter
        var goals = await _analyticManager.RetrieveGoalsForGraph(startDate, endDate);

        if (!goals.Any())
        {
            _logger.LogError("No goals found for graph.");
            return View();
        }

        // Log the goals
        _logger.LogInformation("Retrieved goals: {0} goals found.", goals.Count());

        // Log the goal dates and emissions
        var dates = goals.Select(g => new DateTime(g.GetGoalYear(), g.GetGoalMonth(), 1)).ToList();
        var emissions = goals.Select(g => g.GetTargetEmission()).ToList();

        _logger.LogInformation("Dates: {0}", string.Join(", ", dates.Select(d => d.ToString("yyyy-MM"))));
        _logger.LogInformation("Emissions: {0}", string.Join(", ", emissions));

        // Serialize the lists to JSON format for use in the view
        ViewData["Dates"] = JsonConvert.SerializeObject(dates);
        ViewData["Emissions"] = JsonConvert.SerializeObject(emissions);

        return View();
    }
}