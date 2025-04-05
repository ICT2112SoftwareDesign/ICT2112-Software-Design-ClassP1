using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CleanBrilliantCompany.Models
{

    [Table("AnalyticsReportLog")] // For renamed table
    public class ReportLog
    {
        [Key]
        public int LogID { get; set; }

        [ForeignKey("Report")]
        public int ReportID { get; set; }

        public DateTime GeneratedDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public Report Report { get; set; } = null!;
    }
}
