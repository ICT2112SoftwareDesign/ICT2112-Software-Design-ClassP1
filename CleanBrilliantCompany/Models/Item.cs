namespace CleanBrilliantCompany.Models
{

    public enum ItemStatus
    {
        Available,
        Reserved,
        Sold,
        Refunded,
        Returned,
        ToReturn,
        Transferred
    }

    public class Item
    {
        // Private fields
        private int ItemId;
        private int ProductId;
        private float SalePrice;
        private int BatchCode;
        private int WarehouseId;
        private ItemStatus ItemStatus;
        private int? ReservationId;
        private int? OrderId;
        private int? TransferId;
        private int? ReturnId;
 


        // Constructor to initialize the private fields
        public Item(int itemId, int productId, float salePrice, int batchCode, int warehouseId,
                     ItemStatus itemStatus, int? reservationId, int? orderId, int? transferId, int? returnId)
        {
            ItemId = itemId;
            ProductId = productId;
            SalePrice = salePrice;
            BatchCode = batchCode;
            WarehouseId = warehouseId;
            ItemStatus = itemStatus;
            ReservationId = reservationId;
            OrderId = orderId;
            TransferId = transferId;
            ReturnId = returnId;
        }

        // Private getters 
        public int getItemId() => ItemId;
        public int getProductId() => ProductId;
        private float getSalePrice() => SalePrice;
        private int getBatchCode() => BatchCode;
        private int getWarehouseId() => WarehouseId;
        private ItemStatus getItemStatus() => ItemStatus;
        private int? getReservationId() => ReservationId;
        private int? getOrderId() => OrderId;
        private int? getTransferId() => TransferId;
        private int? getReturnId() => ReturnId;

        // Private setters 
        private void setItemId(int itemId) => ItemId = itemId;
        private void setProductId(int productId) => ProductId = productId;
        private void setSalePrice(float salePrice) => SalePrice = salePrice;
        private void setBatchCode(int batchCode) => BatchCode = batchCode;
        private void setWarehouseId(int warehouseId) => WarehouseId = warehouseId;
        private void setItemStatus(ItemStatus itemStatus) => ItemStatus = itemStatus;
        private void setReservationId(int? reservationId) => ReservationId = reservationId;
        private void setOrderId(int? orderId) => OrderId = orderId;
        private void setTransferId(int? transferId) => TransferId = transferId;
        private void setReturnId(int? returnId) => ReturnId = returnId;

        public Item() { }
    }
}
