using CleanBrilliantCompany.Control;

public class DashboardFacade : IAnalyticsReportDetails
{
    private readonly AgingControl agingControl;
    private readonly ManufacturerControl manufacturerControl;
    private readonly CostControl costControl;
    private readonly InventoryControl inventoryControl;

    public DashboardFacade(
        AgingControl agingControl,
        ManufacturerControl manufacturerControl,
        CostControl costControl,
        InventoryControl inventoryControl)
    {
        this.agingControl = agingControl;
        this.manufacturerControl = manufacturerControl;
        this.costControl = costControl;
        this.inventoryControl = inventoryControl;
    }


    public string GenerateCombinedReport(List<string> selected)
    {
        var sb = new System.Text.StringBuilder();

        if (selected.Contains("Aging"))
        {
            sb.AppendLine("===== AGING DASHBOARD =====");
            sb.AppendLine(agingControl.GenerateReport());
        }

        if (selected.Contains("Manufacturer"))
        {
            sb.AppendLine("===== MANUFACTURER DASHBOARD =====");
            sb.AppendLine(manufacturerControl.GenerateReport());
        }

        if (selected.Contains("Cost"))
        {
            sb.AppendLine("===== COST DASHBOARD =====");
            sb.AppendLine(costControl.GenerateReport());
        }

        if (selected.Contains("Inventory"))
        {
            sb.AppendLine("===== INVENTORY DASHBOARD =====");
            sb.AppendLine(inventoryControl.GenerateReport());
        }

        return sb.ToString();
    }
}
