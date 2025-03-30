using CleanBrilliantCompany.Control;

public class DashboardFacade : IDashboardFacade
{
    private readonly AgingControl agingControl;
    private readonly ManufacturerControl manufacturerControl;
    private readonly CostControl costControl;
    private readonly InventoryControl inventoryControl;



    public DashboardFacade(
        AgingControl agingControl,
        ManufacturerControl manufacturerControl,
        CostControl costControl,
        InventoryControl inventoryControl
        )
    {
        this.agingControl = agingControl;
        this.manufacturerControl = manufacturerControl;
        this.costControl = costControl;
        this.inventoryControl = inventoryControl;
    }


    public AgingControl GetAgingControl() => agingControl;
    public ManufacturerControl GetManufacturerControl() => manufacturerControl;
    public CostControl GetCostControl() => costControl;
    public InventoryControl GetInventoryControl() => inventoryControl;
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

        // lets simulate to run this so i print the dashboard data 
        return dashboards;
    }

}