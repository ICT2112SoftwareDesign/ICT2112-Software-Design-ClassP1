using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;


namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery
    {
        private static List<Product> _products = new List<Product>();
        private readonly ProductMapper _productMapper;

        // private readonly iProductQuery _productQuery;

        public ProductControl()
        {
            // _productQuery = productQuery;
            _productMapper = new ProductMapper("your_connection_string");

            // Load sample products at startup
            _products = GetAllProducts();
            Console.WriteLine("Sample products loaded into system.");
        }
        
        public void CreateProduct(Product product)
        {
            _products.Add(product);
            Console.WriteLine($"Product '{product.ProductName}' created successfully!");
        }

        // public Product GetProductById(int productId)
        // {
        //     return _productQuery.getProductDetails(productId);
        // }

        public List<Product> GetProducts()
        {
            return _products;
        }

        // Interface methods
        public Product getProductDetails(int productId)
        {
            return _productMapper.getProductDetails(productId);
        }

        public List<Product> GetAllProducts()
        {
            return _productMapper.GetAllProducts();
        }
    }
}