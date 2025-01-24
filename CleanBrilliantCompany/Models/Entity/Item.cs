namespace CleanBrilliantCompany.Models.Entity
{
    public class Item
    {
        /*
        - itemId: Int
        - productId: Int
        - expiryDate: Date
        - receiveDate: Date
        - manufactureDate: Date
        - salePrice: Float
        - batchCode: Int
        - warehouseId: Int
        - status: Status
        - reservationId: Int
        - orderId: Int
        - transferId: Int
        - returnId: Int 
        */

        public int ItemId { get; set; }
        public int ProductId { get; set; }
        public DateOnly ExpiryDate { get; set; }
        public DateOnly ReceiveDate { get; set; }
        public DateOnly ManufactureDate { get; set; }
        public float SalePrice { get; set; }
        public int BatchCode { get; set; }
        public int WarehouseId { get; set; }
        public Status Status { get; set; }
        public int ReservationId { get; set; }
        public int OrderId { get; set; }
        public int TransferId { get; set; }
        public int ReturnId { get; set; }


        public Item(int itemId, int productId, DateOnly expiryDate, DateOnly receiveDate, DateOnly manufactureDate,
                    float salePrice, int batchCode, int warehouseId, Status status, int reservationId, int orderId,
                    int transferId, int returnId)
        {
            ItemId = itemId;
            ProductId = productId;
            ExpiryDate = expiryDate;
            ReceiveDate = receiveDate;
            ManufactureDate = manufactureDate;
            SalePrice = salePrice;
            BatchCode = batchCode;
            WarehouseId = warehouseId;
            Status = status;
            ReservationId = reservationId;
            OrderId = orderId;
            TransferId = transferId;
            ReturnId = returnId;
        }

        public Item() { }
    }

    public enum Status
    {
        Available,
        Reserved,
        Sold,
        Refunded
    }

}
