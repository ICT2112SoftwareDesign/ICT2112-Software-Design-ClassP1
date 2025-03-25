using Microsoft.Data.SqlClient;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class OrderMapper : IOrderDatabase
    {
        private readonly string _connectionString;
        private readonly IOrderQueryObserver _observer;

        public OrderMapper(string connectionString, IOrderQueryObserver observer)
        {
            _connectionString = connectionString;
            _observer = observer;
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
                command.Parameters.AddWithValue("@CustomerID", order.RetrieveCustomerID());
                command.Parameters.AddWithValue("@OrderAddress", order.RetrieveOrderAddress());

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string orderProductsJson = JsonSerializer.Serialize(order.RetrieveOrderProducts(), options);
                command.Parameters.AddWithValue("@OrderProducts", orderProductsJson);

                command.Parameters.AddWithValue("@OrderShipping", order.RetrieveOrderShipping());

                string orderItemsJson = JsonSerializer.Serialize(order.RetrieveOrderItems(), options);
                command.Parameters.AddWithValue("@OrderItems", orderItemsJson);

                command.Parameters.AddWithValue("@OrderDate", order.RetrieveOrderDate());
                command.Parameters.AddWithValue("@Status", order.RetrieveStatus());
                command.Parameters.AddWithValue("@OrderTotal", order.RetrieveOrderTotal());

                connection.Open();
                var result = command.ExecuteScalar();
                int orderId = result != null ? Convert.ToInt32(result) : 0;

                // Notify the observer about the new order
                _observer.onOrderCreated(orderId, order.RetrieveCustomerID(), order.RetrieveOrderTotal());

                return orderId;
            }
        }

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
                        var orderProducts = reader["orderProducts"] != DBNull.Value
                            ? JsonSerializer.Deserialize<Dictionary<int, int>>(reader["orderProducts"].ToString())
                            : new Dictionary<int, int>();

                        var orderItems = new List<int>();
                        try
                        {
                            if (reader["orderItems"] != DBNull.Value && !string.IsNullOrEmpty(reader["orderItems"].ToString()))
                            {
                                var items = reader["orderItems"].ToString();
                                if (items.StartsWith("[") && items.EndsWith("]"))
                                {
                                    orderItems = JsonSerializer.Deserialize<List<int>>(items);
                                }
                                else
                                {
                                    orderItems = items.Split(',').Select(int.Parse).ToList();
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing OrderItems for OrderID {reader["orderID"]}: {ex.Message}");
                        }

                        orders.Add(new OrderRDM(
                            orderID: Convert.ToInt32(reader["orderID"]),
                            customerID: Convert.ToInt32(reader["customerID"]),
                            orderAddress: reader["orderAddress"].ToString(),
                            orderProducts: orderProducts,
                            orderShipping: reader["orderShipping"].ToString(),
                            orderItems: orderItems,
                            orderDate: Convert.ToDateTime(reader["orderDate"]),
                            status: reader["Status"].ToString(),
                            orderTotal: Convert.ToDecimal(reader["orderTotal"])
                        ));
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
                        var orderProducts = reader["orderProducts"] != DBNull.Value
                            ? JsonSerializer.Deserialize<Dictionary<int, int>>(reader["orderProducts"].ToString())
                            : new Dictionary<int, int>();

                        var orderItems = new List<int>();
                        try
                        {
                            if (reader["orderItems"] != DBNull.Value && !string.IsNullOrEmpty(reader["orderItems"].ToString()))
                            {
                                var items = reader["orderItems"].ToString();
                                if (items.StartsWith("[") && items.EndsWith("]"))
                                {
                                    orderItems = JsonSerializer.Deserialize<List<int>>(items);
                                }
                                else
                                {
                                    orderItems = items.Split(',').Select(int.Parse).ToList();
                                }
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing OrderItems for OrderID {orderId}: {ex.Message}");
                        }

                        return new OrderRDM(
                            orderID: Convert.ToInt32(reader["orderID"]),
                            customerID: Convert.ToInt32(reader["customerID"]),
                            orderAddress: reader["orderAddress"].ToString(),
                            orderProducts: orderProducts,
                            orderShipping: reader["orderShipping"].ToString(),
                            orderItems: orderItems,
                            orderDate: Convert.ToDateTime(reader["orderDate"]),
                            status: reader["Status"].ToString(),
                            orderTotal: Convert.ToDecimal(reader["orderTotal"])
                        );
                    }
                }
            }

            return null;
        }

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
                command.Parameters.AddWithValue("@OrderID", order.RetrieveOrderID());
                command.Parameters.AddWithValue("@CustomerID", order.RetrieveCustomerID());
                command.Parameters.AddWithValue("@OrderAddress", order.RetrieveOrderAddress());

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                string orderProductsJson = JsonSerializer.Serialize(order.RetrieveOrderProducts(), options);
                command.Parameters.AddWithValue("@OrderProducts", orderProductsJson);

                command.Parameters.AddWithValue("@OrderShipping", order.RetrieveOrderShipping());

                string orderItemsJson = JsonSerializer.Serialize(order.RetrieveOrderItems(), options);
                command.Parameters.AddWithValue("@OrderItems", orderItemsJson);

                command.Parameters.AddWithValue("@OrderDate", order.RetrieveOrderDate());
                command.Parameters.AddWithValue("@Status", order.RetrieveStatus());
                command.Parameters.AddWithValue("@OrderTotal", order.RetrieveOrderTotal());

                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Notify the observer about the order update
                    _observer.onOrderUpdated(order.RetrieveOrderID(), order.RetrieveCustomerID(), order.RetrieveStatus());
                }

                return rowsAffected > 0;
            }
        }

        public bool cancelOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "UPDATE CustOrder SET Status = 'Cancelled' WHERE orderID = @OrderID";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                var rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Notify the observer about the order cancellation
                    _observer.onOrderCancelled(orderId, 0); // Pass 0 for customerId if it's not available
                }

                return rowsAffected > 0;
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
                        var orderProducts = reader["orderProducts"] != DBNull.Value
                            ? JsonSerializer.Deserialize<Dictionary<int, int>>(reader["orderProducts"].ToString())
                            : new Dictionary<int, int>();

                        var orderItems = reader["orderItems"] != DBNull.Value
                            ? JsonSerializer.Deserialize<List<int>>(reader["orderItems"].ToString())
                            : new List<int>();

                        orders.Add(new OrderRDM(
                            orderID: Convert.ToInt32(reader["orderID"]),
                            customerID: Convert.ToInt32(reader["customerID"]),
                            orderAddress: reader["orderAddress"].ToString(),
                            orderProducts: orderProducts,
                            orderShipping: reader["orderShipping"].ToString(),
                            orderItems: orderItems,
                            orderDate: Convert.ToDateTime(reader["orderDate"]),
                            status: reader["Status"].ToString(),
                            orderTotal: Convert.ToDecimal(reader["orderTotal"])
                        ));
                    }
                }
            }
            return orders;
        }
        public List<OrderRDM> getOrdersByMonth(int monthNumber)
        {
            var orders = new List<OrderRDM>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
                    SELECT * FROM CustOrder
                    WHERE MONTH(orderDate) = @MonthNumber
                    ORDER BY orderDate DESC";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MonthNumber", monthNumber);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var orderProducts = reader["orderProducts"] != DBNull.Value
                            ? JsonSerializer.Deserialize<Dictionary<int, int>>(reader["orderProducts"].ToString())
                            : new Dictionary<int, int>();

                        var orderItems = reader["orderItems"] != DBNull.Value
                            ? JsonSerializer.Deserialize<List<int>>(reader["orderItems"].ToString())
                            : new List<int>();

                        orders.Add(new OrderRDM(
                            orderID: Convert.ToInt32(reader["orderID"]),
                            customerID: Convert.ToInt32(reader["customerID"]),
                            orderAddress: reader["orderAddress"].ToString(),
                            orderProducts: orderProducts,
                            orderShipping: reader["orderShipping"].ToString(),
                            orderItems: orderItems,
                            orderDate: Convert.ToDateTime(reader["orderDate"]),
                            status: reader["Status"].ToString(),
                            orderTotal: Convert.ToDecimal(reader["orderTotal"])
                        ));
                    }
                }
            }
            return orders;
        }
    }
}