// Interfaces/IAlertsDB.cs
using CleanBrilliantCompany.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IAlertsDB
    {
        Task<IEnumerable<Alert>> GetAllAlertsAsync();
        Task<Alert> GetAlertByIdAsync(int id);
        Task<int> AddAlertAsync(Alert alert);
        Task<bool> UpdateAlertAsync(Alert alert);
        Task<bool> DeleteAlertAsync(int id);
    }
}
