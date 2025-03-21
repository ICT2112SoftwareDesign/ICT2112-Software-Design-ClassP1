public class CostAnalyticsDetailsDTO
{
    public int AnalyticsId { get; set; }
    public int BatchCode { get; set; } // Foreign Key linking to batch
    public int DashboardId { get; set; } // Foreign Key linking to Dashboard
}