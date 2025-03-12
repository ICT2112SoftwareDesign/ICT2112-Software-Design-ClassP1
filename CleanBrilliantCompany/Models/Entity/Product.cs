namespace CleanBrilliantCompany.Models.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
        public float CostPrice { get; set; }
        public int ManufacturerId { get; set; }
        public float Weight { get; set; }
        public int Quantity { get; set; } = 0;
        public int Volume { get; set; }
        public int ToxicityPercentage { get; set; }
        public int CarbonFootprint { get; set; }


        public Product(int productId, string productName, string category, float costPrice, int manufacturerId,
                       float weight, int quantity, int volume, int toxicityPercentage, int carbonFootprint)
        {
            ProductId = productId;
            ProductName = productName;
            Category = category;
            CostPrice = costPrice;
            ManufacturerId = manufacturerId;
            Weight = weight;
            Quantity = quantity;
            Volume = volume;
            ToxicityPercentage = toxicityPercentage;
            CarbonFootprint = carbonFootprint;
        }

        public Product() { }
    }
}