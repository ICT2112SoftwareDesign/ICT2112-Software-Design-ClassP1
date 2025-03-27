using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Services
{
    public interface _IShippingAgentDB
    {
        Task<List<CleanBrilliantCompany.Models.ShippingAgent>> GetShippingAgentsAsync();
        Task<CleanBrilliantCompany.Models.ShippingAgent> GetShippingAgentByIdAsync(int id);
        Task<bool> UpdateShippingAgentAsync(CleanBrilliantCompany.Models.ShippingAgent shippingAgent);
        Task<bool> DeleteShippingAgentAsync(int id);
        Task<bool> AddShippingAgentAsync(CleanBrilliantCompany.Models.ShippingAgent shippingAgent); // New method
    }
}