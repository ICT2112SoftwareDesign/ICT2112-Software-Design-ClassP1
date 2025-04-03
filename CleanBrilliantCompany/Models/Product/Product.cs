using System;

namespace CleanBrilliantCompany.Models
{
    public class Product
    {
        private int productId;
        private string productName;
        private string category;
        private float costPrice;
        private int manufacturerId;
        private string package;
        private float weight;
        private int toxicityPercentage;
        private int quantity;
        private int carbonFootprint;
        private int volume;

        private int GetProductId() => productId;
        private void SetProductId(int value) => productId = value;

        private string GetProductName() => productName;
        private void SetProductName(string value) => productName = value;

        private string GetCategory() => category;
        private void SetCategory(string value) => category = value;

        private float GetCostPrice() => costPrice;
        private void SetCostPrice(float value) => costPrice = value;

        private int GetManufacturerId() => manufacturerId;
        private void SetManufacturerId(int value) => manufacturerId = value;

        private string GetPackage() => package;
        private void SetPackage(string value) => package = value;

        private float GetWeight() => weight;
        private void SetWeight(float value) => weight = value;

        private int GetToxicityPercentage() => toxicityPercentage;
        private void SetToxicityPercentage(int value) => toxicityPercentage = value;

        private int GetQuantity() => quantity;
        private void SetQuantity(int value) => quantity = value;

        private int GetCarbonFootprint() => carbonFootprint;
        private void SetCarbonFootprint(int value) => carbonFootprint = value;

        private int GetVolume() => volume;
        private void SetVolume(int value) => volume = value;

        public Product(int productId, string productName, string category, float costPrice, int manufacturerId, string package, float weight, int toxicityPercentage, int quantity, int carbonFootprint, int volume)
        {
            this.productId = productId;
            this.productName = productName;
            this.category = category;
            this.costPrice = costPrice;
            this.manufacturerId = manufacturerId;
            this.package = package;
            this.weight = weight;
            this.toxicityPercentage = toxicityPercentage;
            this.quantity = quantity;
            this.carbonFootprint = carbonFootprint;
            this.volume = volume;
        }

        public Dictionary<string, object> GetProductDetails()
        {
            return new Dictionary<string, object>
            {
                { "ProductId", productId },
                { "ProductName", productName },
                { "Category", category },
                { "CostPrice", costPrice },
                { "ManufacturerId", manufacturerId },
                { "Package", package },
                { "Weight", weight },
                { "ToxicityPercentage", toxicityPercentage },
                { "Quantity", quantity },
                { "CarbonFootprint", carbonFootprint },
                { "Volume", volume }
            };
        }

        public int GetProductID()
        {
            return productId;
        }
    }
}