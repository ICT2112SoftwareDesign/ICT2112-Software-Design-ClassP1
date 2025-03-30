namespace CleanBrilliantCompany.Interface
{
    public interface IAnalyticsDetailsFacade
    {
        /// <summary>
        /// Retrieves all analytics data required for generating reports.
        /// </summary>
        /// <returns>A string or object representing analytics data.</returns>
        string RetrieveAllAnalytics();
    }
}
