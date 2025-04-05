using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;

public class AgingControl : IStorageDuration
{
    private Dashboard _agingDashboard;
    private IAgingRepository _agingMapper;
    private IProduct _productInterface;
    private IBatch _batchInterface; 

    public AgingControl(IAgingRepository agingMapper, IProduct productInterface, IBatch batchInterface)
    {
        this._agingMapper = agingMapper;
        this._batchInterface = batchInterface;
        this._productInterface = productInterface;
        // 🔹 Retrieve data from the database / fake DB
        LoadDashboards();
    }

    // 🔹 Load dashboards from the database (or fake DB)
    private void LoadDashboards()
    {
        // Step 1: Retrieve the latest dashboard DTO
        var dashboardDto = _agingMapper.GetLatestAgingDashboard();
        if (dashboardDto == null)
        {
            Console.WriteLine("⚠ No dashboard found.");
            return; // No data to load
        }
        Console.WriteLine($"📊 Dashboard found: {dashboardDto.Name} generated on : {dashboardDto.GeneratedDate}");

        // Step 2: Retrieve analytics data for the dashboard
        var analyticsDtos = _agingMapper.GetAgingAnalytics(dashboardDto.DashboardId);

        // step 2.5 ? maybe i group the analytics by ProductID first
        // so for each product i can make use of the interface to get the product details 
        var productToAnalyticsMap = analyticsDtos.GroupBy(x => x.ProductId)
            .ToDictionary(
                group => group.Key,
                group => group.ToList()
            );

        // Step 3: Create an AgingDashboardRdm and populate with analytics
        _agingDashboard = DashboardFactory.CreateDashboard(dashboardDto);

        if (_agingDashboard == null)
        {
            Console.WriteLine("⚠ Dashboard could not be created.");
            return;
        }

        // Loop through the productToAnalyticsMap 
        foreach (var product in productToAnalyticsMap)
        {
            var productID = product.Key;
            var analyticsList = product.Value;
            // get the product details 
            Product productData = _productInterface.getProductDetails(productID); 

            if (productData == null)
            {
                Console.WriteLine("⚠ Product not found.");
                continue;
            }
            // add this product to the dashboard
            (_agingDashboard as AgingDashboardRdm).AddProductToNameMap(productID, productData.retrieveProductInfo()["ProductName"].ToString());

            // loop through the analyticsList and create the analytics 
            foreach (var analyticsDto in analyticsList)
            {
                // add this batchID to the list of batch for the productID in the dashboard
                (_agingDashboard as AgingDashboardRdm).AddBatchtoProductMap(productID, analyticsDto.BatchCode);

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

                (_agingDashboard as AgingDashboardRdm).AddBatchAnalytics(analyticsDto.BatchCode, stockTurnOverDetails);
                (_agingDashboard as AgingDashboardRdm).AddBatchAnalytics(analyticsDto.BatchCode, storageLifeCycleDetails);
            }
        }
    }

    // 🔹 Method to Retrieve the Latest Dashboard
    public AgingDashboardRdm? GetLatestDashboard()
    {
        Console.WriteLine("🔍 Retrieving the latest dashboard...");
        // print out all available dashboards 
        //Console.WriteLine("Amount of dashboards: " + dashboards.Count); 
        return _agingDashboard as AgingDashboardRdm;
    }

    public void GenerateNewDashboard(DashboardDTO dto)
    {
        dto.Type = 1;
        var dashboard = DashboardFactory.CreateDashboard(dto);
        var batches = _batchInterface.getAllProductBatch();
        // using the dashboard's requestedStartDate and requestedEndDate
        // i will filter out the batches that are within the date range using the batch's receive date 

        var filteredBatches = batches
        .Where(b =>
        {
            var info = b.retrieveProductBatchInfo();
            var receiveDate = (DateTime)info["ReceiveDate"];
            return receiveDate >= dashboard.requestedStartDate &&
                receiveDate <= dashboard.requestedEndDate;
        }).ToList();
        
        // Retrieve all stock histories for all batches
        var stockHistories = new List<StockHistory>(); // Use the correct type for stock histories 

        foreach (var batch in filteredBatches)
        {
            Console.WriteLine($"Fetching stock history for batch {batch.retrieveProductBatchInfo()["BatchCode"]}");
            var stockHistory = _batchInterface.getStockHistoryByBatch((int)batch.retrieveProductBatchInfo()["BatchCode"]);
            Console.WriteLine($"Found {stockHistory.Count} stock records.");
            if (stockHistory != null)
            {
                stockHistories.AddRange(stockHistory);
            } 
        }

        // Use the existing populateAnalytics method
        (dashboard as AgingDashboardRdm).PopulateAnalytics(filteredBatches, stockHistories);

        // Add to the list and save 
        _agingMapper.saveDashboardandAnalytics(dashboard as AgingDashboardRdm);
    }

    public int GetStorageDuration(int batchCode)
    {
        ProductBatch batch = _batchInterface.getBatchDetails(batchCode); 
        if (batch == null)
        {
            Console.WriteLine("⚠ Batch not found.");
            return -1;
        } 
        DateTime currentDate = DateTime.Now; 
        DateTime receiveDate = (DateTime)batch.retrieveProductBatchInfo()["ReceiveDate"]; 
        TimeSpan storageDuration = currentDate - receiveDate; 
        Console.WriteLine("Storage duration for batch {0} is {1} days", batchCode, storageDuration.Days); 
        return storageDuration.Days; 
    }

    public string GenerateReport()
    {
        var dashboard = GetLatestDashboard();
        var report = new System.Text.StringBuilder();

        report.AppendLine($"<h1>Aging Report - {dashboard.name}</h1>");
        report.AppendLine($"<p>Generated: {dashboard.generatedDate}</p>");
        report.AppendLine("<hr/>");

        var productMap = dashboard.GetProductToBatchMap();
        var productNameMap = dashboard.GetProductIDToNameMap();

        foreach (var entry in productMap)
        {
            int productId = entry.Key;
            List<int> batchCodes = entry.Value;

            string productName = productNameMap.ContainsKey(productId) ? productNameMap[productId] : $"Product {productId}";

            report.AppendLine($"<h2>{productName} (ID: {productId})</h2>");
            report.AppendLine("<ul>");

            foreach (int batchCode in batchCodes)
            {
                var analyticsList = dashboard.GetBatchAnalytics(batchCode);
                if (analyticsList == null || analyticsList.Count == 0) continue;

                report.AppendLine($"<li><strong>Batch {batchCode}</strong><ul>");

                foreach (var analytics in analyticsList)
                {
                    var summary = analytics.CalculateBatchSummary();
                    report.AppendLine($"<li>{analytics.GetAnalyticsType()}</li>");
                    report.AppendLine("<ul>");
                    foreach (var kvp in summary)
                    {
                        report.AppendLine($"<li>{kvp.Key}: {kvp.Value}</li>");
                    }
                    report.AppendLine("</ul>");
                }

                report.AppendLine("</ul></li>");
            }

            report.AppendLine("</ul>");
        }

        Console.WriteLine("Aging Report generated.");
        return report.ToString();
    }
}