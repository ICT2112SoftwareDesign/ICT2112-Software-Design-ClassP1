using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iItem
    {
        Task<Item> getItemById(int itemId); 
    }
}