using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductQuery
    {
        // Product
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
        void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState);
    }
}