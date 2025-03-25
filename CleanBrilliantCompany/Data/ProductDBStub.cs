using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Data
{
    public class ProductDBStub : IProduct
    {
        private List<Product> productsDB;

        public ProductDBStub()
        {
            productsDB =
            [
                new() { ProductId = 1, ProductName = "Disinfectant Spray", ProductCategory = "Spray", ProductCost = 5.99f, ManufacturerId = 201, ProductWeight = 0.5f, Quantity = 50, Volume = 100, ToxicityPercentage = 0.25f, CarbonFootprint = 25, ProductState = "Out of Stock" },
                new() { ProductId = 2, ProductName = "Multipurpose Cleaner", ProductCategory = "Cleaner", ProductCost = 4.50f, ManufacturerId = 202, ProductWeight = 1.0f, Quantity = 30, Volume = 150, ToxicityPercentage = 0.12f, CarbonFootprint = 18, ProductState = "Ready" },
                new() { ProductId = 3, ProductName = "Glass Cleaner", ProductCategory = "Cleaner", ProductCost = 3.75f, ManufacturerId = 203, ProductWeight = 0.75f, Quantity = 40, Volume = 120, ToxicityPercentage = 0.18f, CarbonFootprint = 22, ProductState = "Ready" },
                new() { ProductId = 4, ProductName = "Disinfectant Spray Premium", ProductCategory = "Spray", ProductCost = 5.99f, ManufacturerId = 201, ProductWeight = 0.5f, Quantity = 50, Volume = 100, ToxicityPercentage = 0.23f, CarbonFootprint = 23, ProductState = "Out of Stock" },
                new() { ProductId = 5, ProductName = "Multipurpose Cleaner Premium", ProductCategory = "Cleaner", ProductCost = 4.50f, ManufacturerId = 202, ProductWeight = 1.0f, Quantity = 30, Volume = 150, ToxicityPercentage = 0.11f, CarbonFootprint = 17, ProductState = "Ready" },
                new() { ProductId = 6, ProductName = "Glass Cleaner Premium", ProductCategory = "Cleaner", ProductCost = 3.75f, ManufacturerId = 203, ProductWeight = 0.75f, Quantity = 40, Volume = 120, ToxicityPercentage = 0.15f, CarbonFootprint = 18, ProductState = "Ready" }
            ];
        }

        public List<Product> GetAllProducts()
        {
            return productsDB;
        }

        public Product GetProductDetails(int productId)
        {
            for (int i = 0; i < productsDB.Count; i++)
            {
                if (productsDB[i].ProductId == productId)
                {
                    return productsDB[i];
                }
            }

            return null;
        }
    }
}
