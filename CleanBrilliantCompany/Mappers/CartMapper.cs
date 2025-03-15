using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Mappers
{
    public class CartMapper : ICartDatabase
    {
        private readonly string _connectionString;

        public CartMapper(string connectionString)
        {
           _connectionString = connectionString;
        }

        public bool AddCart(Dictionary<int, int> productsInCart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Serialize productsInCart to JSON
                        string productsJson = JsonSerializer.Serialize(productsInCart);

                        // Insert into Cart table
                        var cartCommand = new SqlCommand("INSERT INTO dbo.Cart (customerID, productsInCart) VALUES (@CustomerID, @ProductsInCart); SELECT SCOPE_IDENTITY();", connection, transaction);
                        int customerID = 1; // Replace with actual customerID
                        cartCommand.Parameters.AddWithValue("@CustomerID", customerID);
                        cartCommand.Parameters.AddWithValue("@ProductsInCart", productsJson);
                        int cartID = Convert.ToInt32(cartCommand.ExecuteScalar());

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        public bool UpdateCart(Dictionary<int, int> productsInCart)
        {
           using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Serialize productsInCart to JSON
                        string productsJson = JsonSerializer.Serialize(productsInCart);

                        // Assuming cartID is known and passed as part of orderItems or another parameter
                        int cartID = 0; // Replace with actual cartID

                        // Update Cart table
                        var updateCommand = new SqlCommand("UPDATE dbo.Cart SET productsInCart = @ProductsInCart WHERE cartID = @CartID", connection, transaction);
                        updateCommand.Parameters.AddWithValue("@CartID", cartID);
                        updateCommand.Parameters.AddWithValue("@ProductsInCart", productsJson);
                        updateCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }
}