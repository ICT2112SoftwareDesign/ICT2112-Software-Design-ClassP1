using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iItemQuery
    {
        List<Item> getAllItems(); 

        Item getItem(int itemId);

    }
}