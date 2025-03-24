public class DashboardFactory {
    public static Dashboard? createDashboard(DashboardDTO dTO) {
        switch(dTO.Type) {
            case 1:
                return new AgingDashboardRdm(
                    id: 0,
                    name: "Aging Dashboard",
                    requestedStartDate: dTO.RequestedStartDate,
                    requestedEndDate: dTO.RequestedEndDate,
                    validityDuration: dTO.ValidityDuration,
                    type: 1,
                    generatedDate: dTO.GeneratedDate ?? DateTime.Now
                );

            default:
                return null;
        }
    } 
}