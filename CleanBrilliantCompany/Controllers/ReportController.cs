using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;

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

        // For streaming the PDF
        public async Task<IActionResult> GetReportPdf()
        {
            var report = await _reportControl.GenerateReportAsync();

            Response.Headers.Add("Content-Disposition", "inline; filename=Report.pdf");

            return File(report.ReportData, "application/pdf");
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
            var reportHtmlSections = new List<string>();

            if (selectedDashboards.Contains("Aging"))
                reportHtmlSections.Add(_dashboardFacade.GetAgingControl().GenerateReport());

            if (selectedDashboards.Contains("Manufacturer"))
                reportHtmlSections.Add(_dashboardFacade.GetManufacturerControl().GenerateReport());

            if (selectedDashboards.Contains("Cost"))
                reportHtmlSections.Add(_dashboardFacade.GetCostControl().GenerateReport());

            if (selectedDashboards.Contains("Inventory"))
                reportHtmlSections.Add(_dashboardFacade.GetInventoryControl().GenerateReport());

            string combinedHtml = string.Join("<hr/>", reportHtmlSections);

            var report = new Report
            {
                ReportName = "Custom Report",
                ReportData = _reportGenerator.GeneratePDF(new Report
                {
                    ReportName = "Custom Report",
                    ReportDataText = combinedHtml
                })
            };

            return File(report.ReportData, "application/pdf", "CustomReport.pdf");
        }

    }
}