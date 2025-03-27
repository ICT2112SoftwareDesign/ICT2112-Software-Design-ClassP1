// Example IShippingAgentService.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IShippingAgent
    {
        Task<IEnumerable<ShippingAgent_RDM>> GetAllShippingAgentsAsync();
        Task<ShippingAgent_RDM> GetShippingAgentByIdAsync(int id);
        Task<ShippingAgent_RDM> AddShippingAgentAsync(ShippingAgent_RDM shippingAgent);
        Task<ShippingAgent_RDM> UpdateShippingAgentAsync(int id, ShippingAgent_RDM shippingAgent);
        Task<bool> DeleteShippingAgentAsync(int id);
    }
}