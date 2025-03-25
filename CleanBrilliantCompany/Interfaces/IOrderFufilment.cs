using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderFufilment
    {
        // still have to reflect on product quantity
        List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts); 
        void processCancelledOrder(int orderId);
    }
}