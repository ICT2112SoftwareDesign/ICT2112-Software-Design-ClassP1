namespace CleanBrilliantCompany.Models.Entity
{
    public class Product
    {
        // CLASS DIAGRAM
        // - productId: Int
        // - productName: String
        // - category: String
        // - costPrice: Float
        // - manufacturerId: Int
        // - weight: Float
        // - quantity: Int
        // - volume: Int
        // - toxicityPercentage: Int
        // - carbonFootprint: Int
        // - ProductState: String


        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCategory { get; set; }
        public float ProductCost { get; set; }
        public int ManufacturerId { get; set; }
        public float ProductWeight { get; set; }
        public int Quantity { get; set; } = 0;
        public int Volume { get; set; }
        public float ToxicityPercentage { get; set; }
        public int CarbonFootprint { get; set; }
        public string ProductState { get; set; } // Change to string 


        public Product(int productId, string productName, string productCategory, float productCost, int manufacturerId,
                       float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            ProductId = productId;
            ProductName = productName;
            ProductCategory = productCategory;
            ProductCost = productCost;
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
