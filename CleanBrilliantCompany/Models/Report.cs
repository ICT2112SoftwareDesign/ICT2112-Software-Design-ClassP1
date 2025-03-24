namespace CleanBrilliantCompany.Models
{
    public class Report
    {
        public int ReportID { get; set; }
        public string ReportName { get; set; }
        public string ReportType { get; set; }
        public byte[] ReportData { get; set; }
    }
}
