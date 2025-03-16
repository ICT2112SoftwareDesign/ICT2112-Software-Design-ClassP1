public class AgingControl 
{
    private List<AgingDashboardRdm> dashboards; 
    private AgingMapper agingMapper; 

    public AgingControl(AgingMapper agingMapper) {
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
            dashboardDto.Name,
            dashboardDto.RequestedStartDate,
            dashboardDto.RequestedEndDate,
            dashboardDto.ValidityDuration,
            dashboardDto.Type 
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
        return dashboards.OrderByDescending(d => d.RequestedStartDate).FirstOrDefault();
    }
}
