using System.Collections.Generic;
using CleanBrilliantCompany.Models.Entity;

public class AgingDashboardRdm : Dashboard
{


    // batchCode to list of analytics 
    private Dictionary<int, List<AbstractAnalyticsDetails>> batchAnalyticsMap;

    // productID to list of batchCode 
    private Dictionary<int, List<int>> productToBatchMap = new Dictionary<int, List<int>>();

    // productID to productName 
    private Dictionary<int, String> productIDToNameMap = new Dictionary<int, string>();

    // this is for db 
    public AgingDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
        : base(id, name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        //_batchAnalyticsList = new List<AbstractAnalyticsDetails>();
        batchAnalyticsMap = new Dictionary<int, List<AbstractAnalyticsDetails>>();
    }

    public Dictionary<int, List<int>> GetProductToBatchMap() => productToBatchMap;
    public void AddBatchtoProductMap(int productId, int batchCode)
    {

        //Console.WriteLine("Adding batch {0} to product {1}", batchCode, productId); 
        if (productToBatchMap.ContainsKey(productId))
        {
            productToBatchMap[productId].Add(batchCode);
        }
        else
        {
            productToBatchMap.Add(productId, new List<int> { batchCode });
        }
    }

    public Dictionary<int, string> GetProductIDToNameMap() => productIDToNameMap;

    public void AddProductToNameMap(int productId, string productName)
    {
        if (!productIDToNameMap.ContainsKey(productId))
        {
            productIDToNameMap.Add(productId, productName);
        }
    }

    public void PopulateAnalytics(List<ProductBatch> productBatchData, List<StockHistory> stockHistoryData) 
    {
                // so for each rawanalyticsdata i need to create 2 instances since 
        // i have 2 types of agingAnalytics

        // convert stockHistory to a dictionary first 
        Dictionary<int , Dictionary<DateOnly, int>> stockHistoryMap = stockHistoryData
        .Select( s => {
            var info = s.retrieveStockHistory();
            return new 
            {
                batchCode = (int)info["BatchCode"],
                stockTakeDate = (DateOnly)info["StockTakeDate"],
                quantity = (int)info["Quantity"] 
            };
        })
        .GroupBy(x => x.batchCode) 
        .ToDictionary(
            g => g.Key, 
            g => g.ToDictionary(x => x.stockTakeDate, x => x.quantity) 
        );
        //step 2 : loop through the rawBatchData and create the analytics 
        foreach(var batch in productBatchData)
        {
            var info = batch.retrieveProductBatchInfo(); 
            int batchCode = (int)info["BatchCode"]; 
            int productId = (int)info["ProductId"]; 
            DateTime receiveDate = (DateTime)info["ReceiveDate"]; 
            DateTime expiryDate = (DateTime)info["ExpiryDate"]; 
            int quantity = (int)info["Quantity"]; 

            // ensure batch code is in the analytics map 
            if (!batchAnalyticsMap.ContainsKey(batchCode))
            {
                batchAnalyticsMap.Add(batchCode, new List<AbstractAnalyticsDetails>());
            } 

            // add the batch to the product map 
            addBatchtoProductMap(productId, batchCode); 

            // create StorageLifeCycleAanyltics 
            var storageLifeCycleAnalytics = new StorageLifeCycleAnalyticsDetails(
                batchCode, 
                receiveDate, 
                expiryDate); 
            
            // Create StockTurnOverAnalytics 
            var stockTurnOverAnalytics = new StockTurnOverAnalyticsDetails(
                batchCode, 
                quantity); 

            // set the quantity per day for the stockTurnOverAnalytics
            if (stockHistoryMap.ContainsKey(batchCode))
            {
                stockTurnOverAnalytics.setQuantityPerDay(stockHistoryMap[batchCode]);
            }
            
            // add them to analytics map
            AddBatchAnalytics(batchCode, storageLifeCycleAnalytics); 
            AddBatchAnalytics(batchCode, stockTurnOverAnalytics); 
        }
    }




    public void AddBatchAnalytics(int batchCode, AbstractAnalyticsDetails batchDetails)
    {
        if (batchAnalyticsMap.ContainsKey(batchCode))
        {
            batchAnalyticsMap[batchCode].Add(batchDetails);
        }
        else
        {
            batchAnalyticsMap.Add(batchCode, new List<AbstractAnalyticsDetails> { batchDetails });
        }
    }


    public List<AbstractAnalyticsDetails>? GetBatchAnalytics(int batchCode)
    {
        return batchAnalyticsMap.ContainsKey(batchCode) ? batchAnalyticsMap[batchCode] : null;
    }



}