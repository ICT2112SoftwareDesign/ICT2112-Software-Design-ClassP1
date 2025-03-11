using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl
    {
        private static List<Product> _products = new List<Product>();

        private readonly iProductQuery _productQuery;

        public ProductControl(iProductQuery productQuery)
        {
            _productQuery = productQuery;

            // Load sample products at startup
            _products = _productQuery.GetAllProducts();
            Console.WriteLine("Sample products loaded into system.");
        }
        
        public void CreateProduct(Product product)
        {
            _products.Add(product);
            Console.WriteLine($"Product '{product.ProductName}' created successfully!");
        }

        public Product GetProductById(int productId)
        {
            return _productQuery.getProductDetails(productId);
        }

        public List<Product> GetProducts()
        {
            return _products;
        }
    }
}