
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
        public DateTime StockCheckDate { get; set; } 
        public int Quantity { get; set; } 
        public DateTime TimeRecorded { get; set; } 

        public StockHistory(int stockId, int batchCode, DateTime stockCheckDate, int quantity, DateTime timeRecorded)
        {
            StockId = stockId;
            BatchCode = batchCode;
            StockCheckDate = stockCheckDate;
            Quantity = quantity;
            TimeRecorded = timeRecorded;
        }

        public StockHistory() { }
    }
}
