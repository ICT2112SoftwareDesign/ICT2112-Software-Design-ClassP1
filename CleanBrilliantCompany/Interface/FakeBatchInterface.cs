public class FakeBatchInterface
{
    private readonly SimulatedDbContext _context;

    public FakeBatchInterface(SimulatedDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    } 
    public RawBatchData? getBatchDetails(int batchCode)
    {
        var batch = _context.Batches.FirstOrDefault(b => b.batchCode == batchCode);
        if (batch == null) return null;

        return new RawBatchData(
            batch.batchCode,
            batch.productId,
            batch.expiryDate,
            batch.receiveDate,
            batch.manufactureDate,
            batch.quantity,
            batch.batchCost
        );
    }

    public List<RawBatchData> getAllProductBatch()
    {
        return _context.Batches
            .Select(b => new RawBatchData(
                b.batchCode,
                b.productId,
                b.expiryDate,
                b.receiveDate,
                b.manufactureDate,
                b.quantity,
                b.batchCost
            ))
            .ToList();
    }
    public Dictionary<int, RawStockHistoryData> getStockHistoryByDate(DateOnly stocktakeDate)
    {
        var result = new Dictionary<int, RawStockHistoryData>();

        var matchingStocks = _context.StockHistories
            .Where(sh => sh.stockTakeDate == stocktakeDate) 
            .ToList();

        foreach (var stock in matchingStocks)
        {
            result[stock.batchCode] = new RawStockHistoryData(
                stockId: stock.stockId,
                batchCode: stock.batchCode,
                date: stock.stockTakeDate,
                quantity: stock.quantity, 
                recordedAt: stock.recordedDate

            );
        }

        return result;
    }

    public List<RawStockHistoryData> getStockHistoryByBatch(int batchCode)
    {
        return _context.StockHistories
            .Where(sh => sh.batchCode == batchCode)
            // so this are apparently sql queries
            // so i cannot use the param names in the query 
            .Select(sh => new RawStockHistoryData(
                sh.stockId,
                sh.batchCode,
                sh.stockTakeDate,
                sh.quantity,
                sh.recordedDate
            ))
            .ToList();
    }


}