public class DashboardDTO
{
    public int DashboardId { get; set; }
    public string Name { get; set; } = string.Empty; 
    public DateTime RequestedStartDate { get; set; }
    public DateTime RequestedEndDate { get; set; }
    public DateTime GeneratedDate { get; set; }
    public int ValidityDuration { get; set; }
    public int Type { get; set; } // Foreign Key
}
