using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductQuantity
    {
        Product getProductDetails(int productId);
        public void updateQuantity(int productId, int quantity, string arithmeticOperations);
    }
}