// Interfaces/IAlertsDB.cs
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    /// <summary>
    /// interface for interacting with the alerts data store (using the full alert model)
    /// </summary>
    public interface IAlertsDB
    {
        /// <summary>
        /// gets all alerts, ordered by timestamp descending.
        /// </summary>
        Task<IEnumerable<Alert>> GetAllAlertsAsync();

        /// <summary>
        /// adds a new alert record to the database.
        /// </summary>
        Task<int> AddAlertAsync(Alert alert); // returns the new alert's id

        /// <summary>
        /// checks if an alert already exists for the specified period.
        /// </summary>
        Task<bool> CheckAlertExistsAsync(int year, int month);

        /// <summary>
        /// deletes an alert record based on its goal period.
        /// </summary>
        /// <returns>true if an alert was deleted, false otherwise.</returns>
        Task<bool> DeleteAlertByPeriodAsync(int year, int month);
    }
}
