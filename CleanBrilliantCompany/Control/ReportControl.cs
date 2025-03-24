using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;

namespace CleanBrilliantCompany.Control
{
    public class ReportControl
    {
        private readonly ReportGenerator _reportGenerator;
        private readonly IAIService _AIService;

        public ReportControl(ReportGenerator reportGenerator, IAIService AIService)
        {
            _reportGenerator = reportGenerator;
            _AIService = AIService;
        }

        public Report GenerateReport()
        {
            // placeholder
            var report = new Report
            {
                ReportID = 1,
                ReportName = "Sales Report",
                ReportType = "Sales"
            };

            var aiContent = _AIService.GenerateAnalysis("Sales data input");
            report.ReportData = _reportGenerator.GeneratePDF(report);

            return report;
        }

        public List<ReportLog> GetReportLogs(int reportID) => new(); // Placeholder
        public Report GetReport(int reportID) => new(); // Placeholder
    }
}
