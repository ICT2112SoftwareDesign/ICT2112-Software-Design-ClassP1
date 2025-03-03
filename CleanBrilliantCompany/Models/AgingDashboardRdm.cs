public class AgingDashboardRdm : Dashboard
{
    private List<AbstractAnalyticsDetails> _batchAnalyticsList;  
    public AgingDashboardRdm(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
        : base(name, requestedStartDate, requestedEndDate, validityDuration)
    {
        _batchAnalyticsList = new List<AbstractAnalyticsDetails>();
    }

     // ✅ Implementing the abstract method from Dashboard
    public override Dashboard CreateNewDashboard(string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration)
    {
        return new AgingDashboardRdm(name, requestedStartDate, requestedEndDate, validityDuration);
    }

    public void AddBatchAnalytics(AbstractAnalyticsDetails batchDetails)
    {
        _batchAnalyticsList.Add(batchDetails);
    }

    public List<AbstractAnalyticsDetails> GetAllBatchAnalytics()
    {
        return _batchAnalyticsList;
    }

    // public AbstractAnalyticsDetails? GetBatchAnalytics(int batchCode)
    // {
    //     return _batchAnalyticsList.Find(batch => batch.BatchCode == batchCode);
    // }

    // public object? GetMetricForBatch(int batchCode, string metricName)
    // {
    //     AbstractAnalyticsDetails? batch = GetBatchAnalytics(batchCode);
    //     return batch?.GetMetric(metricName)?.GetValue();
    // }
}