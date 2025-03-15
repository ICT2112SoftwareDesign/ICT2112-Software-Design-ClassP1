namespace CleanBrilliantCompany.Models.Entity
{
    public enum ItemStatus
    {
        Available,
        Reserved,
        Sold,
        Refunded
    }

    public class Item
    {
        // Private fields
        private int itemId;
        private int productId;
        private float salePrice;
        private int batchCode;
        private int warehouseId;
        private ItemStatus itemStatus;
        private int? reservationId;
        private int? orderId;
        private int? transferId;
        private int? returnId;

        // Constructor to initialize the private fields
        public Item(int itemId, int productId, float salePrice, int batchCode, int warehouseId,
                     ItemStatus itemStatus, int? reservationId, int? orderId, int? transferId, int? returnId)
        {
            this.itemId = itemId;
            this.productId = productId;
            this.salePrice = salePrice;
            this.batchCode = batchCode;
            this.warehouseId = warehouseId;
            this.itemStatus = itemStatus;
            this.reservationId = reservationId;
            this.orderId = orderId;
            this.transferId = transferId;
            this.returnId = returnId;
        }

        // Public method to create a new item
        public static Item CreateItem(int itemId, int productId, float salePrice, int batchCode, int warehouseId,
                                      ItemStatus itemStatus, int? reservationId, int? orderId, int? transferId, int? returnId)
        {
            return new Item(itemId, productId, salePrice, batchCode, warehouseId, itemStatus, reservationId, orderId, transferId, returnId);
        }

        // Public method to update item details
        public void UpdateItemDetails(int productId, float salePrice, int batchCode, int warehouseId, ItemStatus itemStatus,
                                      int? reservationId, int? orderId, int? transferId, int? returnId)
        {
            this.productId = productId;
            this.salePrice = salePrice;
            this.batchCode = batchCode;
            this.warehouseId = warehouseId;
            this.itemStatus = itemStatus;
            this.reservationId = reservationId;
            this.orderId = orderId;
            this.transferId = transferId;
            this.returnId = returnId;
        }

        public Dictionary<string, object> retrieveItemInfo()
        {
            return new Dictionary<string, object>
            {
                { "ItemId", getItemId() },
                { "ProductId", getProductId() },
                { "SalePrice", getSalePrice() },
                { "BatchCode", getBatchCode() },
                { "WarehouseId", getWarehouseId() },
                { "ItemStatus", getItemStatus() },
                { "ReservationId", getReservationId() },
                { "OrderId", getOrderId() },
                { "TransferId", getTransferId() },
                { "ReturnId", getReturnId() }
            };
        }


        public int retrieveItemId() => getItemId();
        public int retrieveProductId() => getProductId();

        // Private getters 
        private int getItemId() => itemId;
        private int getProductId() => productId;
        private float getSalePrice() => salePrice;
        private int getBatchCode() => batchCode;
        private int getWarehouseId() => warehouseId;
        private ItemStatus getItemStatus() => itemStatus;
        private int? getReservationId() => reservationId;
        private int? getOrderId() => orderId;
        private int? getTransferId() => transferId;
        private int? getReturnId() => returnId;

        // Private setters 
        private void setItemId(int itemId) => this.itemId = itemId;
        private void setProductId(int productId) => this.productId = productId;
        private void setSalePrice(float salePrice) => this.salePrice = salePrice;
        private void setBatchCode(int batchCode) => this.batchCode = batchCode;
        private void setWarehouseId(int warehouseId) => this.warehouseId = warehouseId;
        private void setItemStatus(ItemStatus itemStatus) => this.itemStatus = itemStatus;
        private void setReservationId(int? reservationId) => this.reservationId = reservationId;
        private void setOrderId(int? orderId) => this.orderId = orderId;
        private void setTransferId(int? transferId) => this.transferId = transferId;
        private void setReturnId(int? returnId) => this.returnId = returnId;

        public Item() { } // dk if need anot 
    }
}
