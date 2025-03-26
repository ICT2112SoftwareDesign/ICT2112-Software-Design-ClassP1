using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iBatch
    {
        public List<StockHistory> getStockHistoryByBatch(int batchCode);
        public Dictionary<int, List<StockHistory>> getStockHistoryByDate(DateOnly stockTakeDate);
    }
}