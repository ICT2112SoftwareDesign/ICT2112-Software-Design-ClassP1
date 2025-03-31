using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Models
{
    public class SeaTransportStrategy : ITransportStrategy
    {
        private readonly IRoutingService _routingService;

        // Hardcoded Singapore port name
        private const string SingaporePortAddress = "PSA Singapore, Singapore";

        public SeaTransportStrategy(IRoutingService routingService)
        {
            _routingService = routingService;
        }

        public async Task<List<RouteSegment>> CreateRouteAsync(string senderAddress, string recipientAddress)
        {
            var segments = new List<RouteSegment>();

            // Step 1: Geocode recipient address
            var recipientCoords = await _routingService.GeocodeAddressAsync(recipientAddress);

            // Step 2: Find nearest port to recipient
            var recipientNearestPort = await _routingService.GetNearestPortAsync(recipientCoords.Latitude, recipientCoords.Longitude);

            // Step 3: Truck leg from sender address to Singapore Port
            float truckToSingaporePort = await _routingService.GetDistanceAsync(senderAddress, SingaporePortAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(1, TransportMode.Truck, truckToSingaporePort));

            // Step 4: Sea leg from Singapore Port to Recipient Port
            float seaLeg = await _routingService.GetDistanceAsync(SingaporePortAddress, recipientNearestPort, TransportMode.Sea);
            segments.Add(new RouteSegment(2, TransportMode.Sea, seaLeg));

            // Step 5: Truck leg from Recipient Port to Recipient Address
            float portToRecipient = await _routingService.GetDistanceAsync(recipientNearestPort, recipientAddress, TransportMode.Truck);
            segments.Add(new RouteSegment(3, TransportMode.Truck, portToRecipient));

            return segments;
        }
    }
}
