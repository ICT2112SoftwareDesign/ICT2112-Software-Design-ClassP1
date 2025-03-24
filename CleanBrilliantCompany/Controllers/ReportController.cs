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

        public IActionResult GenerateReport()
        {
            var report = _reportControl.GenerateReport();
            return File(report.ReportData, "application/pdf", $"{report.ReportName}.pdf");
        }
    }
}