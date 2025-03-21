namespace CleanBrilliantCompany.Models
{
    public interface IOrderDatabase
    {
        int InsertOrder(OrderRDM order); // Inserts a new order and returns the order ID
        OrderRDM GetOrderById(int orderId); // Retrieves an order by its ID
    }
}