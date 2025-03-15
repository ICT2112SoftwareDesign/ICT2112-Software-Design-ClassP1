namespace CleanBrilliantCompany.Models.Entity
{


    public class Transfer
    {
        private int transferId;
        private int productId;
        private string status;
        private int quantity;
        private int sourceWarehouse;
        private int destinationWarehouse;

        private int getTransferId()
        {
            return transferId;
        }

        private void setTransferId(int transferId)
        {
            this.transferId = transferId;
        }

        private int getProductId()
        {
            return productId;
        }

        private void setProductId(int productId)
        {
            this.productId = productId;
        }

        private string getStatus()
        {
            return status;
        }

        private void setStatus(string status)
        {
            this.status = status;
        }

        private int getQuantity()
        {
            return quantity;
        }

        private void setQuantity(int quantity)
        {
            this.quantity = quantity;
        }

        private int getSourceWarehouse()
        {
            return sourceWarehouse;
        }

        private void setSourceWarehouse(int sourceWarehouse)
        {
            this.sourceWarehouse = sourceWarehouse;
        }

        private int getDestinationWarehouse()
        {
            return destinationWarehouse;
        }

        private void setDestinationWarehouse(int destinationWarehouse)
        {
            this.destinationWarehouse = destinationWarehouse;
        }

        public void createTransfer(int productId, string status, int quantity, int sourceWarehouse, int destinationWarehouse)
        {
            setProductId(productId);
            setStatus(status);
            setQuantity(quantity);
            setSourceWarehouse(sourceWarehouse);
            setDestinationWarehouse(destinationWarehouse);
            setStatus("Pending");

        }
    }

}