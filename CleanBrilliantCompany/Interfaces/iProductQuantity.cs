using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductQuantity
    {
        public void updateQuantity(int productId, int quantity, string arithmeticOperations);
    }
}