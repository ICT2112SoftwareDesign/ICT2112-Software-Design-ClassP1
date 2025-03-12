namespace CleanBrilliantCompany.Models.Entity
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public float CostPrice { get; set; }
        public int ManufacturerId { get; set; }
        public float ProductWeight { get; set; }
        public int Quantity { get; set; } = 0;
        public int Volume { get; set; }
        public float ToxicityPercentage { get; set; }
        public int CarbonFootprint { get; set; }
        public int ProductState { get; set; }


        public Product(int productId, string productName, string productCategory, float costPrice, int manufacturerId,
                       float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            ProductId = productId;
            ProductName = productName;
            ProductCategory = productCategory;
            CostPrice = costPrice;
            ManufacturerId = manufacturerId;
            ProductWeight = productWeight;
            Quantity = quantity;
            Volume = volume;
            ToxicityPercentage = toxicityPercentage;
            CarbonFootprint = carbonFootprint;
            ProductState = productState;
        }

        public Product() { }
    }
}