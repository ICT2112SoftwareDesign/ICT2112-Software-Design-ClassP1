using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.ReportManagement.Services;
using CleanBrilliantCompany.Control;

namespace CleanBrilliantCompany.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportControl _reportControl;

        private readonly ReportGenerator _reportGenerator;
        
        public ReportController(ReportControl reportControl, ReportGenerator reportGenerator)
        {
            _reportControl = reportControl;
            _reportGenerator = reportGenerator;
        }

        public IActionResult Index() => View();

        public IActionResult PreviewReport()
        {
            return View(); // View will embed PDF
        }

        public IActionResult ViewReport()
        {
            return View();
        }

        public IActionResult GetReportPdf()
        {
            byte[]? reportData = HttpContext.Session.Get("LatestReport");

            if (reportData == null)
            {
                // Fallback dummy PDF if no real data is available
                string dummyHtml = @"
            Sample Report Preview
            This is a placeholder preview. Your actual AI-generated report will appear here once generated. 
            Summarized dashboard insights
            Explained metrics
            Download-ready format
            
        ";

                byte[] fallbackPdf = _reportGenerator.GeneratePDF(new Report
                {
                    ReportName = "Sample Preview",
                    ReportType = "Placeholder",
                    ReportDataText = dummyHtml
                });

                Response.Headers.Add("Content-Disposition", "inline; filename=SampleReportPreview.pdf");
                return File(fallbackPdf, "application/pdf");
            }

            Response.Headers.Add("Content-Disposition", "inline; filename=CustomReport.pdf");
            return File(reportData, "application/pdf");
        }

        [HttpPost]
        public async Task<IActionResult> GenerateSelectedReport(List<string> selectedDashboards)
        {
            if (selectedDashboards == null || !selectedDashboards.Any())
            {
                TempData["Error"] = "Please select at least one dashboard to generate the report.";
                return RedirectToAction("PreviewReport"); // Return to selection page if empty
            }

            var report = await _reportControl.GenerateCustomReportAsync(selectedDashboards);

            // Store report data in memory for the next request (Session)
            HttpContext.Session.Set("LatestReport", report.ReportData); // Needs Session configured
            return RedirectToAction("ViewReport");
        }

    }
}