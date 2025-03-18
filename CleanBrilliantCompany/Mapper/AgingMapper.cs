public class AgingMapper : AgingRepo
{
    //private readonly AppDbContext? realDbContext;
    private readonly FakeDbContext? fakeDbContext;

    // 🔹 Constructor supports BOTH Fake & Real DB
    public AgingMapper(FakeDbContext? fakeDbContext = null)
    {
        //this.realDbContext = realDbContext;
        this.fakeDbContext = fakeDbContext ?? throw new ArgumentNullException(nameof(fakeDbContext)); 
    }

    // 🔹 Fetch Dashboard DTO
    public DashboardDTO? GetLatestAgingDashboard()
    {
        //Ensure `fakeDbContext` is non-null before accessing `Dashboards`
        if (fakeDbContext == null || fakeDbContext.Dashboards == null)
        return null; // Return null if no dashboards exist

        return (fakeDbContext.Dashboards) 
            .OrderByDescending(d => d.GeneratedDate)
            .Select(d => new DashboardDTO
            {
                DashboardId = d.DashboardId,
                Name = d.Name,
                RequestedStartDate = d.RequestedStartDate,
                RequestedEndDate = d.RequestedEndDate,
                GeneratedDate = d.GeneratedDate,
                ValidityDuration = d.ValidityDuration,
                Type = d.Type
            })
            .FirstOrDefault();
    }

    // 🔹 Fetch Aging Analytics DTOs
    public List<AgingAnalyticsDetailsDTO> GetAgingAnalytics(int dashboardId)
    {
        if (fakeDbContext == null || fakeDbContext.AgingAnalyticsDetails == null)
            return new List<AgingAnalyticsDetailsDTO>(); // Return empty list if no data


        return (fakeDbContext.AgingAnalyticsDetails) 
            .Where(a => a.DashboardId == dashboardId)
            .Select(a => new AgingAnalyticsDetailsDTO
            {
                AnalyticsId = a.AnalyticsId,
                BatchCode = a.BatchCode,
                DashboardId = a.DashboardId,
                DaysInStorage = a.DaysInStorage,
                IsExpired = a.IsExpired,
                RemainingDays = a.RemainingDays,
                TurnOverRate = a.TurnOverRate,
                DeadStockPercentage = a.DeadStockPercentage
            })
            .ToList();
    }

    public void saveDashboardandAnalytics(AgingDashboardRdm dashboard){
        if (fakeDbContext == null)
            return; 
        //Save the dashboard and analytics to the database 
        //convert the dashboard to a dashboardDTO 
        //convert the analytics to a list of analyticsDTO 
        //save the dashboardDTO and the list of analyticsDTO to the database/fakedbContext 
    //     var newdbDTO = new DashboardDTO{
    //         DashboardId = dashboard.DashboardId,
    //         Name = dashboard.Name,
    //         RequestedStartDate = dashboard.RequestedStartDate,
    //         RequestedEndDate = dashboard.RequestedEndDate,
    //         GeneratedDate = dashboard.GeneratedDate ?? DateTime.Now,
    //         ValidityDuration = dashboard.ValidityDuration,
    //         Type = 1 
    //     }; 

    //     fakeDbContext.Dashboards.Add(newdbDTO); 
    
//    foreach (var analytics in dashboard.getBatchAnalyticsMap())
// {
//     int batchCode = analytics.Key; // Extract Batch Code
//     var analyticsList = analytics.Value; // Get List<AbstractAnalyticsDetails>

//     // Initialize values (null for optional fields)
//     float? turnOverRate = null;
//     float? deadStockPercentage = null;
//     int? daysInStorage = null;
//     bool? isExpired = null;
//     int? remainingDays = null;

//     foreach (var analytic in analyticsList)
//     {
//         // ✅ Use CalculateBatchSummary() to get a dictionary of key-value pairs
//         var batchSummary = analytic.CalculateBatchSummary();

//         // ✅ Retrieve values dynamically using keys
//         if (batchSummary.ContainsKey("TurnOverRate"))
//             turnOverRate = Convert.ToSingle(batchSummary["TurnOverRate"]);

//         if (batchSummary.ContainsKey("DeadStockPercentage"))
//             deadStockPercentage = Convert.ToSingle(batchSummary["DeadStockPercentage"]);

//         if (batchSummary.ContainsKey("DaysInStorage"))
//             daysInStorage = Convert.ToInt32(batchSummary["DaysInStorage"]);

//         if (batchSummary.ContainsKey("IsExpired"))
//             isExpired = Convert.ToBoolean(batchSummary["IsExpired"]);

//         if (batchSummary.ContainsKey("RemainingDays"))
//             remainingDays = Convert.ToInt32(batchSummary["RemainingDays"]);
//     }

//     // ✅ Create DTO and add it to the database
//     var analyticsDTO = new AgingAnalyticsDetailsDTO
//     {
//         BatchCode = batchCode,
//         DashboardId = dashboard.DashboardId,
//         DaysInStorage = daysInStorage,
//         IsExpired = isExpired,
//         RemainingDays = remainingDays,
//         TurnOverRate = turnOverRate,
//         DeadStockPercentage = deadStockPercentage
//     };
}
}