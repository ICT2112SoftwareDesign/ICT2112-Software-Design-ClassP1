namespace CleanBrilliantCompany.Interfaces
{
    public interface IMetric
    {
        /// <summary>
        /// Gets the name of the metric.
        /// </summary>
        /// <returns>String representing the metric name.</returns>
        string GetMetricName();

        /// <summary>
        /// Gets the computed value of the metric.
        /// </summary>
        /// <returns>Object representing the metric's computed value.</returns>
        object GetValue();
    }
}
