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
        public Product getProductDetails(int productId)
        {
            return _productMapper.findByProductId(productId);
        }

        public List<Product> getAllProducts()
        {
            return _productMapper.findall();
        }

        public void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            try
            {
                
                _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);


                Console.WriteLine($"Product '{productName}' created successfully and added to the database.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting product: {ex.Message}");
            }
        }
    }
}