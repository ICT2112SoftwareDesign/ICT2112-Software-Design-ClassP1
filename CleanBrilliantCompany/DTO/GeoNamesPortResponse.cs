namespace CleanBrilliantCompany.DTO
{
    public class GeoNamesPortResponse
    {
        public GeoNamePort[] geonames { get; set; }
    }

    public class GeoNamePort
    {
        public string name { get; set; }
        public string countryName { get; set; }
        public string lat { get; set; }    
        public string lng { get; set; }
    }
}
