using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class ShipmentControl
    {
        private readonly IRoutingService _routingService;

        public ShipmentControl(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        // Create a shipment using hard-coded details and the chosen transport strategy.
        public ShipmentSDM CreateShipment(
            int orderId,
            double totalWeight,
            string shippingMethod,
            string senderAddress,
            string recipientAddress)
        {
            // Select the appropriate strategy.
            ITransportStrategy strategy = SelectStrategy(shippingMethod);
            List<RouteSegment> segments = strategy.CreateRoute(senderAddress, recipientAddress);

            // Build and return the ShipmentSDM object.
            return new ShipmentSDM
            {
                ShipmentId = 0, // Set or generate as needed.
                OrderId = orderId,
                TotalWeight = totalWeight,
                RouteSegments = segments
            };
        }

        // Simple strategy selection based on shipping method.
        private ITransportStrategy SelectStrategy(string method)
        {
            if (method.Equals("Air", System.StringComparison.OrdinalIgnoreCase))
                return new AirTransportStrategy(_routingService);
            else if (method.Equals("Sea", System.StringComparison.OrdinalIgnoreCase))
                return new SeaTransportStrategy(_routingService);
            else // Default to truck
                return new TruckTransportStrategy(_routingService);
        }

        // Calculate total weight from a list of items (optional helper).
        public double CalculateTotalWeight(List<Item> items)
        {
            double total = 0;
            foreach (var item in items)
            {
                total += item.Weight * item.Quantity;
            }
            return total;
        }



        public class Item
        {
            public string Name { get; set; }
            public double Weight { get; set; }
            public int Quantity { get; set; }
        }
    }

}
