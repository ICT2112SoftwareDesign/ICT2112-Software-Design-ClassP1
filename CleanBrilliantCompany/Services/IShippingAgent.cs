using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Services
{
    public interface IShippingAgentService
    {
        Task<List<CleanBrilliantCompany.Models.ShippingAgent>> GetShippingAgentsAsync();
        Task<CleanBrilliantCompany.Models.ShippingAgent> GetShippingAgentByIdAsync(int id);
        Task<bool> UpdateShippingAgentAsync(CleanBrilliantCompany.Models.ShippingAgent shippingAgent);
    }
}