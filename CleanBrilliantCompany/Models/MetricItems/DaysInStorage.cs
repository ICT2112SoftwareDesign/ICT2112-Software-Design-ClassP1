using CleanBrilliantCompany.Interfaces;

public class DaysInStorage : IMetric {
    private DateTime _receivedDate; 
    public DaysInStorage(DateTime receivedDate){
        _receivedDate = receivedDate;
    } 
    public string GetMetricName () => "DaysInStorage";  

    public object GetValue(){
        return (DateTime.Now - _receivedDate).Days;
    } 
}