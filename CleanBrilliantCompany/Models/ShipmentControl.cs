using System.Collections.Generic;
using System.Threading.Tasks;
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

        // Asynchronous shipment creation method.
        public async Task<ShipmentSDM> CreateShipmentAsync(
            int orderId,
            double totalWeight,
            string shippingMethod,
            string senderAddress,
            string recipientAddress)
        {
            // Select the appropriate strategy based on the shipping method.
            ITransportStrategy strategy = SelectStrategy(shippingMethod);

            // Await the asynchronous creation of the route.
            List<RouteSegment> segments = await strategy.CreateRouteAsync(senderAddress, recipientAddress);

            // Build and return the ShipmentSDM object.
            return new ShipmentSDM
            {
                ShipmentId = 0, // Set or generate an ID as needed.
                OrderId = orderId,
                TotalWeight = totalWeight,
                RouteSegments = segments
            };
        }

        // Synchronous helper method to choose the correct strategy.
        private ITransportStrategy SelectStrategy(string method)
        {
            if (method.Equals("Air", System.StringComparison.OrdinalIgnoreCase))
                return new AirTransportStrategy(_routingService);
            else if (method.Equals("Sea", System.StringComparison.OrdinalIgnoreCase))
                return new SeaTransportStrategy(_routingService);
            else // default to Truck.
                return new TruckTransportStrategy(_routingService);
        }

        // Helper method to calculate total weight from a list of items.
        public double CalculateTotalWeight(List<Item> items)
        {
            double total = 0;
            foreach (var item in items)
            {
                total += item.Weight * item.Quantity;
            }
            return total;
        }

        // Inner class representing an item; alternatively, define this in its own file.
        public class Item
        {
            public string Name { get; set; }
            public double Weight { get; set; }
            public int Quantity { get; set; }
        }
    }
}
