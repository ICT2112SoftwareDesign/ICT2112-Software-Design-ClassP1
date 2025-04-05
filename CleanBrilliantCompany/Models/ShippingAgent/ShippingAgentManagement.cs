using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class ShippingAgentManagement : IShippingAgent
    {
        private readonly IShippingAgentDB _shippingAgentDB;

        public ShippingAgentManagement(IShippingAgentDB shippingAgentDB)
        {
            _shippingAgentDB = shippingAgentDB;
        }

        // Add this property
        public IEnumerable<ShippingAgent_RDM> ShippingAgents { get; set; }

        public async Task<IEnumerable<ShippingAgent_RDM>> GetAllShippingAgentsAsync()
        {
            return await _shippingAgentDB.GetAllShippingAgentsAsync();
        }

        public async Task<ShippingAgent_RDM> GetShippingAgentByIdAsync(int id)
        {
            return await _shippingAgentDB.GetShippingAgentByIdAsync(id);
        }

        public async Task<ShippingAgent_RDM> AddShippingAgentAsync(ShippingAgent_RDM shippingAgent)
        {
            return await _shippingAgentDB.AddShippingAgentAsync(shippingAgent);
        }

        public async Task<ShippingAgent_RDM> UpdateShippingAgentAsync(int id, ShippingAgent_RDM shippingAgent)
        {
            return await _shippingAgentDB.UpdateShippingAgentAsync(id, shippingAgent);
        }

        public async Task<bool> DeleteShippingAgentAsync(int id)
        {
            return await _shippingAgentDB.DeleteShippingAgentAsync(id);
        }
    }
}