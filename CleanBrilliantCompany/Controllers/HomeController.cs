using System.Diagnostics;
using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CleanBrilliantCompany.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Db()
    {
        if (_context.Database.CanConnect())
        {
            ForecastDashboardDTO dto = new ForecastDashboardDTO(
                0,
                DateTime.Now,
                DateTime.Now,
                DateTime.Now
            );
            _context.ForecastDashboards.Add(dto);
            _context.SaveChanges();

            return Ok("Successfully connected to the database!");
        }
        else
        {
            return StatusCode(500, "Failed to connect to the database.");
        }
    }

    public IActionResult Dashboards()
    {
        return View(); // Will look for Views/Home/Dashboards.cshtml
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Cards()
    {
        return View();
    }

    public IActionResult Charts()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
