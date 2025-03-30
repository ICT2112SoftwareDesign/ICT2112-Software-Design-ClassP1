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


        // For preview (returns a View with an iframe)
        public IActionResult PreviewReport()
        {
            return View(); // View will embed PDF
        }

        // For streaming the PDF
        public async Task<IActionResult> GetReportPdf()
        {
            var report = await _reportControl.GenerateReportAsync();

            Response.Headers.Add("Content-Disposition", "inline; filename=Report.pdf");

            return File(report.ReportData, "application/pdf");
        }

    }
}