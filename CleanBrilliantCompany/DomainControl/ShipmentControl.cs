using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class ShipmentControl
    {
        private readonly IRoutingService _routingService;
        private readonly IOrder _orderService;  // Module 1's order interface

        public ShipmentControl(IRoutingService routingService, IOrder orderService)
        {
            _routingService = routingService;
            _orderService = orderService;
        }

        // Asynchronous shipment creation method.
        // Retrieves order details from IOrder and uses the OrderWeight property.
        public async Task<ShipmentSDM> CreateShipmentAsync(int orderId, string senderAddress)
        {
            // Retrieve order details from Module 1 via IOrder.
            OrderRDM orderRDM = _orderService.getOrderDetails(orderId);
            if (orderRDM == null)
            {
                throw new Exception($"Order with ID {orderId} not found.");
            }

            // Extract recipient address and shipping method from OrderRDM.
            string recipientAddress = orderRDM.GetOrderAddress();
            string shippingMethod = orderRDM.GetOrderShipping();

            // Use the order weight directly from OrderRDM.
            double totalWeight = orderRDM.OrderWeight;

            // Select the appropriate transport strategy.
            ITransportStrategy strategy = SelectStrategy(shippingMethod);

            // Generate route segments using the fixed sender address and the recipient address.
            var routeSegments = await strategy.CreateRouteAsync(senderAddress, recipientAddress);

            // Construct and return the shipment entity.
            return new ShipmentSDM
            {
                OrderId = orderRDM.GetOrderID(),
                TotalWeight = totalWeight,
                RouteSegments = routeSegments
            };
        }

        // Helper method for selecting the appropriate transport strategy.
        private ITransportStrategy SelectStrategy(string method)
        {
            if (method.Equals("Air", StringComparison.OrdinalIgnoreCase))
                return new AirTransportStrategy(_routingService);
            else if (method.Equals("Sea", StringComparison.OrdinalIgnoreCase))
                return new SeaTransportStrategy(_routingService);
            else // default to Truck
                return new TruckTransportStrategy(_routingService);
        }
    }

    // Note: OrderRDM is provided by Module 1.
}
