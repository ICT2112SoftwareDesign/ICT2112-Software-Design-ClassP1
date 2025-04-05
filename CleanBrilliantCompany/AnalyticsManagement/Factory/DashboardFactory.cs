public class DashboardFactory
{
    public static Dashboard? createDashboard(DashboardDTO dTO)
    {
        switch (dTO.Type)
        {
            case 1:
                return new AgingDashboardRdm(
                    id: 0,
                    name: "Aging Dashboard",
                    requestedStartDate: dTO.RequestedStartDate,
                    requestedEndDate: dTO.RequestedEndDate,
                    validityDuration: dTO.ValidityDuration,
                    type: 1
                );

            case 2:
                return new InventoryDashboardRDM(
                    id: dTO.DashboardId,
                    name: "Inventory Dashboard",
                    requestedStartDate: dTO.RequestedStartDate,
                    requestedEndDate: dTO.RequestedEndDate,
                    validityDuration: dTO.ValidityDuration,
                    type: dTO.Type,
                    generatedDate: dTO.GeneratedDate ?? DateTime.Now
                );

            case 3:
                return new ManufacturerDashboardRdm(
                    id: dTO.DashboardId,
                    name: "Manufacturer Dashboard",
                    requestedStartDate: dTO.RequestedStartDate,
                    requestedEndDate: dTO.RequestedEndDate,
                    validityDuration: dTO.ValidityDuration,
                    type: dTO.Type,
                    generatedDate: dTO.GeneratedDate ?? DateTime.Now
                );

            case 4:
                return new CostDashboardRdm(
                    id: dTO.DashboardId,
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
