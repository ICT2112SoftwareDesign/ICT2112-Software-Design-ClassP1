using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Services
{
    public interface IShippingAgentService
    {
        Task<List<ShippingAgent>> GetShippingAgentsAsync();
    }
}
