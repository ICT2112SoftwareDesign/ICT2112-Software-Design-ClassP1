public class ManufacturerMetricDetails
{
    public string ManufacturerId { get; set; }
    public double DeliveryRate { get; set; }
    public double DefectRate { get; set; }
    public double DependencyRate { get; set; }
    public bool RiskFlag { get; set; }

    public ManufacturerMetricDetails(string manufacturerId, double deliveryRate, double defectRate, double dependencyRate, bool riskFlag)
    {
        ManufacturerId = manufacturerId;
        DeliveryRate = deliveryRate;
        DefectRate = defectRate;
        DependencyRate = dependencyRate;
        RiskFlag = riskFlag;
    }
}
