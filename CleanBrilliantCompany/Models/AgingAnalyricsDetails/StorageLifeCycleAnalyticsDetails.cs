using System; 

public class StorageLifeCycleAnalyticsDetails : AbstractAnalyticsDetails {
    private DateTime receiveDate; 
    private DateTime expiryDate; 


    // from the database 
    private int daysInStorage = -1 ; 
    private bool isExpired; 
    private int remainingDays = -1; 



    public StorageLifeCycleAnalyticsDetails(int batchCode, DateTime receiveDate, DateTime expiryDate) : base(batchCode, "StorageLifeCycleAnalyticsDetails"){
        this.receiveDate = receiveDate; 
        this.expiryDate = expiryDate; 
    } 

    //constructor for retrieving from the database 
    public StorageLifeCycleAnalyticsDetails(int batchCode, int daysInStorage, bool isExpired, int remainingDays) : base(batchCode, "StorageLifeCycleAnalyticsDetails"){
        this.daysInStorage = daysInStorage; 
        this.isExpired = isExpired; 
        this.remainingDays = remainingDays; 
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
        // batchSummary.Add("StorageDuration", calculateStorageDuration()); 
        // batchSummary.Add("RemainingDays", calculateRemainingDays()); 
        // batchSummary.Add("ExpiryStatus", checkExpiryStatus()); 
        //check if daysInStorage, isExpired and remainingDays are -1 
        if (daysInStorage == -1 && remainingDays == -1){
            batchSummary.Add("StorageDuration", calculateStorageDuration()); 
            batchSummary.Add("RemainingDays", calculateRemainingDays()); 
            batchSummary.Add("ExpiryStatus", checkExpiryStatus()); 
        } else {
            batchSummary.Add("StorageDuration", daysInStorage); 
            batchSummary.Add("RemainingDays", remainingDays); 
            batchSummary.Add("ExpiryStatus", isExpired); 
        }
        return batchSummary; 
    }   
    
}