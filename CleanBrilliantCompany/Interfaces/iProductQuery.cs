using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductQuery
    {
        Product getProductDetails(int productId);
        List<Product> GetAllProducts();
    }
}