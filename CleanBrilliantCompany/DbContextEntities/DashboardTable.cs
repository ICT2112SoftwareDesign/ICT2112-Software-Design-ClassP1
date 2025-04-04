using System;
using System.ComponentModel.DataAnnotations;

public class DashboardTable
{
    [Key]
    public int DashboardId { get; set; }
    public string Name { get; set; }
    public DateTime RequestedStartDate { get; set; }
    public DateTime RequestedEndDate { get; set; }
    public DateTime GeneratedDate { get; set; }
    public int ValidityDuration { get; set; }
    public int TypeId { get; set; }

    // Optionally, you can add navigation properties here if you have relationships (e.g., one-to-many with Analytics)
    // public ICollection<AgingAnalyticsDetailsTable> AgingAnalyticsDetails { get; set; }
}
