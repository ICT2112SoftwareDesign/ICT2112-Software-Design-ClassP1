namespace CleanBrilliantCompany.Models.Entity
{
    public class Transaction
    {

        //Private Attributes here
        private int TransactionId;
        private DateTime DateTime;
        private string AdjustmentType;
        private int ProductId;
        private int ItemId;
        private int StaffId;

        private string ProductName;

        public Transaction(int transactionId, DateTime dateTime, string adjustmentType, int productId, int itemId, int staffId)
        {
            TransactionId = transactionId;
            DateTime = dateTime;
            AdjustmentType = adjustmentType;
            ProductId = productId;
            ItemId = itemId;
            StaffId = staffId;
        }

        public Transaction(int transactionId, DateTime dateTime, string adjustmentType, string productName, int productId, int itemId, int staffId)
        {
            TransactionId = transactionId;
            DateTime = dateTime;
            AdjustmentType = adjustmentType;
            ProductName = productName;
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
                { "StaffId", StaffId},
                { "ProductName", ProductName}

            };
        }


        public Transaction() { }
    }
}
