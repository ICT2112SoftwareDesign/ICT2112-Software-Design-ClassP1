public class DashboardFactory {
    public static Dashboard? createDashboard(DashboardDTO dTO) {
        switch(dTO.Type) {
            case 4:
                return new CostDashboardRdm(
                    id: 0,
                    name: dTO.Name,
                    requestedStartDate: dTO.RequestedStartDate,
                    requestedEndDate: dTO.RequestedEndDate,
                    validityDuration: dTO.ValidityDuration,
                    type: dTO.Type,
                    generatedDate: dTO.GeneratedDate ?? DateTime.Now
                );

            default:
                return null;
        }
    } 
}