using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;
using Newtonsoft.Json;
using CleanBrilliantCompany.DTO;

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

        // Retrieve Emissions to load from db
        await _analyticManager.retrieveOrderEmission();
        await _analyticManager.retrieveItemEmission();

        // Retrieve Predictions
        await _analyticManager.predictCarbonEmission();
        List<EmissionPredDTO> predictions = _analyticManager.getPredictEmission();

        

        if (!goals.Any())
        {
            _logger.LogError("No goals found for graph.");
            return View();
        }

        // Log the goals
        _logger.LogInformation("Retrieved goals: {0} goals found.", goals.Count());

        // Log the goal dates and emissions
        var goalDates = goals.Select(g => new DateTime(g.GetGoalYear(), g.GetGoalMonth(), 1)).ToList();
        var goalEmissions = goals.Select(g => g.GetTargetEmission()).ToList();

        // ViewBag.goalDates = goalDates;
        // ViewBag.goalEmissions = goalEmissions;

        _logger.LogInformation("Dates: {0}", string.Join(", ", goalDates.Select(d => d.ToString("yyyy-MM"))));
        _logger.LogInformation("Emissions: {0}", string.Join(", ", goalEmissions));

        // Serialize the lists to JSON format for use in the view
        ViewData["graph"] = JsonConvert.SerializeObject(await _analyticManager.getGraphEmission());
        ViewData["goalDates"] = JsonConvert.SerializeObject(goalDates);
        ViewData["goalEmissions"] = JsonConvert.SerializeObject(goalEmissions);

        ViewData["predictions"] = JsonConvert.SerializeObject(predictions);

        return View();
    }
}