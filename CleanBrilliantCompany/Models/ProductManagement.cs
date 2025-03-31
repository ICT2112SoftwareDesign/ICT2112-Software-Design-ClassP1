using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class ProductManagement : IProduct
    {
        private List<Product> products = new List<Product>
        {
            new Product(1, "Generic Detergent", "Detergent", 10.5f, 101, "Package1", 1.2f, 5, 100, 10, 2),
            new Product(2, "Cheapo Brand Bleach", "Bleach", 20.0f, 102, "Package2", 2.5f, 10, 200, 20, 4),
            new Product(3, "Dynamo", "Detergent", 30.0f, 103, "Package3", 3.0f, 15, 300, 30, 6),
            new Product(4, "Ultra Germ Killa Bleach", "Bleach", 40.0f, 104, "Package4", 4.0f, 20, 400, 40, 8),
            new Product(5, "Fragrance-Free Air Freshener", "Personal Care", 12.0f, 103, "Package3", 3.0f, 15, 300, 30, 6),
            new Product(6, "All-Purpose Cleaner", "Household", 21.0f, 104, "Package4", 4.0f, 20, 400, 40, 8)
        };

        public Product getProductDetails(int productId)
        {
            return products.Find(p => p.GetProductID() == productId) 
                   ?? new Product(0, "Default", "Default", 0.0f, 0, "Default", 0.0f, 0, 0, 0, 0);
        }

        public List<Product> getAllProducts()
        {
            return products;
        }
    }
}