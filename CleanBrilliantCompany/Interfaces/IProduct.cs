using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProduct
    {
        // Product
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
    }
}