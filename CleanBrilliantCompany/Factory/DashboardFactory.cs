public class DashboardFactory {
    public static Dashboard? createDashboard(DashboardDTO dTO) {
        switch(dTO.Type) {
            // case 1: // Aging Dashboard
            //     return new AgingDashboardRdm(
            //         id: 0,
            //         name: "Aging Dashboard",
            //         requestedStartDate: dTO.RequestedStartDate,
            //         requestedEndDate: dTO.RequestedEndDate,
            //         validityDuration: dTO.ValidityDuration,
            //         type: 1,
            //         generatedDate: dTO.GeneratedDate ?? DateTime.Now
            //     );

            case 3: // Manufacturer Dashboard
                return new ManufacturerDashboardRdm(
                    id: dTO.DashboardId,
                    name: "Manufacturer Dashboard",
                    requestedStartDate: dTO.RequestedStartDate ?? DateTime.MinValue,
                    requestedEndDate: dTO.RequestedEndDate ?? DateTime.MinValue,
                    validityDuration: dTO.ValidityDuration,
                    type: dTO.Type,
                    generatedDate: dTO.GeneratedDate ?? DateTime.Now
                );

            default:
                return null;
        }
    } 
}
