using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class AirTransportStrategy : ITransportStrategy
    {
        private readonly IRoutingService _routingService;

        public AirTransportStrategy(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public List<RouteSegment> CreateRoute(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            // Example: truck from warehouse to origin airport, then air leg, then truck to recipient.
            float truckToAirport = _routingService.GetDistance(senderAddress, "NearestAirport", TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToAirport));

            float airLeg = _routingService.GetDistance("NearestAirport", "DestinationAirport", TransportMode.Air);
            segments.Add(new RouteSegment(2, TransportMode.Air, airLeg));

            float airportToRecipient = _routingService.GetDistance("DestinationAirport", recipientAddress, TransportMode.Truck);
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
