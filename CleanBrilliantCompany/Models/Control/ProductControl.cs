using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;


namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery
    {
        private static List<Product> _products = new List<Product>();
        private readonly ProductMapper _productMapper;

         public ProductControl(string connectionString)
        {
            _productMapper = new ProductMapper(connectionString);

            // Load products from the database at startup
            _products = GetAllProducts();
            Console.WriteLine("Products loaded from database.");
        }
        
        public void CreateProduct(Product product)
        {
            // Insert the new product into the database
            _productMapper.createProduct(product.ProductName, product.ProductCategory, 
                                        product.CostPrice, product.ManufacturerId, product.ProductWeight, 
                                        product.Quantity, product.Volume, product.ToxicityPercentage, 
                                        product.CarbonFootprint, product.ProductState);

            // Reload products from database after insertion to reflect the change in UI
            _products = GetAllProducts();

            Console.WriteLine($"Product '{product.ProductName}' created successfully and added to the database.");

            // _products.Add(product);
            // Console.WriteLine($"Product '{product.ProductName}' created successfully!");
        }

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

        public void createProduct(string productName, string category, float costPrice, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            _productMapper.createProduct(productName, category, costPrice, 
                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);
        }
    }
}