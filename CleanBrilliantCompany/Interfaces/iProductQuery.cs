using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductQuery
    {
        Product getProductDetails(int productId);
        List<Product> getAllProducts();
        void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState);
    }
}