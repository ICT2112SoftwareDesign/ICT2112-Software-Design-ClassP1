using System; 

public class StorageLifeCycleAnalyticsDetails : AbstractAnalyticsDetails {
    private DateTime receiveDate; 
    private DateTime expiryDate; 


    public StorageLifeCycleAnalyticsDetails(int batchCode, DateTime receiveDate, DateTime expiryDate) : base(batchCode, "StorageLifeCycleAnalyticsDetails"){
        this.receiveDate = receiveDate; 
        this.expiryDate = expiryDate; 
    } 

    public int calculateStorageDuration(){
        DateTime currentDate = DateTime.Now; 
        TimeSpan storageDuration = currentDate - receiveDate; 
        return storageDuration.Days; 
    } 

    public int calculateRemainingDays(){
        DateTime currentDate = DateTime.Now; 
        TimeSpan remainingDays = expiryDate - currentDate; 
        return remainingDays.Days; 
    } 

    public bool checkExpiryStatus(){
        return DateTime.Now > expiryDate; 
    } 

    public override Dictionary<string, object> CalculateBatchSummary(){
        Dictionary<string, object> batchSummary = new Dictionary<string, object>(); 
        batchSummary.Add("StorageDuration", calculateStorageDuration()); 
        batchSummary.Add("RemainingDays", calculateRemainingDays()); 
        batchSummary.Add("ExpiryStatus", checkExpiryStatus()); 
        return batchSummary; 
    }   
    
}