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
        public async Task<ShipmentSDM> CreateShipmentAsync(int orderId, double totalWeight, string shippingMethod, string senderAddress, string recipientAddress)
        {
            ITransportStrategy strategy;

            switch (shippingMethod.ToLower())
            {
                case "air":
                    strategy = new AirTransportStrategy(_routingService);
                    break;
                case "sea":
                    strategy = new SeaTransportStrategy(_routingService);
                    break;
                case "truck":
                    strategy = new TruckTransportStrategy(_routingService);
                    break;
                default:
                    throw new ArgumentException("Unsupported shipping method.");
            }

            var routeSegments = await strategy.CreateRouteAsync(senderAddress, recipientAddress);

            var shipment = new ShipmentSDM
            {
                OrderId = orderId,
                TotalWeight = totalWeight,
                RouteSegments = routeSegments
            };

            return shipment;
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
