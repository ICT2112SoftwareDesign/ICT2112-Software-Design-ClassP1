using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CleanBrilliantCompany.Models
{
    [Table("Report")]
    public class Report
    {
        [Key]
        public int ReportID { get; set; }

        public string ReportName { get; set; } = string.Empty;
        public string ReportType { get; set; } = string.Empty;
        public byte[]? ReportData { get; set; }
        public string? ReportDataText { get; set; }

        public ICollection<ReportLog>? ReportLogs { get; set; }
    }

}
