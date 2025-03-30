using System.Text;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;

namespace CleanBrilliantCompany.Control
{
    public class ReportControl
    {
        private readonly ReportGenerator _reportGenerator;
        private readonly IAIService _AIService;
        private readonly ReportRepo _repo;
        private readonly IDashboardFacade _dashboardFacade;
        public ReportControl(ReportGenerator reportGenerator, IAIService aiService, ReportRepo repo, IDashboardFacade dashboardFacade)
        {
            _reportGenerator = reportGenerator;
            _AIService = aiService;
            _repo = repo;
            _dashboardFacade = dashboardFacade;
        }

        public async Task<Report> GenerateReportAsync()
        {
            // Create new report
            var report = new Report
            {
                ReportName = "Q1 Performance Report",
                ReportType = "Sales"
            };


            // report.ReportDataText = aiSummary;

            // Generate PDF from report
            report.ReportData = _reportGenerator.GeneratePDF(report);

            // Insert into DB
            _repo.InsertReport(report);

            var log = new ReportLog
            {
                Report = report,
                GeneratedDate = DateTime.Now,
                Status = "Generated"
            };
            _repo.InsertReportLog(log);

            await _repo.SaveChangesAsync();

            return report;
        }

        public async Task<Report> GenerateCustomReportAsync(List<string> selected)
        {
            var sb = new StringBuilder();

            if (selected.Contains("Aging"))
                sb.AppendLine("===== AGING DASHBOARD =====");
            sb.AppendLine(_dashboardFacade.GetAgingControl().GenerateReport());

            if (selected.Contains("Manufacturer"))
                sb.AppendLine("===== MANUFACTURER DASHBOARD =====");
            sb.AppendLine(_dashboardFacade.GetManufacturerControl().GenerateReport());

            if (selected.Contains("Cost"))
                sb.AppendLine("===== COST DASHBOARD =====");
            sb.AppendLine(_dashboardFacade.GetCostControl().GenerateReport());

            if (selected.Contains("Inventory"))
                sb.AppendLine("===== INVENTORY DASHBOARD =====");
            sb.AppendLine(_dashboardFacade.GetInventoryControl().GenerateReport());

            string combinedDashboardData = sb.ToString();

            // Pass combined data to OpenAI for analysis
            string aiSummary = await _AIService.GenerateAnalysis(combinedDashboardData);

            var report = new Report
            {
                ReportName = "Custom Dashboard Report",
                ReportType = "Multi-Dashboard Summary",
                ReportDataText = aiSummary,
                ReportData = _reportGenerator.GeneratePDF(new Report
                {
                    ReportName = "Custom Dashboard Report",
                    ReportType = "Multi-Dashboard Summary",
                    ReportDataText = aiSummary
                })
            };

            // Save to db
            _repo.InsertReport(report);
            _repo.InsertReportLog(new ReportLog
            {
                Report = report,
                GeneratedDate = DateTime.Now,
                Status = "Generated"
            });
            await _repo.SaveChangesAsync();
            Console.WriteLine("AI Summary:\n" + aiSummary);
            return report;
        }



        public async Task<List<ReportLog>> GetReportLogsAsync(int reportID)
        {
            return await _repo.QueryStatusAsync(reportID);
        }

        public async Task<Report?> GetReportAsync(int reportID)
        {
            return await _repo.FindByIdAsync(reportID);
        }
    }
}
