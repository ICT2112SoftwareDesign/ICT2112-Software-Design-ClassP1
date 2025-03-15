using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProduct
    {
        Product GetProductDetails(int productId);
    }
}