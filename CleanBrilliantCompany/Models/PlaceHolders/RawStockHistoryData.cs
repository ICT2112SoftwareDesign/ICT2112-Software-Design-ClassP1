public class RawStockHistoryData
{
    public int StockId { get; set; }  // Primary Key
    public int BatchCode { get; set; } // Foreign Key linking to Batch
    public DateOnly Date { get; set; } // Unique, represents stock snapshot date
    public int Quantity { get; set; } // Remaining quantity in stock on that date
    public DateTime RecordedAt { get; set; } // When the record was logged

    // 🔹 Constructor for Initialization
    public RawStockHistoryData(int stockId, int batchCode, DateOnly date, int quantity, DateTime recordedAt)
    {
        StockId = stockId;
        BatchCode = batchCode;
        Date = date;
        Quantity = quantity;
        RecordedAt = recordedAt;
    }
}
