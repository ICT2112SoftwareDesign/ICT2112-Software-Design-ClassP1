using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Mappers
{
    public class OrderMapper : IOrderDatabase
    {
        private readonly string _connectionString;

        public OrderMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int createOrder(OrderRDM order)
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
                command.Parameters.AddWithValue("@OrderProducts", order.OrderProducts);
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

        public OrderRDM RetrieveOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Orders WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new OrderRDM
                        {
                            // Assuming OrderRDM has public setters for these properties
                           // CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                            //OrderAddress = reader.GetString(reader.GetOrdinal("OrderAddress")),
                            //OrderProducts = JsonConvert.DeserializeObject<Dictionary<int, int>>(reader.GetString(reader.GetOrdinal("OrderProducts"))),
                            //OrderShipping = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.GetString(reader.GetOrdinal("OrderShipping"))),
                            //OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                            //Status = reader.GetString(reader.GetOrdinal("Status")),
                            //OrderTotal = reader.GetDecimal(reader.GetOrdinal("OrderTotal"))
                        };
                    }
                }
            }

            return null;
        }

        public List<OrderRDM> RetrieveOrders(int customerId)
        {
            var orders = new List<OrderRDM>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Orders WHERE CustomerID = @CustomerID", connection);
                command.Parameters.AddWithValue("@CustomerID", customerId);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new OrderRDM
                        {
                            //CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID")),
                            //OrderAddress = reader.GetString(reader.GetOrdinal("OrderAddress")),
                            //OrderProducts = JsonConvert.DeserializeObject<Dictionary<int, int>>(reader.GetString(reader.GetOrdinal("OrderProducts"))),
                            //OrderShipping = JsonConvert.DeserializeObject<Dictionary<string, string>>(reader.GetString(reader.GetOrdinal("OrderShipping"))),
                            //OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                            //Status = reader.GetString(reader.GetOrdinal("Status")),
                            //OrderTotal = reader.GetDecimal(reader.GetOrdinal("OrderTotal"))
                        });
                    }
                }
            }

            return orders;
        }

        public bool UpdateOrder(OrderRDM order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "UPDATE Orders SET OrderAddress = @OrderAddress, OrderProducts = @OrderProducts, OrderShipping = @OrderShipping, OrderDate = @OrderDate, Status = @Status, OrderTotal = @OrderTotal WHERE OrderID = @OrderID",
                    connection
                );

                //command.Parameters.AddWithValue("@OrderID", order.retrieveOrderId());
                //command.Parameters.AddWithValue("@OrderAddress", order.retrieveOrderAddress());
                //command.Parameters.AddWithValue("@OrderProducts", JsonConvert.SerializeObject(order.retrieveOrderProducts()));
                //command.Parameters.AddWithValue("@OrderShipping", JsonConvert.SerializeObject(order.retrieveOrderShipping()));
                //command.Parameters.AddWithValue("@OrderDate", order.retrieveOrderDate());
                //command.Parameters.AddWithValue("@Status", order.retrieveStatus().ToString());
                //command.Parameters.AddWithValue("@OrderTotal", order.retrieveOrderTotal());

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteOrder(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Orders WHERE OrderID = @OrderID", connection);
                command.Parameters.AddWithValue("@OrderID", orderId);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }
    }
}