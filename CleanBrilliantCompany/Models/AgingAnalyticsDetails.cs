public class AgingAnalyticsDetails{
    public int BatchCode { get; private set; } 
    public float TurnOverRate { get; private set; }
    public int DaysInStorage { get; private set; }
    public bool IsExpired { get; private set; }
    public StorageRiskLevel RiskLevel { get; private set; }


    private AgingDashboardRdm _parentDashboard;

    public AgingAnalyticsDetails(AgingDashboardRdm parentDashboard, int batchCode, float turnoverRate, int daysInStorage, bool isExpired, StorageRiskLevel riskLevel
    ){
        _parentDashboard = parentDashboard;
        BatchCode = batchCode;
        TurnOverRate = turnoverRate;
        DaysInStorage = daysInStorage;
        IsExpired = isExpired;
        RiskLevel = riskLevel; 
    }
}

// Enum for Storage Risk Level
public enum StorageRiskLevel
{
    Low,
    Medium,
    High
}