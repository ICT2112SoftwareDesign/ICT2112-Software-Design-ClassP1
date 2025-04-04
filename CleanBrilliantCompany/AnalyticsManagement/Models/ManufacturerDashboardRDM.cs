public class ManufacturerDashboardRdm : Dashboard
{
    // Constructor that passes values to the base class (Dashboard)
    public ManufacturerDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
        : base(id, name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        // Initialization code, if needed
        Metrics = new List<ManufacturerMetricsDTO>();  // Initialize the Metrics list
        MetricDetailsList = new List<ManufacturerMetricDetails>();  // List for detailed metric calculations
        ManufacturerNames = new Dictionary<int, string>();
    }

    // Property to store the manufacturer metrics
    public List<ManufacturerMetricsDTO> Metrics { get; set; }  // List of metrics specific to the manufacturer dashboard

    // List of detailed metrics for calculations
    public List<ManufacturerMetricDetails> MetricDetailsList { get; set; }

    public Dictionary<int, string> ManufacturerNames { get; set; }

    public void populateMetrics(List<ReorderData> reorders)
    {
        // Grouping the reorders by ManufacturerId to calculate metrics for each manufacturer
        var manufacturerReorders = reorders
            .GroupBy(r => r.ManufacturerId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // Calculate total quantity ordered across all manufacturers
        double totalQuantityInSystem = reorders.Sum(o => o.Quantity);

        // Iterate over each manufacturer and calculate the metrics
        foreach (var manufacturerGroup in manufacturerReorders)
        {
            int manufacturerId = manufacturerGroup.Key;
            var orders = manufacturerGroup.Value;

            // Calculate DeliveryRate: number of delivered orders / total orders
            double totalOrders = orders.Count;
            double deliveredOrders = orders.Count(o => o.Status == "Delivered");
            double deliveryRate = totalOrders > 0 ? Math.Round(deliveredOrders / totalOrders, 3) : 0;

            // Calculate DefectRate: total defect quantity / total quantity
            double totalDefectQuantity = orders.Sum(o => o.DefectQuantity);
            double totalQuantity = orders.Sum(o => o.Quantity);
            double defectRate = totalQuantity > 0 ? Math.Round(totalDefectQuantity / totalQuantity, 3) : 0;

            // Calculate DependencyRate: total quantity for that manufacturer / total quantity in the system
            double dependencyRate = totalQuantityInSystem > 0 ? Math.Round(totalQuantity / totalQuantityInSystem, 3) : 0;

            // Calculate RiskFlag: true if defect rate * dependency rate > 0.1
            bool riskFlag = defectRate * dependencyRate > 0.1;

            string manufacturerIdString = manufacturerId.ToString();

            // Add to the list of metrics
            MetricDetailsList.Add(new ManufacturerMetricDetails(manufacturerIdString, deliveryRate, defectRate, dependencyRate, riskFlag));
        }
    }

}
