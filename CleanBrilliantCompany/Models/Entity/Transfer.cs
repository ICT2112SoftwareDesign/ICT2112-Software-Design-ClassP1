namespace CleanBrilliantCompany.Models.Entity
{


    public enum TransferStatus
    {
        Pending,
        Completed,
        Cancelled
    }

    public class Transfer
    {
        private int transferId;
        private int productId;
        private TransferStatus status;
        private int quantity;
        private int sourceWarehouse;
        private int destinationWarehouse;

        //Getter Method
        private int getTransferId() => transferId;
        private int getProductId() => productId;
        private TransferStatus getStatus() => status;
        private int getQuantity() => quantity;
        private int getSourceWarehouse() => sourceWarehouse;
        private int getDestinationWarehouse() => destinationWarehouse;


        //Setter Method
        private void setTransferId(int transferId) => this.transferId = transferId;
        private void setProductId(int productId) => this.productId = productId;
        private void setStatus(TransferStatus status) => this.status = status;
        private void setQuantity(int quantity) => this.quantity = quantity;
        private void setSourceWarehouse(int sourceWarehouse) => this.sourceWarehouse = sourceWarehouse;
        private void setDestinationWarehouse(int destinationWarehouse) => this.destinationWarehouse = destinationWarehouse;
        


        public void createTransfer(int productId, TransferStatus status, int quantity, int sourceWarehouse, int destinationWarehouse)
        {
            setProductId(productId);
            setStatus(status);
            setQuantity(quantity);
            setSourceWarehouse(sourceWarehouse);
            setDestinationWarehouse(destinationWarehouse);

        }
    }

}