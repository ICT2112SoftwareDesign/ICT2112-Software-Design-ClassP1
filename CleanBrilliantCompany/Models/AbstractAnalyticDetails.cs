using CleanBrilliantCompany.Interfaces; 
public abstract class AbstractAnalyticsDetails {
    protected int _batchCode; 
    protected List<IMetric> _metrics; 

    public AbstractAnalyticsDetails(int batchCode){
        _batchCode = batchCode;
        _metrics = new List<IMetric>(); 
    }

    //! ITS PUBLIC FOR NOW
    /// <summary>
    /// Gets the batch code.
    /// </summary>
    public int GetBatchCode()
    {
        return _batchCode;
    }

    /// <summary>
    /// Adds a metric to the batch.
    /// </summary>
    public void AddMetric(IMetric metric){
        _metrics.Add(metric); 
    } 
    public IMetric? GetMetric(string metricName){
        return _metrics.FirstOrDefault(m => m.GetMetricName() == metricName); 
    } 
    public List<IMetric> GetAllMetrics() {
        return _metrics; 
    } 
}