using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductQuery
    {
        // Product
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
        void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState);
        void deleteProduct(int productId);
        public bool updateProduct(int productId, string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState);

        // Product Batch
        List<ProductBatch> getAllProductBatch();
        ProductBatch getBatchDetails(int batchCode); 
        public int createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, float batchCost);

        // Stock History
        // List<StockHistory> getStockHistoryByBatch(int batchCode);
    }
}