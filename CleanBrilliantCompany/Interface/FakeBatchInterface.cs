public class FakeBatchInterface {
    private Dictionary<int, RawBatchData> fakeBatches;
    private Dictionary<int, List<RawStockHistoryData>> fakeStockHistory;


    public FakeBatchInterface()
    {

        fakeBatches = new Dictionary<int , RawBatchData>()
        {
        
        { 1001, new RawBatchData(1001, 501, DateTime.Now.AddMonths(6), DateTime.Now.AddDays(-30), DateTime.Now.AddMonths(-1), 100, 15.5f) },
        { 1002, new RawBatchData(1002, 502, DateTime.Now.AddMonths(3), DateTime.Now.AddDays(-60), DateTime.Now.AddMonths(-2), 50, 22.0f) },
        { 1003, new RawBatchData(1003, 503, DateTime.Now.AddMonths(9), DateTime.Now.AddDays(-10), DateTime.Now.AddMonths(-3), 200, 10.75f) }
        
        };
            // Initialize fake stock history data
        fakeStockHistory = new Dictionary<int, List<RawStockHistoryData>>()
        {
            { 1001, new List<RawStockHistoryData>
                {
                    new RawStockHistoryData(1, 1001, DateTime.Now.AddDays(-10), 80, DateTime.Now.AddDays(-10)),
                    new RawStockHistoryData(2, 1001, DateTime.Now.AddDays(-5), 50, DateTime.Now.AddDays(-5))
                }
            },
            { 1002, new List<RawStockHistoryData>
                {
                    new RawStockHistoryData(3, 1002, DateTime.Now.AddDays(-8), 30, DateTime.Now.AddDays(-8))
                }
            },
            { 1003, new List<RawStockHistoryData>
                {
                    new RawStockHistoryData(4, 1003, DateTime.Now.AddDays(-12), 150, DateTime.Now.AddDays(-12))
                }
            }
        };
    }


      // Retrieve batch details based on batch code
    public RawBatchData getBatchDetails(int batchCode)
    {
        return fakeBatches.ContainsKey(batchCode) ? fakeBatches[batchCode] : null;
    }

    // Retrieve all available product batches
    public List<RawBatchData> getAllProductBatch()
    {
        return fakeBatches.Values.ToList();
    }

    // Retrieve stock history by stocktake date
    public Dictionary<int, RawStockHistoryData> getStockHistoryByDate(DateTime stocktakeDate)
    {
        var result = new Dictionary<int, RawStockHistoryData>();

        foreach (var batchHistory in fakeStockHistory)
        {
            var stockRecord = batchHistory.Value.FirstOrDefault(sh => sh.Date.Date == stocktakeDate.Date);
            if (stockRecord != null)
            {
                result[batchHistory.Key] = stockRecord;
            }
        }

        return result;
    }

    // Retrieve stock history by batch code
    public List<RawStockHistoryData> getStockHistoryByBatch(int batchCode)
    {
        return fakeStockHistory.ContainsKey(batchCode) ? fakeStockHistory[batchCode] : new List<RawStockHistoryData>();
    }
}
