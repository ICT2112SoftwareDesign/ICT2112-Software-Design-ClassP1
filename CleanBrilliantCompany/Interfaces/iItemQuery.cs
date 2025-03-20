using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemQuery
    {
        Task<List<Item>> getAllItems(); 

        Task<Item> getItemById(int itemId);

    }
}