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

        public async Task<Report> GenerateReportAsync()
        {
            var report = new Report
            {
                ReportID = 1,
                ReportName = "Q1 Performance Report",
                ReportType = "Sales"
            };

            // DUMMY INPUT
            string dummyData = @"
            Sales increased by 20% in Q1.
            Inventory levels improved.
            Manufacturer Y had a 3-day delay in delivery.
            Costs decreased due to bulk shipping.";

            // Generate AI summary
            string aiSummary = await _AIService.GenerateAnalysis(dummyData);

            // Replace this 
            report.ReportData = _reportGenerator.GeneratePDF(new Report
            {
                ReportName = report.ReportName,
                ReportType = report.ReportType
            });

            return report;
        }

        public List<ReportLog> GetReportLogs(int reportID) => new(); // Placeholder
        public Report GetReport(int reportID) => new(); // Placeholder
    }
}
