using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;

public class AgingControl : IStorageDuration
{
    private Dashboard agingDashboard;
    //private AgingMapper agingMapper; 
    private AgingRepo agingMapper;


    //private fakebatchinterface 
   //!private FakeBatchInterface fakeBatchInterface;

    //private fakeProductInterface 
    //!private FakeProductInterface fakeProductInterface;

    private IProduct fakeProductInterface;
    private IBatch fakeBatchInterface; 

    public AgingControl(
        AgingRepo agingMapper,
        //!FakeBatchInterface fakeBatchInterface,
        //!FakeProductInterface fakeProductInterface
        IProduct fakeProductInterface,
        IBatch fakeBatchInterface
        )
    {
        this.agingMapper = agingMapper;
        // dashboards = new List<AgingDashboardRdm>();
        this.fakeBatchInterface = fakeBatchInterface;
        this.fakeProductInterface = fakeProductInterface;
        // 🔹 Retrieve data from the database / fake DB
        LoadDashboards();
        //getStorageDuration(1);
    }

    // 🔹 Load dashboards from the database (or fake DB)
    private void LoadDashboards()
    {
        // step 0: init the fake interface 
        //var fakeInterface = new FakeProductInterface(); 

        // Step 1: Retrieve the latest dashboard DTO
        var dashboardDto = agingMapper.GetLatestAgingDashboard();
        if (dashboardDto == null)
        {
            Console.WriteLine("⚠ No dashboard found.");
            return; // No data to load
        }
        Console.WriteLine($"📊 Dashboard found: {dashboardDto.Name} generated on : {dashboardDto.GeneratedDate}");

        // Step 2: Retrieve analytics data for the dashboard
        var analyticsDtos = agingMapper.GetAgingAnalytics(dashboardDto.DashboardId);


        // step 2.5 ? maybe i group the analytics by ProductID first
        // so for each product i can make use of the interface to get the product details 
        var productToAnalyticsMap = analyticsDtos.GroupBy(x => x.ProductId)
            .ToDictionary(
                group => group.Key,
                group => group.ToList()
            );

        // Step 3: Create an AgingDashboardRdm and populate with analytics
        agingDashboard = DashboardFactory.createDashboard(dashboardDto);

        if (agingDashboard == null)
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
            //!RawProductData? productData = fakeProductInterface.getProductDetails(productID);
            Product productData = fakeProductInterface.getProductDetails(productID); 


            if (productData == null)
            {
                Console.WriteLine("⚠ Product not found.");
                continue;
            }
            // add this product to the dashboard
            //! (agingDashboard as AgingDashboardRdm).addProductToNameMap(productID, productData.productName);
            (agingDashboard as AgingDashboardRdm).addProductToNameMap(productID, productData.retrieveProductInfo()["ProductName"].ToString());


            // loop through the analyticsList and create the analytics 
            foreach (var analyticsDto in analyticsList)
            {
                // add this batchID to the list of batch for the productID in the dashboard
                (agingDashboard as AgingDashboardRdm).addBatchtoProductMap(productID, analyticsDto.BatchCode);

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

                (agingDashboard as AgingDashboardRdm).addBatchAnalytics(analyticsDto.BatchCode, stockTurnOverDetails);
                (agingDashboard as AgingDashboardRdm).addBatchAnalytics(analyticsDto.BatchCode, storageLifeCycleDetails);
            }
        }

        // Step 4: Add the dashboard to the list
        //dashboards.Add((agingDashboard as AgingDashboardRdm));
    }

    // 🔹 Method to Retrieve the Latest Dashboard
    public AgingDashboardRdm? GetLatestDashboard()
    {
        Console.WriteLine("🔍 Retrieving the latest dashboard...");
        // print out all available dashboards 
        //Console.WriteLine("Amount of dashboards: " + dashboards.Count); 
        return agingDashboard as AgingDashboardRdm;
    }

    // i also need a method where User wants to generate a new dashboard
    // so i have to go talk to the other team through this interface
    // so i will prolly use the function to get all available batches 
    // loop through the list, for each batchNumber 
    // i will call the interface again to get their stockhistory data
    // they returns me a dictionary of stockhistory that belongs to that batch 
    // so i assume the key will be a date and the value will be the rawstockhistorydata instance 
    // i will then create a new dashbaord instance

    public void generateNewDashboard(DashboardDTO dto)
    {
        dto.Type = 1;
        var dashboard = DashboardFactory.createDashboard(dto);
        //var fakeInterface = new FakeBatchInterface(); 
        var batches = fakeBatchInterface.getAllProductBatch();
        // using the dashboard's requestedStartDate and requestedEndDate
        // i will filter out the batches that are within the date range using the batch's receive date 

        //! var filteredBatches = batches.Where(b => b.ReceiveDate >= dashboard.RequestedStartDate && b.ReceiveDate <= dashboard.RequestedEndDate).ToList();
        var filteredBatches = batches
        .Where(b =>
        {
            var info = b.retrieveProductBatchInfo();
            var receiveDate = (DateTime)info["ReceiveDate"];
            return receiveDate >= dashboard.RequestedStartDate &&
                receiveDate <= dashboard.RequestedEndDate;
        }).ToList();
        //! ================================================================

        // Retrieve all stock histories for all batches
        //! var stockHistories = new List<RawStockHistoryData>();
        var stockHistories = new List<StockHistory>(); // Use the correct type for stock histories 

        foreach (var batch in filteredBatches)
        {
            //!var stockHistory = fakeBatchInterface.getStockHistoryByBatch(batch.BatchCode);
            
            //!stockHistories.AddRange(stockHistory);  // Efficiently add all records at once
            var stockHistory = fakeBatchInterface.getStockHistoryByBatch((int)batch.retrieveProductBatchInfo()["BatchCode"]);
        }

        // Use the existing populateAnalytics method
        (dashboard as AgingDashboardRdm).populateAnalytics(filteredBatches, stockHistories);

        // Add to the list and save 
        agingMapper.saveDashboardandAnalytics(dashboard as AgingDashboardRdm);
    }


    public int getStorageDuration(int batchCode)
    {
        // i need the batch's receive data then use current time to minus the receive date 

        // RawBatchData batch = fakeBatchInterface.getBatchDetails(batchCode);
        // if (batch == null)
        // {
        //     Console.WriteLine("⚠ Batch not found.");
        //     return -1;
        // }
        // DateTime currentDate = DateTime.Now;
        // TimeSpan storageDuration = currentDate - batch.ReceiveDate;
        // Console.WriteLine("Storage duration for batch {0} is {1} days", batchCode, storageDuration.Days);
        // return storageDuration.Days;
        return 0 ; 
    }
    public string GenerateReport()
    {
        var dashboard = GetLatestDashboard();
        var report = new System.Text.StringBuilder();

        report.AppendLine($"<h1>Aging Report - {dashboard.Name}</h1>");
        report.AppendLine($"<p>Generated: {dashboard.GeneratedDate}</p>");
        report.AppendLine("<hr/>");

        var productMap = dashboard.getProductToBatchMap();
        var productNameMap = dashboard.getProductIDToNameMap();


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
                    report.AppendLine($"<li>{analytics.getAnalyticsType()}</li>");
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
