using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductQuery
    {
        Task<Product> getProductDetails(int productId);
        Task<(string status, List<Product> products)> getAllProducts();
        Task<string> createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState);
    }
}