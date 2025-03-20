public class AgingMapper1 : AgingRepo
{
    //private readonly AppDbContext? realDbContext;
    private readonly FakeDbContext? fakeDbContext;

    // 🔹 Constructor supports BOTH Fake & Real DB
    public AgingMapper1(FakeDbContext? fakeDbContext = null)
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
                DeadStockPercentage = a.DeadStockPercentage,
                ProductId = a.ProductId 
            })
            .ToList();
    }

    public void saveDashboardandAnalytics(AgingDashboardRdm dashboard){
        if (fakeDbContext == null)
            return; 
        // Save the dashboard and analytics to the database 
        // convert the dashboard to a dashboardDTO 
        // convert the analytics to a list of analyticsDTO 
        // save the dashboardDTO and the list of analyticsDTO to the database/fakedbContext 
        var newdbDTO = new DashboardDTO{
            DashboardId = dashboard.DashboardId,
            Name = dashboard.Name,
            RequestedStartDate = dashboard.RequestedStartDate,
            RequestedEndDate = dashboard.RequestedEndDate,
            GeneratedDate = dashboard.GeneratedDate ?? DateTime.Now,
            ValidityDuration = dashboard.ValidityDuration,
            Type = 1 
        }; 

        fakeDbContext.Dashboards.Add(newdbDTO); 

        // Loop through the productToBatchMap
        foreach (var productBatch in dashboard.getProductToBatchMap())
        {
            int productId = productBatch.Key; // Get Product ID
            List<int> batchCodes = productBatch.Value; // Get the list of batch codes for this product

            foreach (var batchCode in batchCodes)
            {
                // Get the batch analytics for each batch code
                var analyticsList = dashboard.GetBatchAnalytics(batchCode); 

                // Initialize default values for the analytics summary
                float turnOverRate = 0; // Default turnover rate
                float deadStockPercentage = 0; // Default dead stock percentage
                int daysInStorage = 0; // Default days in storage
                bool isExpired = false; // Default expired status
                int remainingDays = 0; // Default remaining days

                foreach (var analytics in analyticsList)
                {
                    // Get batch summary from CalculateBatchSummary
                    var batchSummary = analytics.CalculateBatchSummary();

                    // Check for each key in the batch summary and set the respective values
                    if (batchSummary.ContainsKey("TurnOverRate"))
                        turnOverRate = Convert.ToSingle(batchSummary["TurnOverRate"]);

                    if (batchSummary.ContainsKey("DeadStockPercentage"))
                        deadStockPercentage = Convert.ToSingle(batchSummary["DeadStockPercentage"]);

                    if (batchSummary.ContainsKey("DaysInStorage"))
                        daysInStorage = Convert.ToInt32(batchSummary["DaysInStorage"]);

                    if (batchSummary.ContainsKey("IsExpired"))
                        isExpired = Convert.ToBoolean(batchSummary["IsExpired"]);

                    if (batchSummary.ContainsKey("RemainingDays"))
                        remainingDays = Convert.ToInt32(batchSummary["RemainingDays"]);
                }

                // Create a DTO for analytics details and save it to the database
                var analyticsDTO = new AgingAnalyticsDetailsDTO
                {
                    BatchCode = batchCode,
                    DashboardId = dashboard.DashboardId,
                    ProductId = productId,  // Save the Product ID
                    DaysInStorage = daysInStorage,
                    IsExpired = isExpired,
                    RemainingDays = remainingDays,
                    TurnOverRate = turnOverRate,
                    DeadStockPercentage = deadStockPercentage
                };

                fakeDbContext.AgingAnalyticsDetails.Add(analyticsDTO);
            };
        }
    }
};

