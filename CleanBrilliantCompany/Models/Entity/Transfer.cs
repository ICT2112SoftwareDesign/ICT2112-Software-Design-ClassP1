namespace CleanBrilliantCompany.Models.Entity
{


    public enum TransferStatus
    {
        Pending,
        Transit,
        Completed
    }

    public class Transfer
    {
        private int TransferId;
        private int ProductId;
        private TransferStatus Status;
        private int Quantity;
        private int SourceWarehouse;
        private int DestinationWarehouse;
        private int StaffId;

        // Private fields for additional information
        private string ProductName;
        private string SourceWarehouseName;
        private string DestinationWarehouseName;

        public Transfer(int transferId, int productId, TransferStatus status, int quantity, int sourceWarehouse, int destinationWarehouse, int staffId)
        {
            TransferId = transferId;
            ProductId = productId;
            Status = status;
            Quantity = quantity;
            SourceWarehouse = sourceWarehouse;
            DestinationWarehouse = destinationWarehouse;
            StaffId = staffId;
        }

        public Transfer(int transferId, int productId, TransferStatus status, int quantity, int sourceWarehouse, int destinationWarehouse, int staffId, string productName, string sourceWarehouseName, string destinationWarehouseName)
        {
            TransferId = transferId;
            ProductId = productId;
            Status = status;
            Quantity = quantity;
            SourceWarehouse = sourceWarehouse;
            DestinationWarehouse = destinationWarehouse;
            StaffId = staffId;
            ProductName = productName;
            SourceWarehouseName = sourceWarehouseName;
            DestinationWarehouseName = destinationWarehouseName;
        }
        
        public Dictionary<string, object> retrieveTransferInfo()
        {
            return new Dictionary<string, object>
            {
                { "TransferId", TransferId },
                { "ProductId", ProductId },
                { "Quantity", Quantity },
                { "SourceWarehouse", SourceWarehouse },
                { "DestinationWarehouse", DestinationWarehouse },
                { "StaffId", StaffId },
                { "Status", Status },
                { "ProductName", ProductName },
                { "SourceWarehouseName", SourceWarehouseName },
                { "DestinationWarehouseName", DestinationWarehouseName }            
            };
        }

        //Getter Method
        private int getTransferId() => TransferId;
        private int getProductId() => ProductId;
        private TransferStatus getStatus() => Status;
        private int getQuantity() => Quantity;
        private int getSourceWarehouse() => SourceWarehouse;
        private int getDestinationWarehouse() => DestinationWarehouse;
        private int getStaffId() => StaffId;


        //Setter Method
        private void setTransferId(int transferId) => this.TransferId = transferId;
        private void setProductId(int productId) => this.ProductId = productId;
        private void setStatus(TransferStatus status) => this.Status = status;
        private void setQuantity(int quantity) => this.Quantity = quantity;
        private void setSourceWarehouse(int sourceWarehouse) => this.SourceWarehouse = sourceWarehouse;
        private void setDestinationWarehouse(int destinationWarehouse) => this.DestinationWarehouse = destinationWarehouse;
        private void setStaffId(int staffId) => this.StaffId = staffId;


        public static Transfer createTransfer(int transferId, int productId, TransferStatus status, int quantity, int sourceWarehouse, int destinationWarehouse, int staffId)
        {
            return new Transfer(transferId, productId, status, quantity, sourceWarehouse, destinationWarehouse, staffId);
        }

        // public void updateTransferStatus(TransferStatus status)
        // {
        //     this.Status = status;
        // }

        // Public method to update item details
        public void UpdateTransfer(int transferId, int productId, TransferStatus status, int quantity, int sourceWarehouse, int destinationWarehouse, int staffId)
        {
            TransferId = transferId;
            ProductId = productId;
            Status = status;
            Quantity = quantity;
            SourceWarehouse = sourceWarehouse;
            DestinationWarehouse = destinationWarehouse;
            StaffId = staffId;
        }
    
    
    }

}