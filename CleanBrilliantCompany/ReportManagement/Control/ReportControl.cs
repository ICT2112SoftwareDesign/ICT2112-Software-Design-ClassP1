using System.Text;
using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.Interface;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.ReportManagement.Services;
using CleanBrilliantCompany.Services;

namespace CleanBrilliantCompany.Control
{
    public class ReportControl
    {

        private readonly ReportGenerator _reportGenerator;
        private readonly IAIService _AIService;
        private readonly ReportRepo _repo;
        private readonly IAnalyticsReportDetails _dashboardFacade;
        private readonly IForecastReportDetails _forecastFacade;
        public ReportControl(ReportGenerator reportGenerator, IAIService aiService, ReportRepo repo, IAnalyticsReportDetails dashboardFacade, IForecastReportDetails forecastFacade)
        {
            _reportGenerator = reportGenerator;
            _AIService = aiService;
            _repo = repo;
            _dashboardFacade = dashboardFacade;
            _forecastFacade = forecastFacade;
        }

        public async Task<Report> GenerateCustomReportAsync(List<string> selected)
        {
            var combinedDashboardData = new StringBuilder();
            combinedDashboardData.AppendLine(_dashboardFacade.GenerateCombinedReport(selected));

            if (selected.Contains("Forecast"))
            {
                combinedDashboardData.AppendLine("===== FORECAST DASHBOARD =====");
                combinedDashboardData.AppendLine(_forecastFacade.GenerateReport());
            }

            // Pass combined data to OpenAI for analysis
            string aiSummary = await _AIService.GenerateAnalysis(combinedDashboardData.ToString());

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

            // Save to database
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

        public async Task<Report?> GetReportAsync(int reportID)
        {
            return await _repo.FindByIdAsync(reportID);
        }

        public async Task<List<ReportLog>> GetReportLogsAsync(int reportID)
        {
            return await _repo.QueryStatusAsync(reportID);
        }
    }
}

