namespace CleanBrilliantCompany.Models
{
    public class CarbonDashboardViewModel
    {
        public float TotalProductCF { get; set; }
        public float TotalItemCF { get; set; }
        public float TotalOrderCF { get; set; }
        public int EcoFriendlyProductCount { get; set; }
        public int NonEcoFriendlyProductCount { get; set; }
        public List<string> TransportModeLabels { get; set; } = new();
        public List<int> TransportModeCounts { get; set; } = new();
        public List<string> EmissionTrendLabels { get; set; }
        public List<float> EmissionTrendValues { get; set; }
        public Dictionary<string, float> EmissionTrendDaily { get; set; }
        public Dictionary<string, float> EmissionTrendWeekly { get; set; }
        public Dictionary<string, float> EmissionTrendMonthly { get; set; }
        public Dictionary<string, int> ProductEcoBreakdown { get; set; }
        public Dictionary<string, int> OrderTransportBreakdown { get; set; }
        public Dictionary<string, float> EmissionTrendOverTime { get; set; }
    }
}
