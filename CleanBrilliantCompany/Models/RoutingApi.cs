using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class RoutingAPI : IRoutingService
    {
        public float GetDistance(string origin, string destination, TransportMode mode)
        {
            // Dummy values – replace with real API calls when ready.
            return mode switch
            {
                TransportMode.Air => 1000.0f,  // e.g., 1000 km for an air leg
                TransportMode.Sea => 3000.0f,  // e.g., 3000 km for a sea leg
                TransportMode.Truck => 50.0f,  // e.g., 50 km for truck segments
                _ => 0.0f,
            };
        }
    }
}
