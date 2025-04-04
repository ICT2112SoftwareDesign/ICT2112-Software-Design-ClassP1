using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Services
{
    /// <summary>
    /// abstract base class for subjects that notify observers upon alert creation.
    /// handles observer management and notification logic.
    /// </summary>
    public abstract class AbstractAlertNotifierSubject
    {
        protected readonly List<IAlertCreationObserver> _observers;

        protected AbstractAlertNotifierSubject(
            IEnumerable<IAlertCreationObserver> observers)
        {
            _observers = observers?.ToList() ?? new List<IAlertCreationObserver>();
        }

        /// <summary>
        /// attaches an observer to be notified of alert creations.
        /// </summary>
        /// <param name="observer">the observer to attach.</param>
        public virtual void AttachObserver(IAlertCreationObserver observer)
        {
            if (observer != null && !_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        /// <summary>
        /// detaches an observer so it no longer receives notifications.
        /// </summary>
        /// <param name="observer">the observer to detach.</param>
        public virtual void DetachObserver(IAlertCreationObserver observer)
        {
            if (observer != null && _observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        /// <summary>
        /// notifies all registered observers about a newly created alert.
        /// intended to be called by concrete subject classes.
        /// </summary>
        /// <param name="createdAlert">the alert that was created.</param>
        protected async Task NotifyObserversAsync(Alert createdAlert)
        {
            if (_observers.Any())
            {
                var notificationTasks = _observers.Select(observer => observer.Update(createdAlert));
                try
                {
                    await Task.WhenAll(notificationTasks);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"error in observer notification: {ex.Message}");
                }
            }
        }
    }
}