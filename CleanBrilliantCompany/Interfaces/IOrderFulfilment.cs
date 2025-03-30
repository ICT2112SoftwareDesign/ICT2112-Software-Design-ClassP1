using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderFulfilment
    {
        // still have to reflect on product quantity
        List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts); 
        void processCancelledOrder(int orderId);
    }
}