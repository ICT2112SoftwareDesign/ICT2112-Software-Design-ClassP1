using System.Collections.Generic;

public class AgingDashboardRdm : Dashboard
{


    private Dictionary<int , List <AbstractAnalyticsDetails>> batchAnalyticsMap;   

    public AgingDashboardRdm(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type)
        : base(name, requestedStartDate, requestedEndDate, validityDuration, type)
    {
        //_batchAnalyticsList = new List<AbstractAnalyticsDetails>();
        batchAnalyticsMap = new Dictionary<int, List<AbstractAnalyticsDetails>>();
    }

    public Dictionary<int, List<AbstractAnalyticsDetails>> getBatchAnalyticsMap() => batchAnalyticsMap; 
    private void setBatchAnalyticsMap(Dictionary<int, List<AbstractAnalyticsDetails>> batchAnalyticsMap) => this.batchAnalyticsMap = batchAnalyticsMap;
    
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

            // ensure that the batchCode is in the analytics map 
            if (!batchAnalyticsMap.ContainsKey(batchCode)){
                batchAnalyticsMap.Add(batchCode, new List<AbstractAnalyticsDetails>()); 
            } 

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