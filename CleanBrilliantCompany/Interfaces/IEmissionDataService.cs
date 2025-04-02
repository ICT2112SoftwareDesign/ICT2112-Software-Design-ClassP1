namespace CleanBrilliantCompany.Interfaces
{
    /// <summary>
    /// defines the contract for services providing aggregated emission data.
    /// </summary>
    public interface IEmissionDataService
    {
        /// <summary>
        /// calculates the total combined carbon emissions (items + orders) for a specific month and year.
        /// </summary>
        /// <param name="month">the month (1-12).</param>
        /// <param name="year">the year.</param>
        /// <returns>the total calculated emissions as a decimal.</returns>
        Task<decimal> GetTotalEmissionsForMonthAsync(int month, int year);
    }
}