using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;


namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : IProductQuery, IProduct
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
            return  _productMapper.findByProductId(productId);
        }


        public List<Product> getAllProducts()
        {
            return  _productMapper.findAllProducts();
        }

        public void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);
        }

        public void deleteProduct(int productId)
        {
            _productMapper.delete(productId);
        }

        public void updateProduct(int productId, string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productMapper.update(productId, productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);;
        }

        public List<ProductBatch> getAllProductBatch()
        {
            return _productMapper.findAllProductBatch(); 
        }

        public ProductBatch getBatchDetails(int batchCode) 
        {
            return _productMapper.findByBatchCode(batchCode);
        }

        public void createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            _productMapper.insert(productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost);
        }

        public List<StockHistory> getStockHistoryByBatch(int batchCode)
        {
            return _productMapper.findStockHistoryByBatchCode(batchCode);

            //turn into dict before sending it out. WILL FIX FEATURES FIRST
        }
    }
}