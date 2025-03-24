using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class ReportController : Controller
    {
        private readonly Control.ReportControl _reportControl;

        public ReportController(Control.ReportControl reportControl)
        {
            _reportControl = reportControl;
        }

        public IActionResult Index() => View();

        public async Task<IActionResult> GenerateReport()
        {
            var report = await _reportControl.GenerateReportAsync();
            return File(report.ReportData, "application/pdf", $"{report.ReportName}.pdf");
        }
    }
}