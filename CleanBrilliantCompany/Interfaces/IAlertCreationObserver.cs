using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    /// <summary>
    /// defines the contract for observers that need to be notified
    /// when a new alert is successfully created and persisted.
    /// </summary>
    public interface IAlertCreationObserver
    {
        /// <summary>
        /// method called by the subject when a new alert is created.
        /// </summary>
        /// <param name="createdAlert">the alert object that was just created.</param>
        /// <returns>a task representing the asynchronous operation.</returns>
        Task Update(Alert createdAlert);
    }
} 