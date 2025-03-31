public abstract class AbstractAnalyticsDetails {
    protected int batchCode; 
    protected String analyticsType;

    public AbstractAnalyticsDetails(int batchCode, String analyticsType) { 
        this.batchCode = batchCode;
        this.analyticsType = analyticsType ;
    }

    public int getBatchCode() => batchCode; 
    public String getAnalyticsType() => analyticsType; 
    abstract public Dictionary<string, object> CalculateBatchSummary(); 
    
} 