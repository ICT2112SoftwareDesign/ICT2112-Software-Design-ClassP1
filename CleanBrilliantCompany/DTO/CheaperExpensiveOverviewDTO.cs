using System.Text.Json.Serialization;

public class CheaperExpensiveOverviewDTO
{
    [JsonPropertyName("cheapestBatchCode")]
    public int CheapestBatchCode { get; set; }
    
    [JsonPropertyName("expensiveBatchCode")]
    public int ExpensiveBatchCode { get; set; }
    
    [JsonPropertyName("cheapestManufacturer")]
    public int CheapestManufacturer { get; set; }
    
    [JsonPropertyName("expensiveManufacturer")]
    public int ExpensiveManufacturer { get; set; }

    [JsonPropertyName("cheapestManufacturerName")]
    public string CheapestManufacturerName { get; set; }
    
    [JsonPropertyName("expensiveManufacturerName")]
    public string ExpensiveManufacturerName { get; set; }
    
    [JsonPropertyName("cheapestavgprice")]
    public decimal CheapestAvgPrice { get; set; }
    
    [JsonPropertyName("expensiveavgprice")]
    public decimal ExpensiveAvgPrice { get; set; }
}