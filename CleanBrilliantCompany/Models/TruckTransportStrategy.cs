using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class TruckTransportStrategy : ITransportStrategy
    {
        private readonly IRoutingService _routingService;

        public TruckTransportStrategy(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public async Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();
            // Use the async version of the routing service.
            float distance = await _routingService.GetDistanceAsync(senderAddress, recipientAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, distance));
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
