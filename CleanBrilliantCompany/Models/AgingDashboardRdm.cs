using System.Collections.Generic;

public class AgingDashboardRdm : Dashboard
{


    private Dictionary<int , List <AbstractAnalyticsDetails>> batchAnalyticsMap;   
    //private Dictionary<int, int> batchToProductMap = new Dictionary<int, int>();

    private Dictionary <int, List<int>> productToBatchMap = new Dictionary<int, List<int>>();

    // this is for db 
    public AgingDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
        : base(id ,name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        //_batchAnalyticsList = new List<AbstractAnalyticsDetails>();
        batchAnalyticsMap = new Dictionary<int, List<AbstractAnalyticsDetails>>();
    }

    // this is for when i am creating 1? 
    public AgingDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type)
        : base(id, name, requestedStartDate, requestedEndDate, validityDuration, type)
    {
        //_batchAnalyticsList = new List<AbstractAnalyticsDetails>();
        batchAnalyticsMap = new Dictionary<int, List<AbstractAnalyticsDetails>>();
    }
  


    public Dictionary<int, List<AbstractAnalyticsDetails>> getBatchAnalyticsMap() => batchAnalyticsMap; 
    private void setBatchAnalyticsMap(Dictionary<int, List<AbstractAnalyticsDetails>> batchAnalyticsMap) => this.batchAnalyticsMap = batchAnalyticsMap;
    
    public Dictionary<int, List<int>> getProductToBatchMap() => productToBatchMap; 
    public void addBatchtoProductMap(int productId, int batchCode){
        
        Console.WriteLine("Adding batch {0} to product {1}", batchCode, productId); 
        if (productToBatchMap.ContainsKey(productId)){
            productToBatchMap[productId].Add(batchCode); 
        } else {
            productToBatchMap.Add(productId, new List<int> {batchCode}); 
        }
    }
    public void populateAnalytics(List<RawBatchData> rawBatchData, List<RawStockHistoryData> rawStockHistoryData) {
        // so for each rawanalyticsdata i need to create 2 instances since 
        // i have 2 types of agingAnalytics

        // convert stockHistory to a dictionary first 
        Dictionary<int, Dictionary<DateTime, int>> stockHistoryMap = rawStockHistoryData
        .GroupBy(x => x.BatchCode)  
        .ToDictionary(
            //batchCode as the dictionary key
            group => group.Key, 
            //each groups get converted to a dictionary
            group => group.ToDictionary(x => x.Date, x => x.Quantity)
            ); 
        
        //step 2 : loop through the rawBatchData and create the analytics 
        foreach (var rawBatch in rawBatchData){
            int batchCode = rawBatch.BatchCode; 
            int productId = rawBatch.ProductId;
            
            // ensure that the batchCode is in the analytics map 
            if (!batchAnalyticsMap.ContainsKey(batchCode)){
                batchAnalyticsMap.Add(batchCode, new List<AbstractAnalyticsDetails>()); 
            } 

            // check the batch for its product code, then see if it exist in the productToBatchMap, if yes add to it if not create a new entry 
            addBatchtoProductMap(productId, batchCode); 

            //create storage lifecycle Analytics 
            var storageLifeCycleAnalytics = new StorageLifeCycleAnalyticsDetails(
                batchCode, 
                rawBatch.ReceiveDate, 
                rawBatch.ExpiryDate); 

            // create stock turnover analytics 
            var stockTurnOverAnalytics = new StockTurnOverAnalyticsDetails(
                batchCode, 
                stockHistoryMap[batchCode], 
                rawBatch.Quantity); 

            // add the analytics to the batchAnalyticsMap 
            addBatchAnalytics(batchCode, storageLifeCycleAnalytics);
            addBatchAnalytics(batchCode, stockTurnOverAnalytics); 
        }
    }




    public void addBatchAnalytics(int batchCode, AbstractAnalyticsDetails batchDetails)
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

    // public List<int> getExpiringProducts
    // getStorageDurationForBatch()


}