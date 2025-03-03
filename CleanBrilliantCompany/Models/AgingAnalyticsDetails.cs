using CleanBrilliantCompany.Interfaces; 
public class AgingAnalyticsDetails : AbstractAnalyticsDetails {
    public AgingAnalyticsDetails(int batchCode) : base(batchCode) {  } 
    public Dictionary<string, object> CalculateBatchSummary(){
        Dictionary<string, object> summary = new Dictionary<string, object>();
        foreach (IMetric metric in _metrics){
            summary.Add(metric.GetMetricName(), metric.GetValue()); 
        }
        return summary;
    } 
}
