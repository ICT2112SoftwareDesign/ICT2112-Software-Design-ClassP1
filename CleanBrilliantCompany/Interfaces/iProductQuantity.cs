using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductQuantity
    {
        // Product
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
    }
}