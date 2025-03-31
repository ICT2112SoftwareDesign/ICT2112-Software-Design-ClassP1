namespace CleanBrilliantCompany.DTO
{
    public class CarbonFootprintDTO
    {
        public int CarbonFootprintId { get; set; }
        public int EntityId { get; set; }
        public string EntityType { get; set; }
        public double CarbonEmission { get; set; }
        public string EcoStatus { get; set; }
        public DateOnly DateCreated { get; set; }
    }
}
