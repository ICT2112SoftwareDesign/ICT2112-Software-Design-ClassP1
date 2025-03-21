namespace CleanBrilliantCompany.Models
{
    public interface IOrderDatabase
    {
        int insertOrder(OrderRDM order); // Inserts a new order and returns the order ID
        OrderRDM getOrderById(int orderId); // Retrieves an order by its ID
        List<OrderRDM> getOrdersByCustomerId(int customerId); // Retrieves all orders by a customer
        bool updateOrder(OrderRDM order); // Updates an order in the database

    }
}