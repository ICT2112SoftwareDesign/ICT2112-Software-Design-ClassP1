using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class AirTransportStrategy : ITransportStrategy
    {
        private readonly IRoutingService _routingService;

        public AirTransportStrategy(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public async Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            // Truck leg: from senderAddress to nearest airport.
            float truckToAirport = await _routingService.GetDistanceAsync(senderAddress, "NearestAirport", TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToAirport));

            // Air leg: from nearest airport to destination airport.
            float airLeg = await _routingService.GetDistanceAsync("NearestAirport", "DestinationAirport", TransportMode.Air);
            segments.Add(new RouteSegment(2, TransportMode.Air, airLeg));

            // Truck leg: from destination airport to recipientAddress.
            float airportToRecipient = await _routingService.GetDistanceAsync("DestinationAirport", recipientAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(3, TransportMode.Truck, airportToRecipient));

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
