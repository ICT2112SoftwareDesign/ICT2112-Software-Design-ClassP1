// Controllers/AlertsController.cs
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace CleanBrilliantCompany.Controllers
{
    public class AlertsController : Controller
    {
        private readonly IAlertService _alertService;
        private readonly ILogger<AlertsController> _logger;

        public AlertsController(IAlertService alertService, ILogger<AlertsController> logger)
        {
            _alertService = alertService;
            _logger = logger;
        }

        // GET: Alerts
        public async Task<IActionResult> Index()
        {
            try
            {
                var alerts = await _alertService.GetAllAlertsAsync();
                // sort by goal period (year descending, then month descending)
                return View(alerts.OrderByDescending(a => a.GoalYear).ThenByDescending(a => a.GoalMonth));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error retrieving alerts for display.");
                return View(Enumerable.Empty<Alert>());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckPastMonth(int year, int month)
        {
            _logger.LogInformation("manual check requested for {Month}/{Year}", month, year);

            // basic validation
            if (month < 1 || month > 12 || year < 2020) // assume 2020 is a reasonable minimum year
            {
                TempData["ErrorMessage"] = "Invalid year or month provided.";
                _logger.LogWarning("invalid input for manual check: year={Year}, month={Month}", year, month);
                return RedirectToAction(nameof(Index));
            }

            // prevent checking future or current months
            var today = DateTime.UtcNow;
            var firstOfSelectedMonth = new DateTime(year, month, 1);
            var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);

            if (firstOfSelectedMonth >= firstOfCurrentMonth)
            {
                TempData["ErrorMessage"] = "Cannot check current or future months manually.";
                _logger.LogWarning("attempted manual check for current/future month: {Month}/{Year}", month, year);
                return RedirectToAction(nameof(Index));
            }

            try
            {
                // check if alert already exists
                bool exists = await _alertService.CheckAlertExistsAsync(year, month);
                if (exists)
                {
                    TempData["InfoMessage"] = $"An alert already exists for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}.";
                    _logger.LogInformation("manual check skipped for {Month}/{Year} because alert already exists.", month, year);

                    return RedirectToAction(nameof(Index));
                }

                // generate alert details
                var generatedAlert = await _alertService.GenerateAlertForPeriodAsync(year, month);

                if (generatedAlert != null)
                {
                    // save the alert
                    await _alertService.CreateAlertAsync(generatedAlert);
                    TempData["SuccessMessage"] = $"Alert successfully generated for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year} ({generatedAlert.Status}).";
                    _logger.LogInformation("manual check successful for {Month}/{Year}, status: {Status}", month, year, generatedAlert.Status);

                    // populate tempdata for the toast notification
                    TempData["Toast_GoalPeriod"] = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(generatedAlert.GoalMonth)} {generatedAlert.GoalYear}";
                    TempData["Toast_TargetEmission"] = generatedAlert.TargetEmission?.ToString("N2") ?? "n/a";
                    TempData["Toast_ActualEmission"] = generatedAlert.ActualTotalEmission.ToString("N2");
                    TempData["Toast_Status"] = generatedAlert.Status;
                }
                else
                {
                    TempData["ErrorMessage"] = $"Failed to generate alert data for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year}. check logs for details.";
                    _logger.LogError("manual check failed during alert generation for {Month}/{Year}.", month, year);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error during manual check for {Month}/{Year}.", month, year);
                TempData["ErrorMessage"] = "An unexpected error occurred while checking the month. please try again later.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAlert(int year, int month)
        {
            _logger.LogInformation("delete requested for {Month}/{Year}", month, year);
            try
            {
                bool deleted = await _alertService.DeleteAlertByPeriodAsync(year, month);
                if (deleted)
                {
                    TempData["SuccessMessage"] = $"Alert for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year} deleted successfully.";
                }
                else
                {
                    TempData["InfoMessage"] = $"No alert found for {CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month)} {year} to delete.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "error during alert deletion for {Month}/{Year}.", month, year);
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the alert.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
