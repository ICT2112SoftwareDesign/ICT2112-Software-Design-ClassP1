namespace CleanBrilliantCompany.DTO
{
    public class AeroDataBoxResponse
    {
        public AeroAirportItem[] items { get; set; }
    }

    public class AeroAirportItem
    {
        public string iata { get; set; }
        public string icao { get; set; }
        public string name { get; set; }
        public string municipalityName { get; set; }
        public string countryName { get; set; }
        public Location location { get; set; }
    }

    public class Location
    {
        public double lat { get; set; }
        public double lon { get; set; }
    }
}
