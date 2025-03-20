using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

// Assuming Service is an enum, define it here if it doesn't exist elsewhere
public enum Service
{
    Standard,
}


namespace CleanBrilliantCompany.Models
{
    public class ShippingAgents : IShippingAgents
    {
        public List<string> getShippingAgentList(Service shippingType)
        {
            // Example implementation
            var agents = new List<string>();

            switch (shippingType)
            {
                case Service.Standard:
                    agents.Add("DHL");
                    agents.Add("ParcelForce");
                    break;
                // Add more cases as needed
                default:
                    agents.Add("Default Shipping Agent");
                    break;
            }

            return agents;
        }
    }
}