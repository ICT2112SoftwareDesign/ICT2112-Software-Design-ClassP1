using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderFufilment
    {
        Task<List<Item>> adjustInventory(int orderId, Dictionary<int, int> orderProducts); 
        void processCancelledOrder(int orderId);
    }
}