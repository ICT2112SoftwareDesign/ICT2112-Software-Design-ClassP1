using CleanBrilliantCompany.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Interface
{
    public interface IReportRepository
    {
        void InsertReport(Report report);
        void InsertReportLog(ReportLog log);
        Task<Report?> FindByIdAsync(int reportId);
        Task<List<Report>> FindAllAsync();
        Task<List<ReportLog>> QueryStatusAsync(int reportId);
        Task SaveChangesAsync();
    }
}
