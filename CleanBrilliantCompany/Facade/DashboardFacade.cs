public class DashboardFacade : IDashboardFacade
{
    private readonly AgingControl agingControl;

    public DashboardFacade(
        AgingControl agingControl
        )
    {
        this.agingControl = agingControl;
    }

    public List<Dashboard> getDashboardsData()
    {
        // this thing just calls every dashboard's getDashboardData method 
        List<Dashboard> dashboards = new List<Dashboard>();

        // go to control class and get the dashboard from aging 
        Dashboard agingDashboard = agingControl.GetLatestDashboard();
        dashboards.Add(agingDashboard);


        // lets simulate to run this so i print the dashboard data 
        return dashboards; 
    }  

} 