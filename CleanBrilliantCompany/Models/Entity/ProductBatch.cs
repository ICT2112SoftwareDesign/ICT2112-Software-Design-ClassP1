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


        private int BatchCode;
        private int ProductId;
        private DateTime ExpiryDate; // Change to date
        private DateTime ReceiveDate; // Change to date
        private DateTime ManufactureDate; // Change to date for me
        private int Quantity;
        private int BatchCost; // Change name to SalesPrice
        
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

        public Dictionary<string, object> retrieveProductBatchInfo()
        {
            return new Dictionary<string, object>
            {
                { "BatchCode", BatchCode },
                { "ProductId", ProductId },
                { "ExpiryDate", ExpiryDate },
                { "ReceiveDate", ReceiveDate },
                { "ManufactureDate", ManufactureDate },
                { "Quantity", Quantity },
                { "BatchCost", BatchCost },
            };
        }

        // Getters
        private int GetBatchCode() => BatchCode;
        private int GetProductId() => ProductId;
        private DateTime GetExpiryDate() => ExpiryDate;
        private DateTime GetReceiveDate() => ReceiveDate;
        private DateTime GetManufactureDate() => ManufactureDate;
        private int GetQuantity() => Quantity;
        private int GetBatchCost() => BatchCost;

        // Setters
        private void SetBatchCode(int value) => BatchCode = value;
        private void SetProductId(int value) => ProductId = value;
        private void SetExpiryDate(DateTime value) => ExpiryDate = value;
        private void SetReceiveDate(DateTime value) => ReceiveDate = value;
        private void SetManufactureDate(DateTime value) => ManufactureDate = value;
        private void SetQuantity(int value) => Quantity = value;
        private void SetBatchCost(int value) => BatchCost = value;


        public ProductBatch() { }
    }
}