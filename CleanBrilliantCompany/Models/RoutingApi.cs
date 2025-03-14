using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class RoutingAPI : IRoutingService
    {
        private readonly HttpClient _client = new HttpClient();

        public async Task<float> GetDistanceAsync(string origin, string destination, TransportMode mode)
        {
            if (mode == TransportMode.Truck)
            {
                var originCoords = GeocodeAddress(origin);
                var destCoords = GeocodeAddress(destination);

                // Debug: print the resolved coordinates.
                Console.WriteLine($"[DEBUG] Origin Address: {origin}");
                Console.WriteLine($"[DEBUG] Resolved Origin Coordinates: ({originCoords.Latitude}, {originCoords.Longitude})");
                Console.WriteLine($"[DEBUG] Destination Address: {destination}");
                Console.WriteLine($"[DEBUG] Resolved Destination Coordinates: ({destCoords.Latitude}, {destCoords.Longitude})");

                // Build OSRM URL. OSRM expects coordinates in "longitude,latitude" order.
                string url = $"http://router.project-osrm.org/route/v1/driving/{originCoords.Longitude},{originCoords.Latitude};{destCoords.Longitude},{destCoords.Latitude}?overview=false";
                Console.WriteLine($"[DEBUG] OSRM API URL: {url}");

                HttpResponseMessage response = await _client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[DEBUG] OSRM API call failed: {response.StatusCode}");
                    return 0;
                }

                string json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] OSRM API response: {json}");

                // Use case-insensitive deserialization options.
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                OsrmResponse osrmResponse = JsonSerializer.Deserialize<OsrmResponse>(json, options);
                if (osrmResponse?.Routes != null && osrmResponse.Routes.Length > 0)
                {
                    float distanceKm = osrmResponse.Routes[0].Distance / 1000.0f;
                    Console.WriteLine($"[DEBUG] Calculated Truck Distance: {distanceKm} km");
                    return distanceKm;
                }
                return 0;
            }
            // For Air and Sea modes, use the haversine formula.
            else if (mode == TransportMode.Air || mode == TransportMode.Sea)
            {
                var originCoords = GeocodeAddress(origin);
                var destCoords = GeocodeAddress(destination);

                Console.WriteLine($"[DEBUG] Origin Address: {origin}");
                Console.WriteLine($"[DEBUG] Resolved Origin Coordinates: ({originCoords.Latitude}, {originCoords.Longitude})");
                Console.WriteLine($"[DEBUG] Destination Address: {destination}");
                Console.WriteLine($"[DEBUG] Resolved Destination Coordinates: ({destCoords.Latitude}, {destCoords.Longitude})");

                float haversine = HaversineDistance(originCoords.Latitude, originCoords.Longitude, destCoords.Latitude, destCoords.Longitude);
                Console.WriteLine($"[DEBUG] Calculated Haversine Distance: {haversine} km");
                return haversine;
            }
            return 0;
        }

        private Coordinates GeocodeAddress(string address)
        {
            // Example: Differentiate based on specific keywords.
            if (address.Contains("ABC Warehouse"))
            {
                return new Coordinates { Latitude = 1.3521, Longitude = 103.8198 };
            }
            else if (address.Contains("XYZ Town"))
            {
                return new Coordinates { Latitude = 1.3000, Longitude = 103.8000 };
            }
            else if (address.Contains("NearestAirport"))
            {
                return new Coordinates { Latitude = 1.3500, Longitude = 103.9000 };
            }
            else if (address.Contains("DestinationAirport"))
            {
                return new Coordinates { Latitude = 1.2800, Longitude = 103.7000 };
            }
            else if (address.Contains("NearestPort"))
            {
                return new Coordinates { Latitude = 1.3600, Longitude = 103.9500 };
            }
            else if (address.Contains("DestinationPort"))
            {
                return new Coordinates { Latitude = 1.2500, Longitude = 103.6500 };
            }
            // Default: return a fixed coordinate.
            return new Coordinates { Latitude = 1.3521, Longitude = 103.8198 };
        }

        private float HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // Earth's radius in kilometers
            double dLat = ToRadians(lat2 - lat1);
            double dLon = ToRadians(lon2 - lon1);
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return (float)(R * c);
        }

        private double ToRadians(double angle) => angle * (Math.PI / 180);

        // Helper classes for internal use:
        private class Coordinates
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        private class OsrmResponse
        {
            public Route[] Routes { get; set; }
        }

        private class Route
        {
            public float Distance { get; set; }
        }
    }
}
