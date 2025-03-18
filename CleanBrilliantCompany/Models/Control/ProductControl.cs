using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;


namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery, iProduct
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

        public async Task<List<ProductBatch>> getAllProductBatch()
        {
            var (status, batchList) = await _productMapper.getDatabaseQueryStatus(_productMapper.findAllProductBatch());
            Console.WriteLine($"Query Status: {status}");
            return batchList; 
        }

        public async Task<ProductBatch> getBatchDetails(int batchCode) 
        {
            return await _productMapper.getDatabaseQueryStatus(_productMapper.findByBatchCode(batchCode));
        }

        public async Task<string> createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            return await _productMapper.getDatabaseQueryStatus(
                _productMapper.insert(productId, expiryDate, 
                    receiveDate, manufactureDate, quantity, batchCost)
            );
        }

        public async Task<(string status, List<StockHistory> stockHistory)> getStockHistoryByBatch(int batchCode)
        {
            return await _productMapper.getDatabaseQueryStatus(
                _productMapper.findStockHistoryByBatchCode(batchCode)
                );

            //turn into dict before sending it out. WILL FIX FEATURES FIRST
        }
    }
}