using CleanBrilliantCompany.Control;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Forecast;

public class DashboardFacade : IDashboardFacade
{
    private readonly AgingControl agingControl;
    private readonly ManufacturerControl manufacturerControl;
    private readonly CostControl costControl;
    private readonly InventoryControl inventoryControl;
    private readonly IForecastReportDetails forecastControl;


    public DashboardFacade(
        AgingControl agingControl,
        ManufacturerControl manufacturerControl,
        CostControl costControl,
        InventoryControl inventoryControl,
        IForecastReportDetails forecastControl
        )
    {
        this.agingControl = agingControl;
        this.manufacturerControl = manufacturerControl;
        this.costControl = costControl;
        this.inventoryControl = inventoryControl;
        this.forecastControl = forecastControl;
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

    if (selected.Contains("Forecast"))
    {
        sb.AppendLine("===== FORECAST DASHBOARD =====");
        sb.AppendLine(forecastControl.GenerateReport());
    }

    return sb.ToString();
}


}