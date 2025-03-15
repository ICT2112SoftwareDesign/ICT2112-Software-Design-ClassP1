namespace CleanBrilliantCompany.Models.Entity
{
    public class ProductBatch
    {
        // IN CLASS DIAGRAM
        // - batchCode: Int
        // - productId: Int
        // - expiryDate: Date
        // - receiveDate: Date
        // - manufactureDate: Date
        // - quantity: Int
        // - salesPrice: Float

        // CURRENT DB
    //     [batchCode]
    //   ,[productId]
    //   ,[expiryDate]
    //   ,[receiveDate]
    //   ,[manufactureDate]
    //   ,[quantity]
    //   ,[batchCost] ONLY DIFF
        public int BatchCode { get; set; }
        public int ProductId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int Quantity { get; set; }
        public int BatchCost { get; set; }
        
        public ProductBatch(int batchCode, int productId, DateTime expiryDate, DateTime receiveDate, DateTime manufactureDate, 
        int quantity, int batchCost)
        {
            BatchCode = batchCode;
            ProductId = productId;
            ExpiryDate = expiryDate;
            ReceiveDate = receiveDate;
            ManufactureDate = manufactureDate;
            Quantity = quantity;
            BatchCost = batchCost;
        }

        public ProductBatch() { }
    }
}