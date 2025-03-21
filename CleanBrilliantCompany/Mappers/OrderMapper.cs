using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Models
{
    public class OrderMapper : IOrderDatabase
    {
        private readonly string _connectionString;

        public OrderMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int InsertOrder(OrderRDM order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    INSERT INTO CustOrder (customerID, orderAddress, orderProducts, orderShipping, orderItems, orderDate, Status, orderTotal)
                    VALUES (@CustomerID, @OrderAddress, @OrderProducts, @OrderShipping, @OrderItems, @OrderDate, @Status, @OrderTotal);
                    SELECT SCOPE_IDENTITY();";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                command.Parameters.AddWithValue("@OrderAddress", order.OrderAddress);
                // Serialize the dictionary to JSON
                string orderProductsJson = System.Text.Json.JsonSerializer.Serialize(order.OrderProducts);
                command.Parameters.AddWithValue("@OrderProducts", orderProductsJson);
                command.Parameters.AddWithValue("@OrderShipping", order.OrderShipping);
                command.Parameters.AddWithValue("@OrderItems", order.OrderItems);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@Status", order.Status);
                command.Parameters.AddWithValue("@OrderTotal", order.OrderTotal);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != null ? Convert.ToInt32(result) : 0;
            }
        }

       public OrderRDM GetOrderById(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM CustOrder WHERE orderID = @OrderID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderRDM
                        {
                            OrderID = Convert.ToInt32(reader["orderID"]),
                            CustomerID = Convert.ToInt32(reader["customerID"]),
                            OrderAddress = reader["orderAddress"].ToString(),
                            OrderProducts = reader["orderProducts"] != DBNull.Value 
                                ? System.Text.Json.JsonSerializer.Deserialize<Dictionary<int, int>>(reader["orderProducts"].ToString()) 
                                : new Dictionary<int, int>(),
                            OrderShipping = reader["orderShipping"].ToString(),
                            OrderItems = Convert.ToInt32(reader["orderItems"]),
                            OrderDate = Convert.ToDateTime(reader["orderDate"]),
                            Status = reader["Status"].ToString(),
                            OrderTotal = Convert.ToDecimal(reader["orderTotal"])
                        };
                    }
                }
            }
            return null;
        }
    }
}