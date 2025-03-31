using System;
using System.ComponentModel.DataAnnotations;

public class ManufacturerMetricsTable
{
    [Key]
    public int MetricId { get; set; }
    public int DashboardId { get; set; }  // Foreign Key
    public int ManufacturerId { get; set; }  // Foreign Key to ProductManufacturer
    public double DeliveryRate { get; set; }
    public double DefectRate { get; set; }
    public double DependencyRate { get; set; }
    public bool RiskFlag { get; set; }

    public DashboardTable Dashboard { get; set; }
    public ManufacturerTable Manufacturer { get; set; }
}
