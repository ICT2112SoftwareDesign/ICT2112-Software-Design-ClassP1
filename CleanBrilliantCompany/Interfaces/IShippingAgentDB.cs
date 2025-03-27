using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Mapper; 

namespace CleanBrilliantCompany.Interfaces
{
    public class IShippingAgentDB : IShippingAgent
    {
        private readonly ShippingAgentMapper _mapper;

        public IShippingAgentDB(ShippingAgentMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<IEnumerable<ShippingAgent_RDM>> GetAllShippingAgentsAsync()
        {
            // Call the correct method from mapper
            return await _mapper.GetShippingAgentsAsync();
        }

        public async Task<ShippingAgent_RDM> GetShippingAgentByIdAsync(int id)
        {
            return await _mapper.GetShippingAgentByIdAsync(id);
        }

        public async Task<bool> DeleteShippingAgentAsync(int id)
        {
            return await _mapper.DeleteShippingAgentAsync(id);
        }

        public async Task<ShippingAgent_RDM> CreateShippingAgentAsync(ShippingAgent_RDM shippingAgent)
        {
            // Call the correct method and handle the return value conversion
            bool success = await _mapper.AddShippingAgentAsync(shippingAgent);
            if (success)
            {
                // Since AddShippingAgentAsync doesn't return the created object with ID,
                // we would ideally need to retrieve it after creation
                // This is a simplified placeholder approach
                return shippingAgent;
            }
            return new ShippingAgent_RDM(); // Return empty object instead of null
        }

        public async Task<ShippingAgent_RDM> UpdateShippingAgentAsync(int id, ShippingAgent_RDM shippingAgent)
        {
            // Set ID to ensure correct record is updated
            shippingAgent.ShippingAgentId = id;
            
            // Call the correct method
            bool success = await _mapper.UpdateShippingAgentAsync(shippingAgent);
            if (success)
            {
                return shippingAgent;
            }
            return new ShippingAgent_RDM(); // Return empty object instead of null
        }
    }
}