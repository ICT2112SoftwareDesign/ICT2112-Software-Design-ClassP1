
namespace CleanBrilliantCompany.Models.Entity
{
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
        public int GetBatchCode() => BatchCode; 
        public DateOnly GetStockTakeDate() => StockTakeDate; 
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
