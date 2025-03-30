public class DashboardFacade : IDashboardFacade
{
    private readonly ManufacturerControl manufacturerControl;

    public DashboardFacade(
        ManufacturerControl manufacturerControl
        )
    {
        this.manufacturerControl = manufacturerControl;
    }

    public List<Dashboard> getDashboardsData()
    {
        // this thing just calls every dashboard's getDashboardData method 
        List<Dashboard> dashboards = new List<Dashboard>();

        // go to control class and get the dashboard from aging 
        Dashboard manufacturerDashboard = manufacturerControl.GetLatestDashboard();
        dashboards.Add(manufacturerDashboard);


        // lets simulate to run this so i print the dashboard data 
        return dashboards; 
    }  

} 