using Microsoft.Data.SqlClient;
using System.Text.Json;


namespace CleanBrilliantCompany.Models
{
    public class OrderMapper : IOrderDatabase
    {
        private readonly string _connectionString;

        public OrderMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int insertOrder(OrderRDM order)
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
                // Serialize the dictionary to JSON with camel case
                var options = new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                };
                string orderProductsJson = System.Text.Json.JsonSerializer.Serialize(order.OrderProducts, options);
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
        public List<OrderRDM> getAllOrders()
        {
            var orders = new List<OrderRDM>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM CustOrder ORDER BY orderDate DESC";
                var command = new SqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderRDM
                        {
                            OrderID = Convert.ToInt32(reader["orderID"]),
                            CustomerID = Convert.ToInt32(reader["customerID"]),
                            OrderAddress = reader["orderAddress"].ToString(),
                            OrderProducts = reader["orderProducts"] != DBNull.Value
                                ? JsonSerializer.Deserialize<Dictionary<int, int>>(reader["orderProducts"].ToString())
                                : new Dictionary<int, int>(),
                            OrderShipping = reader["orderShipping"].ToString(),
                            OrderItems = Convert.ToInt32(reader["orderItems"]),
                            OrderDate = Convert.ToDateTime(reader["orderDate"]),
                            Status = reader["Status"].ToString(),
                            OrderTotal = Convert.ToDecimal(reader["orderTotal"])
                        });
                    }
                }
            }
            return orders;
        }

        public OrderRDM getOrderById(int orderId)
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

        // Get all orders for a specific customer
        public List<OrderRDM> getOrdersByCustomerId(int customerId)
        {
            var orders = new List<OrderRDM>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM CustOrder WHERE customerID = @CustomerID ORDER BY orderDate DESC";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CustomerID", customerId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderRDM
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
                        });
                    }
                }
            }
            return orders;
        }

        // Update an existing order in the database
        public bool updateOrder(OrderRDM order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    UPDATE CustOrder
                    SET customerID = @CustomerID,
                        orderAddress = @OrderAddress,
                        orderProducts = @OrderProducts,
                        orderShipping = @OrderShipping,
                        orderItems = @OrderItems,
                        orderDate = @OrderDate,
                        Status = @Status,
                        orderTotal = @OrderTotal
                    WHERE orderID = @OrderID";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", order.OrderID);
                command.Parameters.AddWithValue("@CustomerID", order.CustomerID);
                command.Parameters.AddWithValue("@OrderAddress", order.OrderAddress);

                // Serialize the dictionary to JSON with camel case
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string orderProductsJson = JsonSerializer.Serialize(order.OrderProducts, options);
                command.Parameters.AddWithValue("@OrderProducts", orderProductsJson);

                command.Parameters.AddWithValue("@OrderShipping", order.OrderShipping);
                command.Parameters.AddWithValue("@OrderItems", order.OrderItems);
                command.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                command.Parameters.AddWithValue("@Status", order.Status);
                command.Parameters.AddWithValue("@OrderTotal", order.OrderTotal);

                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0; // Return true if the update was successful
            }
        }
    }
}
