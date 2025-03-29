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
        public List<string> TopEcoProductLabels { get; set; }
        public List<double> TopEcoProductValues { get; set; }
        public List<string> ProductCategoryLabels { get; set; }
        public List<double> ProductCategoryValues { get; set; }
        public float ProductPastWeekCF { get; set; }
        public float ProductPastMonthCF { get; set; }

        public float ItemPastWeekCF { get; set; }
        public float ItemPastMonthCF { get; set; }

        public float OrderPastWeekCF { get; set; }
        public float OrderPastMonthCF { get; set; }


        // New properties for comparison tool
        public List<ProductComparisonData> Products { get; set; } = new();
        public List<ShippingMethodComparisonData> ShippingMethods { get; set; } = new();

        // For product and item mockup purposes
        public List<Product> RandomProductList { get; set; } = new List<Product>();
        public List<Item> RandomItemList { get; set; } = new List<Item>();
    }

    public class ProductComparisonData
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double CarbonEmission { get; set; }
    }

    public class ShippingMethodComparisonData
    {
        public string TransportMode { get; set; }
        public double AverageCarbonEmission { get; set; }
    }
}
