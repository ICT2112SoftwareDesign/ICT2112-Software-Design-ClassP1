using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iItemUpdate
    {
        Task<bool> updateItem(int itemId, float salePrice); 
    }
}