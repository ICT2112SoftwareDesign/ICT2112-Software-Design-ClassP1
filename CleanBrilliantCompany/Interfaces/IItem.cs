using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItem
    {
        Task<Item> getItemById(int itemId); 
    }
}