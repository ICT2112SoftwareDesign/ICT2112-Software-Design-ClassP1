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

    public List<Dashboard> getDashboardsData()
    {
        // this thing just calls every dashboard's getDashboardData method 
        List<Dashboard> dashboards = new List<Dashboard>();

        // go to control class and get the dashboard from aging 
        Dashboard agingDashboard = agingControl.GetLatestDashboard();
        dashboards.Add(agingDashboard);

        Dashboard manufacturerDashboard = manufacturerControl.GetLatestDashboard();
        dashboards.Add(manufacturerDashboard);

        Dashboard costDashboard = costControl.GetLatestDashboard();
        dashboards.Add(costDashboard);

        string inventoryReport = inventoryControl.GenerateReport();

        return dashboards;
    }
    public string GetForecastReport() => forecastControl.GenerateReport();


}