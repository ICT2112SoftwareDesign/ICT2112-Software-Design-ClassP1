using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;
namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductDatabase
    {
        // Product
        Task<Prodluct> getDatabaseQueryStatus(Task<Prodluct> task);
        Task<(string status, List<Prodluct> products)> getDatabaseQueryStatus(Task<List<Prodluct>> task);
        Task<string> getDatabaseQueryStatus(Task<string> task);
        // ProductBatch
        Task<(string status, List<ProductBatch> batch)> getDatabaseQueryStatus(Task<List<ProductBatch>> batch);
        Task<ProductBatch> getDatabaseQueryStatus(Task<ProductBatch> task);
        // StockHistory
        Task<(string status, List<StockHistory> stockHistory)> getDatabaseQueryStatus(Task<List<StockHistory>> task);

        // Corrected
        bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}