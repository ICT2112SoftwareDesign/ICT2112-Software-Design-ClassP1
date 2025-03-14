using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        // Updated async method that uses real airport addresses.
        public async Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            // For sender, use a fixed airport (Singapore Changi Airport).
            string senderAirport = "Singapore Changi Airport, Singapore";

            // Dynamically determine the recipient's nearest airport.
            string recipientAirport = GetNearestAirport(recipientAddress);

            // Truck leg: from sender address to sender airport.
            float truckToAirport = await _routingService.GetDistanceAsync(senderAddress, senderAirport, TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToAirport));

            // Air leg: from sender airport to recipient airport.
            float airLeg = await _routingService.GetDistanceAsync(senderAirport, recipientAirport, TransportMode.Air);
            segments.Add(new RouteSegment(2, TransportMode.Air, airLeg));

            // Truck leg: from recipient airport to recipient address.
            float airportToRecipient = await _routingService.GetDistanceAsync(recipientAirport, recipientAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(3, TransportMode.Truck, airportToRecipient));

            return segments;
        }

        // A simple helper to return a recipient airport based on keywords in the recipient address.
        private string GetNearestAirport(string recipientAddress)
        {
            if (recipientAddress.Contains("London", StringComparison.OrdinalIgnoreCase))
            {
                return "Heathrow Airport, London, UK";
            }
            else if (recipientAddress.Contains("Paris", StringComparison.OrdinalIgnoreCase))
            {
                return "Charles de Gaulle Airport, Paris, France";
            }
            else if (recipientAddress.Contains("New York", StringComparison.OrdinalIgnoreCase))
            {
                return "John F. Kennedy International Airport, New York, USA";
            }
            // You can add more conditions as needed.
            else
            {
                // As a fallback, if no keyword is found, use the recipient address itself.
                // (Alternatively, you could throw an exception or use a default airport.)
                return recipientAddress;
            }
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
