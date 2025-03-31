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

        public async Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            Console.WriteLine("[DEBUG] Starting AirTransportStrategy route creation...");

            var senderCoords = await _routingService.GeocodeAddressAsync(senderAddress);
            var recipientCoords = await _routingService.GeocodeAddressAsync(recipientAddress);

            var senderAirport = "Singapore Changi Airport, Singapore";
            string recipientAirport = await _routingService.GetNearestAirportAsync(recipientCoords.Latitude, recipientCoords.Longitude);

            Console.WriteLine($"[DEBUG] Nearest Recipient Airport: {recipientAirport}");

            // 1. Truck from sender to sender airport
            float truckToAirport = await _routingService.GetDistanceAsync(senderAddress, senderAirport, TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToAirport));

            // 2. Air from sender airport to recipient airport
            float airLeg = await _routingService.GetDistanceAsync(senderAirport, recipientAirport, TransportMode.Air);
            segments.Add(new RouteSegment(2, TransportMode.Air, airLeg));

            // 3. Truck from recipient airport to recipient address
            float airportToRecipient = await _routingService.GetDistanceAsync(recipientAirport, recipientAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(3, TransportMode.Truck, airportToRecipient));

            Console.WriteLine("[DEBUG] Completed AirTransportStrategy route creation.");

            return segments;
        }
    }
}
