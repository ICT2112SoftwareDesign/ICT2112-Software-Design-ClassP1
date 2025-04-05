using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interface;
using Microsoft.EntityFrameworkCore;

namespace CleanBrilliantCompany.Gateway
{
    public class ReportGateway : IReportRepository
    {
        private readonly ApplicationDbContext _db;

        public ReportGateway(ApplicationDbContext db)
        {
            _db = db;
        }

        public void InsertReport(Report report)
        {
            _db.Reports.Add(report);
        }

        public void InsertReportLog(ReportLog log)
        {
            _db.ReportLogs.Add(log);
        }

        public async Task<Report?> FindByIdAsync(int reportId)
        {
            return await _db.Reports
                .Include(r => r.ReportLogs)
                .FirstOrDefaultAsync(r => r.ReportID == reportId);
        }

        public async Task<List<Report>> FindAllAsync()
        {
            return await _db.Reports
                .Include(r => r.ReportLogs)
                .ToListAsync();
        }

        public async Task<List<ReportLog>> QueryStatusAsync(int reportId)
        {
            return await _db.ReportLogs
                .Where(l => l.ReportID == reportId)
                .OrderByDescending(l => l.GeneratedDate)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task InsertReportWithLogAsync(Report report, string status)
        {
            var log = new ReportLog
            {
                GeneratedDate = DateTime.Now,
                Status = status,
                Report = report
            };

            _db.Reports.Add(report);
            _db.ReportLogs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
