using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IShippingAgents
    {
        // Get the list of shipping agents based on the service type
        List<string> getShippingAgentList(Service shippingType);

        // Get the list of available service types (e.g., "1 Day", "3 Days", "7 Days")
        List<string> getServiceTypes();

        // Get the list of available shipping methods (e.g., "Air", "Truck")
        List<string> getShippingMethods();
    }
}

