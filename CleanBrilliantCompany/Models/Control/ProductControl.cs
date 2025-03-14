using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;


namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery
    {
        private readonly ProductMapper _productMapper;

         public ProductControl(string connectionString)
        {
            _productMapper = new ProductMapper(connectionString);

            Console.WriteLine("Products loaded from database.");
        }

        // Interface methods
        public async Task<Product> getProductDetails(int productId)
        {
            return await _productMapper.getDatabaseQueryStatus(_productMapper.findByProductId(productId));
        }

        public async Task<(string status, List<Product> products)> getAllProducts()
        {
            return await _productMapper.getDatabaseQueryStatus(_productMapper.findAll());
        }

        public async Task<string> createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            return await _productMapper.getDatabaseQueryStatus(
                _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState)
            );
        }
    }
}