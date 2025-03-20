// Interfaces/ICarbonNotification.cs
using CleanBrilliantCompany.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ICarbonNotification
    {
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task<Alert> GetAlertByIdAsync(int id);
        Task<int> CreateAlertAsync(Alert alert);
        Task<bool> UpdateAlertAsync(Alert alert);
        Task<bool> DeleteAlertAsync(int id);
    }
}
