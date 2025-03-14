using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class SeaTransportStrategy : ITransportStrategy
    {
        private readonly IRoutingService _routingService;

        public SeaTransportStrategy(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public async Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            // Truck leg: from senderAddress to nearest port.
            float truckToPort = await _routingService.GetDistanceAsync(senderAddress, "NearestPort", TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToPort));

            // Sea leg: from nearest port to destination port.
            float seaLeg = await _routingService.GetDistanceAsync("NearestPort", "DestinationPort", TransportMode.Sea);
            segments.Add(new RouteSegment(2, TransportMode.Sea, seaLeg));

            // Truck leg: from destination port to recipientAddress.
            float portToRecipient = await _routingService.GetDistanceAsync("DestinationPort", recipientAddress, TransportMode.Truck);
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
