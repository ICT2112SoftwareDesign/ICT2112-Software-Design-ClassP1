public class DashboardBatchSummaryTable
{
    public int BatchSummaryId { get; set; }           // Auto-generated PK
    public int DashboardId { get; set; }              // FK to Dashboard
    public int CheapestBatchCode { get; set; }
    public int ExpensiveBatchCode { get; set; }
    public int CheapestManufacturer { get; set; }
    public int ExpensiveManufacturer { get; set; }
}