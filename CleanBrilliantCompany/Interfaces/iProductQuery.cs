using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductQuery
    {
        // Product
        Task<Product> getProductDetails(int productId);
        Task<(string status, List<Product> products)> getAllProducts();
        Task<string> createProduct(string productName, string category, float productCost, 
            int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState);
        Task<string> deleteProduct(int productId);
        // Product Batch
        Task<List<ProductBatch>> getAllProductBatch();
        Task<ProductBatch> getBatchDetails(int batchCode);
        Task<string> createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost);
        // Stock History
        Task<(string status, List<StockHistory> stockHistory)> getStockHistoryByBatch(int batchCode);
    }
}