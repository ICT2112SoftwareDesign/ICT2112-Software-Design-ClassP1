namespace CleanBrilliantCompany.Models.Entity
{
    public enum adjustmentType
    {
        //TODO
    }
    public class Transaction
    {
        // public int TransactionId { get; set; }
        // public DateTime DateTime { get; set; }
        
        // //For now, take it as AdjustmentType must not always be present --> hence we do not have it as required
        // public string AdjustmentType { get; set; }
        // public int ItemId { get; set; }

        // public int StaffId { get; set;}

        private int TransactionId;
        private DateTime DateTime;
        private string AdjustmentType;
        private int ProductId;
        private int ItemId;
        private int StaffId;

        public Transaction(int transactionId, DateTime dateTime, string adjustmentType, int productId, int itemId, int staffId)
        {
            TransactionId = transactionId;
            DateTime = dateTime;
            AdjustmentType = adjustmentType;
            ProductId = productId;
            ItemId = itemId;
            StaffId = staffId;
        }

        // Private Getters
        private int getTransactionId() => TransactionId;
        private DateTime getDateTime() => DateTime;
        private string getAdjustmentType() => AdjustmentType;
        private int getProductId() => ProductId;
        private int getItemId() => ItemId;
        private int getStaffId() => StaffId;

        // Private Setters
        private void setTransactionId(int transactionId) => TransactionId = transactionId;
        private void setDateTime(DateTime dateTime) => DateTime = dateTime;
        private void setAdjustmentType(string adjustmentType) => AdjustmentType = adjustmentType;
        private void setProductId(int productId) => ProductId = productId;
        private void setItemId(int itemId) => ItemId = itemId;
        private void setStaffId(int staffId) => StaffId = staffId;

        public Dictionary<string, object> retrieveTransactionInfo()
        {
            return new Dictionary<string, object>
            {
                { "TransactionId", TransactionId },
                { "TransactionDateTime", DateTime },
                { "AdjustmentType", AdjustmentType },
                { "ProductId", ProductId },
                { "ItemId", ItemId },
                { "StaffId", StaffId}

            };
        }


        public Transaction() { }
    }
}
