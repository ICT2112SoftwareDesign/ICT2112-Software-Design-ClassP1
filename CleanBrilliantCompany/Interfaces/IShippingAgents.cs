using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IShippingAgents
    {
        List<string> getShippingAgentList(Service shippingType);
    }
}

