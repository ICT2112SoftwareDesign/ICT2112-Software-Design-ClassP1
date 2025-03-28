using System.Text.Json.Serialization;

public class ManufacturerCostAnalyticsDTO
{
    [JsonPropertyName("manufacturerId")]
    public int ManufacturerId { get; set; }

    [JsonPropertyName("manufacturerName")]
    public string ManufacturerName { get; set; } = string.Empty;

    [JsonPropertyName("avgBatchCost")]
    public decimal AvgBatchCost { get; set; }
}