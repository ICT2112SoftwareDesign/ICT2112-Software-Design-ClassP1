using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItem
    {
        Task<Item> getItemById(int itemId); 
        
    }
}