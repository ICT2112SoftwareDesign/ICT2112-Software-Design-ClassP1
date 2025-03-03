using CleanBrilliantCompany.Interfaces; 
public abstract class AbstractAnalyticsDetails {
    protected int _batchCode; 
    protected List<IMetric> _metrics; 

    public AbstractAnalyticsDetails(int batchCode){
        _batchCode = batchCode;
        _metrics = new List<IMetric>(); 
    }
}