namespace CleanBrilliantCompany.Models.Entity
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public DateTime DateTime { get; set; }
        
        //For now, take it as AdjustmentType must not always be present --> hence we do not have it as required
        public string AdjustmentType { get; set; }
        public int ItemId { get; set; }

        public int StaffId { get; set;}

        public Transaction(int transactionId, DateTime dateTime, string adjustmentType, int itemId, int staffId)
        {
            TransactionId = transactionId;
            DateTime = dateTime;
            AdjustmentType = adjustmentType;
            ItemId = itemId;
            StaffId = staffId;
        }

        public Transaction() { }
    }
}
