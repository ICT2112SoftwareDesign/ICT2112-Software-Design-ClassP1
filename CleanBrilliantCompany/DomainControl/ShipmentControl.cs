using System;
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
        // Note: The totalWeight parameter has been removed.
        public async Task<ShipmentSDM> CreateShipmentAsync(int orderId, string shippingMethod, string senderAddress, string recipientAddress)
        {
            // Hardcoded method to simulate retrieval of order details (IOrder interface in the future).
            OrderDetails orderDetails = GetOrderDetailsHardcoded(orderId);

            // Use the recipient address from order details.
            string recipientAddrFromOrder = orderDetails.RecipientAddress;
            List<Item> items = orderDetails.Items;
            double totalWeight = CalculateTotalWeight(items);

            ITransportStrategy strategy = SelectStrategy(shippingMethod);
            var routeSegments = await strategy.CreateRouteAsync(senderAddress, recipientAddrFromOrder);

            var shipment = new ShipmentSDM
            {
                OrderId = orderDetails.OrderId,
                TotalWeight = totalWeight,
                RouteSegments = routeSegments
            };

            return shipment;
        }

        // Hardcoded method simulating IOrder retrieval.
        private OrderDetails GetOrderDetailsHardcoded(int orderId)
        {
            return new OrderDetails
            {
                OrderId = orderId,
                RecipientAddress = "Buckingham Palace",
                ShippingMethod = "Air",
                Items = new List<Item>
                {
                    new Item { Name = "Bleach", Weight = 2.0, Quantity = 2, BatchCode = 1 },
                    new Item { Name = "Detergent", Weight = 1.0, Quantity = 4, BatchCode = 2 }
                }
            };
        }

        // Helper method for selecting the appropriate strategy.
        private ITransportStrategy SelectStrategy(string method)
        {
            if (method.Equals("Air", StringComparison.OrdinalIgnoreCase))
                return new AirTransportStrategy(_routingService);
            else if (method.Equals("Sea", StringComparison.OrdinalIgnoreCase))
                return new SeaTransportStrategy(_routingService);
            else // default to Truck
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
    }

    // Representation of order details.
    public class OrderDetails
    {
        public int OrderId { get; set; }
        public string RecipientAddress { get; set; }
        public string ShippingMethod { get; set; }
        public List<Item> Items { get; set; }
    }
}
