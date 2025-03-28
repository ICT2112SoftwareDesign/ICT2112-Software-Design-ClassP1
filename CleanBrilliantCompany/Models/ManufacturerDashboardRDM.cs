public class ManufacturerDashboardRdm : Dashboard
{
    // Your implementation of ManufacturerDashboardRdm here
    public ManufacturerDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
        : base(id ,name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        // Initialization code
    }

    // Any methods specific to the manufacturer dashboard
}
