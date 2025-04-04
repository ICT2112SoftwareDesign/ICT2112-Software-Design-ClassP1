using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.ReportManagement.Services;

namespace CleanBrilliantCompany.Controllers
{
    public class ReportController : Controller
    {
        private readonly Control.ReportControl _reportControl;
        private readonly IDashboardFacade _dashboardFacade;

        private readonly AgingControl _agingControl;

        private readonly CostControl _costControl;

        private readonly ManufacturerControl _manufacturerControl;
        private readonly ReportGenerator _reportGenerator;
        public ReportController(Control.ReportControl reportControl, IDashboardFacade dashboardFacade, AgingControl agingControl, ManufacturerControl manufacturerControl, CostControl costControl, ReportGenerator reportGenerator)
        {
            _reportControl = reportControl;
            _dashboardFacade = dashboardFacade;
            _agingControl = agingControl;
            _manufacturerControl = manufacturerControl;
            _costControl = costControl;
            _reportGenerator = reportGenerator;
        }

        public IActionResult Index() => View();


        // For preview (returns a View with an iframe)
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


        [HttpGet]
        public IActionResult TestDashboards()
        {
            var dashboards = _dashboardFacade.getDashboardsData();
            return Json(dashboards);
        }

        [HttpGet]
        public IActionResult ViewAgingReport()
        {
            string reportHtml = _agingControl.GenerateReport();
            ViewBag.ReportHtml = reportHtml;
            return View();
        }

        [HttpGet]
        public IActionResult ViewManufacturerReport()
        {
            string reportHtml = _manufacturerControl.GenerateReport();
            ViewBag.ReportHtml = reportHtml;
            return View();
        }
        [HttpGet]
        public IActionResult ViewCostReport()
        {
            string reportHtml = _costControl.GenerateReport();
            ViewBag.ReportHtml = reportHtml;
            return View();
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

            // Store report data in memory for the next request (Session or TempData or Singleton)
            HttpContext.Session.Set("LatestReport", report.ReportData); // Needs Session configured
            return RedirectToAction("ViewReport");
        }

    }
}