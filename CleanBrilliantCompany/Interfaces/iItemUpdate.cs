using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemUpdate
    {
        Task<bool> updateItem(int itemId, float salePrice); 
    }
}