using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iTransactionQuery
    {
        List<Transaction> getAllTransactions();

    //     Product getProductDetails(int productId);
    //     List<Product> GetAllProducts();
    //     void createProduct(string productName, string category, float costPrice, 
    //     int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState);
    }
}