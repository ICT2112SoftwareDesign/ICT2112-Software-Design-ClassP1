using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReserve
    {
        List<Item> GetItemsByStatus(Status status, IConfiguration configuration);

        Product RetrieveProductDetails(int productId, IConfiguration configuration);

    }

    public interface IItem
    {
        Item GetItemById(int itemId, IConfiguration configuration);
    }

    public interface IItemUpdate
    {
        bool UpdateItemById(int itemId, int productId, DateOnly expiryDate, DateOnly receiveDate, DateOnly manufactureDate,
                    float salePrice, int batchCode, int warehouseId, Status status, int reservationId, int orderId,
                    int transferId, int returnId, IConfiguration configuration);
    }
}
