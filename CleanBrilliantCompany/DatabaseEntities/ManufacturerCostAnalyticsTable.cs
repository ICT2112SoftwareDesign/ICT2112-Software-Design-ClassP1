public class ManufacturerCostAnalyticsTable
{
    public int CostAnalyticsId { get; set; }          // Auto-generated PK
    public int DashboardId { get; set; }              // FK to Dashboard
    public int ManufacturerId { get; set; }
    public decimal AvgBatchCost { get; set; }
    public string ManufacturerName { get; set; }
}