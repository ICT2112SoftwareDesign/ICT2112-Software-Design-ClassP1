namespace CleanBrilliantCompany.Models
{
    public class ReportLog
    {
        public int LogID { get; set; }
        public int ReportID { get; set; }
        public DateTime GeneratedDate { get; set; }
        public string Status { get; set; }
    }
}
