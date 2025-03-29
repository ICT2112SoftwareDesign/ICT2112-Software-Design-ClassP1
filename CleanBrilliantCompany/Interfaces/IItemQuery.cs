using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemQuery
    {
        Task<List<Item>> getAllItems(); 

        Task<Item> getItemById(int itemId);

    }
}