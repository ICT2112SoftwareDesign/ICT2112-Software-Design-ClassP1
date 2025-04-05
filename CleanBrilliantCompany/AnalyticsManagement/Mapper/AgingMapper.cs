public class AgingMapper : IAgingRepository
{
    private readonly ApplicationDbContext _dbContext;  // Changed from _realDbContext

    // Constructor with renamed parameter
    public AgingMapper(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext)); 
    }

    // Fetch Dashboard and Analytics
    public DashboardDTO? GetLatestAgingDashboard()
    {
        return _dbContext.Dashboards
            .Where(d => d.TypeId == 1) // Assuming TypeId 1 is for Aging Dashboard 
            .OrderByDescending(d => d.GeneratedDate)
            .Select(d => new DashboardDTO
            {
                DashboardId = d.DashboardId,
                Name = d.Name,
                RequestedStartDate = d.RequestedStartDate,
                RequestedEndDate = d.RequestedEndDate,
                GeneratedDate = d.GeneratedDate,
                ValidityDuration = d.ValidityDuration,
                Type = d.TypeId
            })
            .FirstOrDefault();
    }

    public List<AgingAnalyticsDetailsDTO> GetAgingAnalytics(int dashboardId)
    {
        return _dbContext.AgingAnalyticsDetails
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
    public void saveDashboardandAnalytics(AgingDashboardRdm dashboard)
    {
        if (_dbContext == null)
            return;

        // Create the DashboardTable (Entity) instead of DashboardDTO
        var newDbTable = new DashboardTable
        {
            //DashboardId = dashboard.DashboardId,
            Name = dashboard.Name,
            RequestedStartDate = dashboard.RequestedStartDate,
            RequestedEndDate = dashboard.RequestedEndDate,
            GeneratedDate = dashboard.GeneratedDate ?? DateTime.Now,
            ValidityDuration = dashboard.ValidityDuration,
            TypeId = 1 // Or other type value based on your logic
        };

        // Add the Dashboard entity to the DbContext
        _dbContext.Dashboards.Add(newDbTable);
        // save 
        _dbContext.SaveChanges(); 

        // Loop through the productToBatchMap
        foreach (var productBatch in dashboard.GetProductToBatchMap())
        {
            int productId = productBatch.Key; // Get Product ID
            List<int> batchCodes = productBatch.Value; // Get the list of batch codes for this product

            foreach (var batchCode in batchCodes)
            {
                // Get the batch analytics for each batch code
                var analyticsList = dashboard.GetBatchAnalytics(batchCode);

                // Initialize default values for the analytics summary
                float turnOverRate = 0;
                float deadStockPercentage = 0;
                int daysInStorage = 0;
                bool isExpired = false;
                int remainingDays = 0;

                foreach (var analytics in analyticsList)
                {
                    // Get batch summary from CalculateBatchSummary
                    var batchSummary = analytics.CalculateBatchSummary();

                    // Check for each key in the batch summary and set the respective values
                    if (batchSummary.ContainsKey("TurnOverRate"))
                        turnOverRate = Convert.ToSingle(batchSummary["TurnOverRate"]);

                    if (batchSummary.ContainsKey("DeadStockPercentage"))
                        deadStockPercentage = Convert.ToSingle(batchSummary["DeadStockPercentage"]);

                    if (batchSummary.ContainsKey("StorageDuration"))
                        daysInStorage = Convert.ToInt32(batchSummary["StorageDuration"]);

                    if (batchSummary.ContainsKey("ExpiryStatus")){
                        Console.WriteLine("Batch Code: {0} is expired: {1}", batchCode, batchSummary["ExpiryStatus"]);
                        isExpired = Convert.ToBoolean(batchSummary["ExpiryStatus"]);
                        Console.WriteLine("after conversion: {0}", isExpired); 
                    }
                        
                    if (batchSummary.ContainsKey("RemainingDays"))
                        remainingDays = Convert.ToInt32(batchSummary["RemainingDays"]);
                }

                // Create a DTO for analytics details and save it to the database
                var analyticsTable = new AgingAnalyticsDetailsTable
                {
                    BatchCode = batchCode,
                    DashboardId = newDbTable.DashboardId,
                    ProductId = productId,  // Save the Product ID
                    DaysInStorage = daysInStorage,
                    IsExpired = isExpired,
                    RemainingDays = remainingDays,
                    TurnOverRate = turnOverRate,
                    DeadStockPercentage = deadStockPercentage
                };

                _dbContext.AgingAnalyticsDetails.Add(analyticsTable);
            };
        }

        // Save changes to the real database
        _dbContext.SaveChanges();
    }
}