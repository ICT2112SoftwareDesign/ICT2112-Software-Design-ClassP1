namespace CleanBrilliantCompany.Models
{
    public interface IOrderDatabase
    {
        int insertOrder(OrderRDM order); // Inserts a new order and returns the order ID
        OrderRDM getOrderById(int orderId); // Retrieves an order by its ID
        List<OrderRDM> getOrdersByCustomerId(int customerId); // Retrieves all orders by a customer
        bool updateOrder(OrderRDM order); // Updates an order in the database
        bool updateOrderStatus(int orderId, string status);
        bool cancelOrder(int orderId); // Cancels an order
        List<OrderRDM> getAllOrders(); // Retrieves all orders
        List<OrderRDM> getOrdersByMonth(int monthNumber); // Retrieves all orders for a specific month
        public decimal GetTotalOrderValue();
        public int GetTotalOrderCount();

        

    }
}