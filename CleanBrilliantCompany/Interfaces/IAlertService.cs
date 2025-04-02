using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    /// <summary>
    /// defines the contract for managing alerts through a service layer.
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// creates a new alert record.
        /// </summary>
        /// <param name="alert">the alert object to create.</param>
        Task CreateAlertAsync(Alert alert);

        /// <summary>
        /// retrieves all historical alerts, typically ordered by date descending.
        /// </summary>
        /// <returns>an enumerable collection of alert objects.</returns>
        Task<IEnumerable<Alert>> GetAllAlertsAsync();

        /// <summary>
        /// generates the alert details for a specific period without saving it.
        /// </summary>
        /// <param name="year">the year of the period.</param>
        /// <param name="month">the month of the period (1-12).</param>
        /// <returns>an alert object populated with the period's data, or null if generation fails.</returns>
        Task<Alert?> GenerateAlertForPeriodAsync(int year, int month);

        /// <summary>
        /// checks if an alert already exists in the database for a specific period.
        /// </summary>
        /// <param name="year">the year of the period.</param>
        /// <param name="month">the month of the period (1-12).</param>
        /// <returns>true if an alert exists, false otherwise.</returns>
        Task<bool> CheckAlertExistsAsync(int year, int month);

        /// <summary>
        /// deletes an alert record based on its goal period.
        /// </summary>
        /// <returns>true if an alert was deleted, false otherwise.</returns>
        Task<bool> DeleteAlertByPeriodAsync(int year, int month);
    }
}