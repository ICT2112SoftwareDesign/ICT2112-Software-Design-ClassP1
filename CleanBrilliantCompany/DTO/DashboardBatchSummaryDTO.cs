using System.Text.Json.Serialization;

public class DashboardBatchSummaryDTO
{
    
    [JsonPropertyName("cheapestBatchCode")]
    public int CheapestBatchCode { get; set; }
    
    [JsonPropertyName("expensiveBatchCode")]
    public int ExpensiveBatchCode { get; set; }
    
    [JsonPropertyName("cheapestManufacturer")]
    public int CheapestManufacturer { get; set; }
    
    [JsonPropertyName("expensiveManufacturer")]
    public int ExpensiveManufacturer { get; set; }
}