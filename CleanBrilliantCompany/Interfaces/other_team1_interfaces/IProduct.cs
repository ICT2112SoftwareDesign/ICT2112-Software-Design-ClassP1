using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProduct
    {
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
    }
}