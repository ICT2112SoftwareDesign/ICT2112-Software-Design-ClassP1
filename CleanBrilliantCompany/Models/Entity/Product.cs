namespace CleanBrilliantCompany.Models.Entity
{
    public class Product
    {
        private int ProductId;
        private string ProductName;
        private string ProductCategory;
        private float ProductCost;
        private int ManufacturerId;
        private float ProductWeight;
        private int Quantity;
        private int Volume;
        private float ToxicityPercentage;
        private int CarbonFootprint;
        private string ProductState;

        // Product Stock, i keep public for now
        public int TotalQuantity { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public int MaxCapacity { get; set; } // Max capacity of the warehouse
        public int CurrentCapacity { get; set; } // Current capacity of the warehouse
        public int AvailableCapacity { get; set; } // Available capacity of the warehouse

        public Dictionary<string, object> retrieveLowStockInfo()
        {
            return new Dictionary<string, object>
            {
                { "ProductId", ProductId },
                { "ProductName", ProductName },
                { "TotalQuantity", TotalQuantity },
                { "WarehouseId", WarehouseId },
                { "WarehouseName", WarehouseName },
                { "MaxCapacity", MaxCapacity },
                { "CurrentCapacity", CurrentCapacity },
                { "AvailableCapacity", AvailableCapacity }
            };
        }

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

        public Product(int productId, string productName, string warehouseName, int warehouseId, int totalQuantity, int maxCapacity, int currentCapacity, int availableCapacity)
        {
            ProductId = productId;
            ProductName = productName;
            WarehouseName = warehouseName;
            WarehouseId = warehouseId;
            TotalQuantity = totalQuantity;
            MaxCapacity = maxCapacity;
            CurrentCapacity = currentCapacity;
            AvailableCapacity = availableCapacity;
        }

        public Dictionary<string, object> retrieveProductInfo()
        {
            return new Dictionary<string, object>
            {
                { "ProductId", ProductId },
                { "ProductName", ProductName },
                { "ProductCategory", ProductCategory },
                { "ProductCost", ProductCost },
                { "ManufacturerId", ManufacturerId },
                { "ProductWeight", ProductWeight },
                { "Quantity", Quantity },
                { "Volume", Volume },
                { "ToxicityPercentage", ToxicityPercentage },
                { "CarbonFootprint", CarbonFootprint },
                { "ProductState", ProductState }
            };
        }

        // Private Getters
        private int getProductId() => ProductId;
        private string getProductName() => ProductName;
        private string getProductCategory() => ProductCategory;
        private float getProductCost() => ProductCost;
        private int getManufacturerId() => ManufacturerId;
        private float getProductWeight() => ProductWeight;
        private int getQuantity() => Quantity;
        private int getVolume() => Volume;
        private float getToxicityPercentage() => ToxicityPercentage;
        private int getCarbonFootprint() => CarbonFootprint;
        private string getProductState() => ProductState;

        // Private Setters 
        private void setProductId(int productId) => ProductId = productId;
        private void setProductName(string productName) => ProductName = productName;
        private void setProductCategory(string productCategory) => ProductCategory = productCategory;
        private void setProductCost(float productCost) => ProductCost = productCost;
        private void setManufacturerId(int manufacturerId) => ManufacturerId = manufacturerId;
        private void setProductWeight(float productWeight) => ProductWeight = productWeight;
        private void setQuantity(int quantity) => Quantity = quantity;
        private void setVolume(int volume) => Volume = volume;
        private void setToxicityPercentage(float toxicityPercentage) => ToxicityPercentage = toxicityPercentage;
        private void setCarbonFootprint(int carbonFootprint) => CarbonFootprint = carbonFootprint;
        private void setProductState(string productState) => ProductState = productState;

        public Product() { }
    }
}
