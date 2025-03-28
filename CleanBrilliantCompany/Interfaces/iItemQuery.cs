using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemQuery
    {
        Task<List<Item>> getAllItems(int pageNumber, int pageSize); 

        Task<Item> getItemById(int itemId);

    }
}