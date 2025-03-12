using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
// using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Mappers
{
    public class ProductMapper
    {
        private readonly string _connectionString;

        public ProductMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Product getProductDetails(int productId)
        {
            Console.WriteLine($"Fetching product details for Product ID: {productId}");

            switch (productId)
            {
                case 1:
                    return new Product(1, "Laptop", "Electronics", 1200.50f, 2, 1.5f, 5, 10, 0, 15);
                case 2:
                    return new Product(2, "Smartphone", "Electronics", 899.99f, 3, 0.5f, 20, 2, 0, 8);
                case 3:
                    return new Product(3, "Tablet", "Electronics", 499.99f, 4, 0.8f, 15, 3, 0, 10);
                default:
                    return null;
            }
        }

        public List<Product> GetAllProducts()
        {
            Console.WriteLine("Fetching all products...");

            return new List<Product>
            {
                new Product(1, "Laptop", "Electronics", 1200.50f, 2, 1.5f, 5, 10, 0, 15),
                new Product(2, "Smartphone", "Electronics", 899.99f, 3, 0.5f, 20, 2, 0, 8),
                new Product(3, "Tablet", "Electronics", 499.99f, 4, 0.8f, 15, 3, 0, 10)
            };
        }
    }
}