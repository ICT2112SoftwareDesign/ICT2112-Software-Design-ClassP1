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
        /// gets a single alert by its primary key.
        /// </summary>
        Task<Alert?> GetAlertByIdAsync(int alertId); // return nullable alert

        /// <summary>
        /// adds a new alert record to the database.
        /// </summary>
        Task<int> AddAlertAsync(Alert alert); // returns the new alert's id

        /// <summary>
        /// updates an existing alert record.
        /// </summary>
        Task<bool> UpdateAlertAsync(Alert alert); // returns true if successful

        /// <summary>
        /// deletes an alert record by its primary key.
        /// </summary>
        Task<bool> DeleteAlertAsync(int alertId); // returns true if successful

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
