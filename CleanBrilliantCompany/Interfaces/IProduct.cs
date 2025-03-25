using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProduct
    {
        // Product
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
    }
}