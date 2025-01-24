using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProduct
    {
        Product GetProductDetails(int productId);

        List<Product> GetAllProducts();

    }

}
