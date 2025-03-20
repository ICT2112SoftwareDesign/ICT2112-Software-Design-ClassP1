using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.DTO;

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

        public async Task<string> GetNearestAirportAsync(double latitude, double longitude)
        {
            var client = new HttpClient();

            var requestUrl = $"https://aerodatabox.p.rapidapi.com/airports/search/location/{latitude}/{longitude}/km/100/1";

            // Required Headers for RapidAPI
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(requestUrl),
                Headers =
                {
                    { "x-rapidapi-key", "77a1b4ec8emsh52d2b49c4cf6a82p1d8f88jsnc79d8a170c16" },      // replace with your new/working key
                    { "x-rapidapi-host", "aerodatabox.p.rapidapi.com" },
                }
            };

            Console.WriteLine($"[DEBUG] AeroDataBox API Request: {requestUrl}");

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();

                var jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[DEBUG] AeroDataBox Response: {jsonResponse}");

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                var airportResponse = JsonSerializer.Deserialize<AeroDataBoxResponse>(jsonResponse, options);

                if (airportResponse != null && airportResponse.items != null && airportResponse.items.Length > 0)
                {
                    var nearestAirport = airportResponse.items[0];
                    Console.WriteLine($"[DEBUG] Nearest Airport Found: {nearestAirport.name}");
                    return $"{nearestAirport.name}, {nearestAirport.municipalityName}, {nearestAirport.countryName}";
                }

                throw new Exception("No airports found near the recipient location.");
            }
        }

        public async Task<string> GetNearestPortAsync(double latitude, double longitude)
        {
            string username = "yugosaito4";  
            string requestUrl = $"http://api.geonames.org/findNearbyJSON?lat={latitude}&lng={longitude}&featureCode=PRT&username={username}";

            Console.WriteLine($"[DEBUG] GeoNames API Request: {requestUrl}");

            var response = await _client.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[ERROR] GeoNames API call failed: {response.StatusCode}");
                throw new Exception($"GeoNames API error: {response.StatusCode}");
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[DEBUG] GeoNames Response: {jsonResponse}");

            var portData = JsonSerializer.Deserialize<GeoNamesPortResponse>(jsonResponse);

            if (portData.geonames != null && portData.geonames.Length > 0)
            {
                var nearestPort = portData.geonames[0];
                Console.WriteLine($"[DEBUG] Nearest Port Found: {nearestPort.name}");
                return $"{nearestPort.name}, {nearestPort.countryName}";
            }

            throw new Exception("No nearby ports found.");
        }


        // Real geocoding using Nominatim API.
        public async Task<Coordinates> GeocodeAddressAsync(string address)
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

    }
}
