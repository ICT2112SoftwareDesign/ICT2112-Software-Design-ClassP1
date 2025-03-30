using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemDetails
    {
        Task<List<Item>> getItems();
        Task<Item> getItemById(int itemId); 
    }
}