using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IBatch
    {
        public List<StockHistory> getStockHistoryByBatch(int batchCode);
        public Dictionary<int, List<StockHistory>> getStockHistoryByDate(DateOnly stockTakeDate);
        public List<ProductBatch> getAllProductBatch();
        public ProductBatch getBatchDetails(int batchCode);
    }
}