using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class SeaTransportStrategy : ITransportStrategy
    {
        private readonly IRoutingService _routingService;

        public SeaTransportStrategy(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public List<RouteSegment> CreateRoute(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            // Example: truck to port, sea leg, truck to recipient.
            float truckToPort = _routingService.GetDistance(senderAddress, "NearestPort", TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToPort));

            float seaLeg = _routingService.GetDistance("NearestPort", "DestinationPort", TransportMode.Sea);
            segments.Add(new RouteSegment(2, TransportMode.Sea, seaLeg));

            float portToRecipient = _routingService.GetDistance("DestinationPort", recipientAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(3, TransportMode.Truck, portToRecipient));

            return segments;
        }

        public float CalculateEmission(RouteSegment segment, float shipmentTotalWeight)
        {
            float factor = segment.Mode switch
            {
                TransportMode.Air => 1.5f,
                TransportMode.Sea => 0.1f,
                TransportMode.Truck => 0.5f,
                _ => 1.0f
            };
            return segment.Distance * shipmentTotalWeight * factor;
        }
    }
}
