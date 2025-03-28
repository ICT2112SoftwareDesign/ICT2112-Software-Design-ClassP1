using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Models.ViewModel
{
    public class BatchDetailsViewModel
    {
        public List<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public List<StockHistory> StockHistory { get; set; } = new List<StockHistory>();
    }
}
