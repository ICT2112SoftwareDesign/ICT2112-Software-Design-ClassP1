using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemCreation
    {
        Task<bool> createItem(int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status); 
    }
}