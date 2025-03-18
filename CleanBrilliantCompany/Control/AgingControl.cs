public class AgingControl 
{
    private List<AgingDashboardRdm> dashboards; 
    //private AgingMapper agingMapper; 
    private AgingRepo agingMapper;
    public AgingControl(AgingRepo agingMapper) {
        this.agingMapper = agingMapper;
        dashboards = new List<AgingDashboardRdm>();

        // 🔹 Retrieve data from the database / fake DB
        LoadDashboards();
    }

    // 🔹 Load dashboards from the database (or fake DB)
    private void LoadDashboards()
    {
        // Step 1: Retrieve the latest dashboard DTO
        var dashboardDto = agingMapper.GetLatestAgingDashboard();
        if (dashboardDto == null)
        {
            Console.WriteLine("⚠ No dashboard found.");
            return; // No data to load
        }

        // Step 2: Retrieve analytics data for the dashboard
        var analyticsDtos = agingMapper.GetAgingAnalytics(dashboardDto.DashboardId);

        // Step 3: Create an AgingDashboardRdm and populate with analytics
        var agingDashboard = new AgingDashboardRdm(
            dashboardDto.DashboardId,
            dashboardDto.Name,
            dashboardDto.RequestedStartDate,
            dashboardDto.RequestedEndDate,
            dashboardDto.ValidityDuration,
            dashboardDto.Type,
            dashboardDto.GeneratedDate 
        );

        foreach (var analyticsDto in analyticsDtos)
        {
            var stockTurnOverDetails = new StockTurnOverAnalyticsDetails(
                analyticsDto.BatchCode,
                (float)analyticsDto.TurnOverRate, 
                (float)analyticsDto.DeadStockPercentage 
            );

            var storageLifeCycleDetails = new StorageLifeCycleAnalyticsDetails(
                analyticsDto.BatchCode,
                analyticsDto.DaysInStorage, 
                analyticsDto.IsExpired,
                analyticsDto.RemainingDays
            );

            agingDashboard.addBatchAnalytics(analyticsDto.BatchCode, stockTurnOverDetails);
            agingDashboard.addBatchAnalytics(analyticsDto.BatchCode, storageLifeCycleDetails);
        }

        // Step 4: Add the dashboard to the list
        dashboards.Add(agingDashboard);
    }

    // 🔹 Method to Retrieve the Latest Dashboard
    public AgingDashboardRdm? GetLatestDashboard()
    {
        Console.WriteLine("🔍 Retrieving the latest dashboard...");
        // print out all available dashboards 
        Console.WriteLine("Amount of dashboards: " + dashboards.Count); 
        foreach (var dashboard in dashboards)
        {
            Console.WriteLine($"Dashboard: {dashboard.GeneratedDate}");
        } 

        return dashboards.OrderByDescending(d => d.RequestedStartDate).FirstOrDefault();
    }

    // i also need a method where User wants to generate a new dashboard
    // so i have to go talk to the other team through this interface
    // so i will prolly use the function to get all available batches 
    // loop through the list, for each batchNumber 
    // i will call the interface again to get their stockhistory data
    // they returns me a dictionary of stockhistory that belongs to that batch 
    // so i assume the key will be a date and the value will be the rawstockhistorydata instance 
    // i will then create a new dashbaord instance

    public AgingDashboardRdm generateNewDashboard() 
    {

        // var dashboard = new AgingDashboardRdm("Aging Dashboard", DateTime.Now, DateTime.Now.AddDays(180), 180, 1);
        //find the max id from the list then increment it by 1 
        var id = dashboards.Max(x => x.DashboardId) + 1; 
        var dashboard = new AgingDashboardRdm(id, "Aging Dashboard new", DateTime.Now, DateTime.Now.AddDays(180), 180, 1);

        var fakeInterface = new FakeBatchInterface(); 
        var batches = fakeInterface.getAllProductBatch(); 
        foreach (var batch in batches){
            var stockHistory = fakeInterface.getStockHistoryByBatch(batch.BatchCode);
            var stockHistoryMap = stockHistory.ToDictionary(x => x.Date, x => x.Quantity);
            
            var storageLifeCycleAnalytics = new StorageLifeCycleAnalyticsDetails(
                batch.BatchCode, 
                batch.ReceiveDate, 
                batch.ExpiryDate
                );    
            var stockTurnOverAnalytics = new StockTurnOverAnalyticsDetails(
                batch.BatchCode, 
                stockHistoryMap, 
                batch.Quantity
                ); 
            dashboard.addBatchAnalytics(batch.BatchCode, storageLifeCycleAnalytics); 
            dashboard.addBatchAnalytics(batch.BatchCode, stockTurnOverAnalytics); 
        }
        // add it to the list 
        dashboards.Add(dashboard); 
        agingMapper.saveDashboardandAnalytics(dashboard); 
        return dashboard; 
    }


}
