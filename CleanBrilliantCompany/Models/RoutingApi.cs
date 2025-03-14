using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class RoutingAPI : IRoutingService
    {
        private readonly HttpClient _client = new HttpClient();

        public RoutingAPI()
        {
            // Set a proper User-Agent per Nominatim's policy.
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("CleanBrilliantCompany/1.0 (yugosaito4@gmail.com)");
        }

        public async Task<float> GetDistanceAsync(string origin, string destination, TransportMode mode)
        {
            if (mode == TransportMode.Truck)
            {
                // Use the real geocoding API for both addresses.
                Coordinates originCoords = await GeocodeAddressAsync(origin);
                Coordinates destCoords = await GeocodeAddressAsync(destination);

                Console.WriteLine($"[DEBUG] Origin Address: {origin}");
                Console.WriteLine($"[DEBUG] Resolved Origin Coordinates: ({originCoords.Latitude}, {originCoords.Longitude})");
                Console.WriteLine($"[DEBUG] Destination Address: {destination}");
                Console.WriteLine($"[DEBUG] Resolved Destination Coordinates: ({destCoords.Latitude}, {destCoords.Longitude})");

                // OSRM API expects coordinates in "longitude,latitude" order.
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

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                OsrmResponse osrmResponse = JsonSerializer.Deserialize<OsrmResponse>(json, options);
                if (osrmResponse?.Routes != null && osrmResponse.Routes.Length > 0)
                {
                    float distanceKm = osrmResponse.Routes[0].Distance / 1000.0f;
                    Console.WriteLine($"[DEBUG] Calculated Truck Distance: {distanceKm} km");
                    return distanceKm;
                }
                return 0;
            }
            else if (mode == TransportMode.Air || mode == TransportMode.Sea)
            {
                // For air and sea, use the haversine formula.
                Coordinates originCoords = await GeocodeAddressAsync(origin);
                Coordinates destCoords = await GeocodeAddressAsync(destination);

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

        // Real geocoding using Nominatim API.
        private async Task<Coordinates> GeocodeAddressAsync(string address)
        {
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(address)}&format=json&limit=1";
            Console.WriteLine($"[DEBUG] Geocoding URL: {url}");

            HttpResponseMessage response = await _client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[DEBUG] Geocoding API call failed: {response.StatusCode}");
                throw new Exception($"Geocoding API error: {response.StatusCode}");
            }
            string json = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] Geocoding API response: {json}");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            List<NominatimResult> results = JsonSerializer.Deserialize<List<NominatimResult>>(json, options);
            if (results != null && results.Count > 0)
            {
                return new Coordinates
                {
                    Latitude = double.Parse(results[0].lat),
                    Longitude = double.Parse(results[0].lon)
                };
            }
            throw new Exception($"No geocoding results for address: {address}");
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

        // Nominatim API result.
        private class NominatimResult
        {
            public string lat { get; set; }
            public string lon { get; set; }
            // You can include other properties if needed.
        }
    }
}
