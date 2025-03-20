using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iItemQuery
    {
        Task<List<Item>> getAllItems(); 

        Task<Item> getItem(int itemId);

    }
}