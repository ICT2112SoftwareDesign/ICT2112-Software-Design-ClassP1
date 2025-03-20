// Models/CarbonNotification.cs
using CleanBrilliantCompany.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Models
{
    public class CarbonNotification : ICarbonNotification
    {
        private readonly IAlertsDB _alertsDb;

        public CarbonNotification(IAlertsDB alertsDb)
        {
            _alertsDb = alertsDb;
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            return await _alertsDb.GetAllAlertsAsync();
        }

        public async Task<Alert> GetAlertByIdAsync(int id)
        {
            return await _alertsDb.GetAlertByIdAsync(id);
        }

        public async Task<int> CreateAlertAsync(Alert alert)
        {
            return await _alertsDb.AddAlertAsync(alert);
        }

        public async Task<bool> UpdateAlertAsync(Alert alert)
        {
            return await _alertsDb.UpdateAlertAsync(alert);
        }

        public async Task<bool> DeleteAlertAsync(int id)
        {
            return await _alertsDb.DeleteAlertAsync(id);
        }
    }
}
