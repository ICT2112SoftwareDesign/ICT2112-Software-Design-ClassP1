using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemQuery
    {
        Task<List<Item>> getAllItems(int pageNumber, int pageSize); 
        int getItemCount();
        Task<List<Item>> getItems();
        Task<Item> getItemById(int itemId);
        Task<List<Item>> getItemByProductName(string productName);
        Task<List<Item>> getItemsByStatus(ItemStatus itemStatus);
        Task<List<Item>> getToReturnItems();
        Task<bool> createItem(int productId, int batchCode, int warehouseId, ItemStatus status);
        Task<bool> updateItem(int itemId, float salePrice);
        Task<bool> updateItemStatus(int itemId, int? reservationId, int? orderId, int? transferId, int? returnId, ItemStatus status);
        Task<Warehouse> getWarehouseDetails(int warehouseId);
        Task<List<Item>> getItemByProductAndWarehouse(int productId, int quantity, int warehouseId);
        Task<int> getProductQuantityByWarehouse(int productId, int warehouseId);
        void returnItemToInventory(List<int> itemId, string refundReason);
        List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts);
        void processCancelledOrder(int orderId);
        Task<List<Item>> getTransferredItems(int transferId);
        Task<List<Product>> getLowStockProductInWarehouse();


    }
}