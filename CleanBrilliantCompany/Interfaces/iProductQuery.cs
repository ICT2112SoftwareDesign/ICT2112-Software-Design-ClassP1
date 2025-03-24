using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductQuery
    {
        // Product
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
        void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState);
        void deleteProduct(int productId);
        public void updateProduct(int productId, string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState);

        // Product Batch
        List<ProductBatch> getAllProductBatch();
        ProductBatch getBatchDetails(int batchCode); 
        public void createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost);

        // Stock History
        // List<StockHistory> getStockHistoryByBatch(int batchCode);
    }
}