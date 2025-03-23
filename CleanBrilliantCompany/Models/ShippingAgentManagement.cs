using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

public enum Service
{
    OneDay,
    ThreeDays,
    SevenDays
}

public enum ShippingMethod
{
    Air,
    Truck
}

namespace CleanBrilliantCompany.Models
{
    public class ShippingAgents : IShippingAgents
    {
        public List<string> getShippingAgentList(Service shippingType)
        {
            // All shipping agents are available for all service types
            return new List<string> { "DHL", "ParcelForce" };
        }


        public List<string> getServiceTypes()
        {
            return new List<string> { "1 Day", "3 Days", "7 Days" };
        }

        public List<string> getShippingMethods()
        {
            return new List<string> { "Air", "Truck" };
        }
    }
}