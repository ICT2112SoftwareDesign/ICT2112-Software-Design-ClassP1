using System.Text.Json.Serialization;

public class ManufacturerAnalyticsDTO
{
    [JsonPropertyName("manufacturerId")]
    public int ManufacturerId { get; set; }

    [JsonPropertyName("manufacturerName")]
    public string ManufacturerName { get; set; } = string.Empty;

    [JsonPropertyName("batchCount")]
    public int BatchCount { get; set; }
}