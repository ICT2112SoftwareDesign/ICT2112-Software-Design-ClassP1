public class ManufacturerDashboardRdm : Dashboard
{
    // Constructor that passes values to the base class (Dashboard)
    public ManufacturerDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
        : base(id, name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        // Initialization code, if needed
        Metrics = new List<ManufacturerMetricsDTO>();  // Initialize the Metrics list
    }

    // Property to store the manufacturer metrics
    public List<ManufacturerMetricsDTO> Metrics { get; set; }  // List of metrics specific to the manufacturer dashboard

    // Any methods specific to the manufacturer dashboard can go here
}
