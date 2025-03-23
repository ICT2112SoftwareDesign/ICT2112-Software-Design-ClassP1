
namespace CleanBrilliantCompany.Models.Entity
{

    // IN CLASS DIAGRAM
//     - stockId: Int
    // - batchCode: Int
    // - stocktakeDate: Date
    // - quantity: Int
    // - recordedDate: DateTime


    // Current DB
    // [stockId]
//       ,[batchCode]
//       ,[stockCheckDate]
//       ,[quantity]
//       ,[timeRecorded]

    public class StockHistory
    {
        public int StockId { get; set; } 
        public int BatchCode { get; set; } 
        public DateOnly StockTakeDate { get; set; } 
        public int Quantity { get; set; } 
        public DateTime RecordedDate { get; set; } // Change to DateTime currently is Time in database, change name to recordedDate

        public StockHistory(int stockId, int batchCode, DateOnly stockTakeDate, int quantity, DateTime recordedDate)
        {
            StockId = stockId;
            BatchCode = batchCode;
            StockTakeDate = stockTakeDate;
            Quantity = quantity;
            RecordedDate = recordedDate;
        }

        public StockHistory() { }
    }
}
