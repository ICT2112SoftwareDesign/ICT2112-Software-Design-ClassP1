using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemUpdate
    {
        Task<bool> updateItemStatus(int itemId, int? reservationId, int? orderId, int? transferId, int? returnId, ItemStatus status); 
        void updateProductQuantity(int productId, int quantity, string arithmeticOperations);
    }
}