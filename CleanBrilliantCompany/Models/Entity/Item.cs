using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models.Entity
{
    public enum ItemStatus
    {
        Available,
        Reserved,
        Sold,
        Refunded
    }

    public class Item : ISubject
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

        private List<IObserver> _observers = new List<IObserver>();

        // public ItemStatus StatusChange
        // {
        //     get => ItemStatus;
            
        // // }
        // public string StatusChange
        // {
        //     get => ItemStatus.ToString(); //convert the enum into string
        //     set 
        //     {
        //         if (ItemStatus.ToString() != value)
        //         {
        //             // ItemStatus = value;
        //             Notify();
        //         }
        //     }
        // }

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

        public Dictionary<string, object> retrieveItemInfo()
        {
            return new Dictionary<string, object>
            {
                { "ItemId", ItemId },
                { "ProductId", ProductId },
                { "SalePrice", SalePrice },
                { "BatchCode", BatchCode },
                { "WarehouseId", WarehouseId },
                { "ItemStatus", ItemStatus },
                { "ReservationId", ReservationId },
                { "OrderId", OrderId },
                { "TransferId", TransferId },
                { "ReturnId", ReturnId }
            };
        }

        // Private getters 
        private int getItemId() => ItemId;
        private int getProductId() => ProductId;
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

        public void Attach(IObserver observer)
        {
            Console.WriteLine("Called Attach Observer method");
            _observers.Add(observer);
            Console.WriteLine("Observer has been added Line 130");

        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);

        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this); //  Pass the Item object instead of a string
            }        
        }

        public void UpdateStatus(ItemStatus newStatus)
    {
        Console.WriteLine("Entered UpdateStatus");
        Console.WriteLine($"Current ItemStatus: {ItemStatus}");
        Console.WriteLine($"New ItemStatus: {newStatus}");
 
        ItemStatus = newStatus;

        Console.WriteLine($"Item {ItemId} status updated to {ItemStatus} in Item.cs file");
        Console.WriteLine($"Calling Notify() in Item.cs file");

        Notify();

    }

        public Item() { } // dk if need anot 
    }
}
