public class FakeBatchInterface1 {
    private Dictionary<int, RawBatchData> fakeBatches;
    private Dictionary<int, List<RawStockHistoryData>> fakeStockHistory;


    public FakeBatchInterface1()
    {

    fakeBatches = new Dictionary<int, RawBatchData>()
    {
        { 
            1, new RawBatchData(
                batchCode: 1, 
                productId: 2, 
                expiryDate: new DateTime(2021, 12, 31), 
                receiveDate: new DateTime(2021, 01, 01), 
                manufactureDate: new DateTime(2021, 01, 01), 
                quantity: 450, 
                salesPrice: 0) 
            },
        { 
            2, new RawBatchData(
                batchCode: 2, 
                productId: 14, 
                expiryDate: new DateTime(2025, 04, 18), 
                receiveDate: new DateTime(2025, 03, 12), 
                manufactureDate: new DateTime(2025, 02, 05), 
                quantity: 20, 
                salesPrice: 60) 
            },
        { 
            3, new RawBatchData(
                batchCode: 3, 
                productId: 2, 
                expiryDate: new DateTime(2025, 04, 26), 
                receiveDate: new DateTime(2025, 03, 11), 
                manufactureDate: new DateTime(2025, 02, 02), 
                quantity: 30, 
                salesPrice: 200) 
            },
        { 
            4, new RawBatchData(
                batchCode: 4, 
                productId: 2, 
                expiryDate: new DateTime(2025, 04, 11), 
                receiveDate: new DateTime(2025, 03, 13), 
                manufactureDate: new DateTime(2025, 02, 25), 
                quantity: 300, 
                salesPrice: 600) 
            },
        { 
            5, new RawBatchData(
                batchCode: 5, 
                productId: 14, 
                expiryDate: new DateTime(2025, 04, 19), 
                receiveDate: new DateTime(2025, 03, 11), 
                manufactureDate: new DateTime(2025, 02, 01), 
                quantity: 800, 
                salesPrice: 10000) 
            },
        { 
            6, new RawBatchData(
                batchCode: 6, 
                productId: 2, 
                expiryDate: new DateTime(2025, 04, 10), 
                receiveDate: new DateTime(2025, 03, 11), 
                manufactureDate: new DateTime(2025, 02, 15), 
                quantity: 200, 
                salesPrice: 80) 
            },
        { 
            7, new RawBatchData(
                batchCode: 7, 
                productId: 23, 
                expiryDate: new DateTime(2027, 06, 20), 
                receiveDate: new DateTime(2025, 03, 12), 
                manufactureDate: new DateTime(2025, 03, 01), 
                quantity: 100, 
                salesPrice: 100) 
            },
        {
            113, new RawBatchData(
                batchCode: 113, 
                productId: 990, 
                expiryDate: new DateTime(2027, 06, 20), 
                receiveDate: new DateTime(2025, 03, 12), 
                manufactureDate: new DateTime(2025, 03, 01), 
                quantity: 20, 
                salesPrice: 100)
        },
        {
            112, new RawBatchData(
                batchCode: 112, 
                productId: 909, 
                expiryDate: new DateTime(2027, 06, 20), 
                receiveDate: new DateTime(2025, 03, 12), 
                manufactureDate: new DateTime(2025, 03, 01), 
                quantity: 20, 
                salesPrice: 100)

        }
            
    };

    // Initialize fake stock history data
    fakeStockHistory = new Dictionary<int, List<RawStockHistoryData>>()
    {
        { 1, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(
                    stockId:1, 
                    batchCode:1, 
                    date:new DateOnly(2021, 12, 30), 
                    quantity:450, 
                    recordedAt:new DateTime(2021, 12, 30)
                    ),
                new RawStockHistoryData(2, 1, new DateOnly(2021, 12, 31), 100, new DateTime(2021, 12, 31))
            }
        },
        { 2, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(3, 2, new DateOnly(2025, 04, 17), 50, new DateTime(2025, 03, 12)),
                new RawStockHistoryData(4, 2, new DateOnly(2025, 04, 18), 20, new DateTime(2025, 03, 12))
            }
        },
        { 3, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(5, 3, new DateOnly(2025, 04, 25), 60, new DateTime(2025, 03, 11)),
                new RawStockHistoryData(6, 3, new DateOnly(2025, 04, 26), 30, new DateTime(2025, 03, 11))
            }
        },
        { 4, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(7, 4, new DateOnly(2025, 04, 10), 300, new DateTime(2025, 03, 13)),
                new RawStockHistoryData(8, 4, new DateOnly(2025, 04, 11), 10, new DateTime(2025, 03, 13))
            }
        },
        { 5, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(9, 5, new DateOnly(2025, 04, 18), 800, new DateTime(2025, 03, 11)),
                new RawStockHistoryData(10, 5, new DateOnly(2025, 04, 19), 70, new DateTime(2025, 03, 11))
            }
        },
        { 6, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(11, 6, new DateOnly(2025, 04, 17), 200, new DateTime(2025, 03, 11)),
                new RawStockHistoryData(12, 6, new DateOnly(2025, 04, 18), 50, new DateTime(2025, 03, 11))
            }
        },
        { 7, new List<RawStockHistoryData>
            {
                new RawStockHistoryData(13, 7, new DateOnly(2005, 06, 19), 100, new DateTime(2025, 03, 12)),
                new RawStockHistoryData(14, 7, new DateOnly(2005, 06, 20), 20, new DateTime(2025, 03, 12))
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
    public Dictionary<int, RawStockHistoryData> getStockHistoryByDate(DateOnly stocktakeDate)
    {
        var result = new Dictionary<int, RawStockHistoryData>();

        foreach (var batchHistory in fakeStockHistory)
        {
            var stockRecord = batchHistory.Value.FirstOrDefault(sh => sh.Date == stocktakeDate);
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
