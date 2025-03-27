using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReturnForm
    {
        Task<List<Item>> getToReturnItems(); 
        Task<Product> retrieveProductDetails(int productId);
    }
}