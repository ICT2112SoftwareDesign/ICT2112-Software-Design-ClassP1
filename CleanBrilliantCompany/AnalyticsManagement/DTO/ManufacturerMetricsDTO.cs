public class ManufacturerMetricsDTO
{
    public int MetricId { get; set; }
    public int DashboardId { get; set; }
    public int ManufacturerId { get; set; }
    public double DeliveryRate { get; set; }
    public double DefectRate { get; set; }
    public double DependencyRate { get; set; }
    public bool RiskFlag { get; set; }
}
