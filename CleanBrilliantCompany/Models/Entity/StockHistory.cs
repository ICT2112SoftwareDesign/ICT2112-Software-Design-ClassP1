
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

        private int StockId;
        private int BatchCode;
        private DateOnly StockTakeDate;
        private int Quantity;
        private DateTime RecordedDate;

        public StockHistory(int stockId, int batchCode, DateOnly stockTakeDate, int quantity, DateTime recordedDate)
        {
            StockId = stockId;
            BatchCode = batchCode;
            StockTakeDate = stockTakeDate;
            Quantity = quantity;
            RecordedDate = recordedDate;
        }

        public Dictionary<string, object> retrieveStockHistory()
        {
            return new Dictionary<string, object>
            {
                { "StockId", StockId },
                { "BatchCode", BatchCode },
                { "StockTakeDate", StockTakeDate },
                { "Quantity", Quantity },
                { "RecordedDate", RecordedDate }
            };
        }

        // Getters
        private int GetStockId() => StockId;
        public int GetBatchCode() => BatchCode; // i made it public, bruh how to do if private
        public DateOnly GetStockTakeDate() => StockTakeDate; // i made it public
        private int GetQuantity() => Quantity;
        private DateTime GetRecordedDate() => RecordedDate;

        // Setters
        private void SetStockId(int value) => StockId = value;
        private void SetBatchCode(int value) => BatchCode = value;
        private void SetStockTakeDate(DateOnly value) => StockTakeDate = value;
        private void SetQuantity(int value) => Quantity = value;
        private void SetRecordedDate(DateTime value) => RecordedDate = value;

        public StockHistory() { }
    }
}
