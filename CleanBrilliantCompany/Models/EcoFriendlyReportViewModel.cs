namespace CleanBrilliantCompany.Models
{
    public class EcoFriendlyReportViewModel
    {
        public float TotalItemCF { get; set; }
        public float TotalEcoFriendlyItemCF { get; set; }
        public float EcoFriendlyCFPercentage { get; set; }
        public List<object> ItemCarbonFootprints { get; set; }
    }
}
