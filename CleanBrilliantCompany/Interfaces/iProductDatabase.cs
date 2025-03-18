using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductDatabase
    {
        // Product
        Task<Product> getDatabaseQueryStatus(Task<Product> task);
        Task<(string status, List<Product> products)> getDatabaseQueryStatus(Task<List<Product>> task);
        Task<string> getDatabaseQueryStatus(Task<string> task);
        // ProductBatch
        Task<(string status, List<ProductBatch> batch)> getDatabaseQueryStatus(Task<List<ProductBatch>> batch);
        Task<ProductBatch> getDatabaseQueryStatus(Task<ProductBatch> task);
        // StockHistory
        Task<(string status, List<StockHistory> stockHistory)> getDatabaseQueryStatus(Task<List<StockHistory>> task);
    }
}