// Controllers/AlertsController.cs
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    public class AlertsController : Controller
    {
        private readonly ICarbonNotification _carbonNotification;

        public AlertsController(ICarbonNotification carbonNotification)
        {
            _carbonNotification = carbonNotification;
        }

        // GET: Alerts
        public async Task<IActionResult> Index()
        {
            var alerts = await _carbonNotification.GetAllAlertsAsync();
            return View(alerts);
        }

        // GET: Alerts/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var alert = await _carbonNotification.GetAlertByIdAsync(id);
            if (alert == null)
            {
                return NotFound();
            }
            return View(alert);
        }

        // GET: Alerts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alerts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlertMessage")] Alert alert)
        {
            if (ModelState.IsValid)
            {
                alert.AlertDate = DateTime.Now;
                await _carbonNotification.CreateAlertAsync(alert);
                return RedirectToAction(nameof(Index));
            }
            return View(alert);
        }

        // GET: Alerts/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var alert = await _carbonNotification.GetAlertByIdAsync(id);
            if (alert == null)
            {
                return NotFound();
            }
            return View(alert);
        }

        // POST: Alerts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlertID,AlertDate,AlertMessage")] Alert alert)
        {
            if (id != alert.AlertID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var success = await _carbonNotification.UpdateAlertAsync(alert);
                if (!success)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alert);
        }

        // GET: Alerts/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var alert = await _carbonNotification.GetAlertByIdAsync(id);
            if (alert == null)
            {
                return NotFound();
            }
            return View(alert);
        }

        // POST: Alerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _carbonNotification.DeleteAlertAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
