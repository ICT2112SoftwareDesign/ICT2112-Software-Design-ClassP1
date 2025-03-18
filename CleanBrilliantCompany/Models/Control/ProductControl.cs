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
            return await _productMapper.getDatabaseQueryStatus(_productMapper.findAllProducts());
        }

        public async Task<string> createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            return await _productMapper.getDatabaseQueryStatus(
                _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState)
            );
        }

        public async Task<string> deleteProduct(int productId)
        {
            return await _productMapper.getDatabaseQueryStatus(_productMapper.delete(productId));
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