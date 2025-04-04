using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReserve
    {
        Task<List<Item>> getItemsByStatus(ItemStatus status); 
        Task<Product> retrieveProductDetails(int productId);
    }
}