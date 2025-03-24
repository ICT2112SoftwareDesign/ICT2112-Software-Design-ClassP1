using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Mappers
{   
    // Handles database operations for the shopping cart.
    public class CartMapper : ICartDatabase
    {
        private readonly string _connectionString;

        // Observer to notify about cart-related events
        private readonly ICartQueryObserver _observer;

        public CartMapper(string connectionString,ICartQueryObserver observer)
        {
           _connectionString = connectionString;
            _observer = observer;
        }

        // Adds a cart for a specific customer in the database.
        public bool addCart(int customerID, Dictionary<int, int> productsInCart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Check if a cart already exists for the customer
                        var checkCartCommand = new SqlCommand(
                            "SELECT productsInCart FROM dbo.Cart WHERE customerID = @CustomerID;",
                            connection,
                            transaction
                        );
                        checkCartCommand.Parameters.AddWithValue("@CustomerID", customerID);

                        var existingCart = checkCartCommand.ExecuteScalar();
                        if (existingCart != null)
                        {
                            // Deserialize the existing cart
                            var existingProductsInCart = JsonSerializer.Deserialize<Dictionary<int, int>>(existingCart.ToString());

                            // Merge the new products with the existing cart
                            foreach (var product in productsInCart)
                            {
                                existingProductsInCart[product.Key] = product.Value; // Overwrite quantity
                            }
                            // Serialize the updated cart to JSON
                            string updatedProductsJson = JsonSerializer.Serialize(existingProductsInCart);

                            // Update the existing cart
                            var updateCartCommand = new SqlCommand(
                                "UPDATE dbo.Cart SET productsInCart = @ProductsInCart WHERE customerID = @CustomerID;",
                                connection,
                                transaction
                            );
                            updateCartCommand.Parameters.AddWithValue("@CustomerID", customerID);
                            updateCartCommand.Parameters.AddWithValue("@ProductsInCart", updatedProductsJson);
                            updateCartCommand.ExecuteNonQuery();
                        }
                        else
                        {
                            // Serialize productsInCart to JSON
                            string productsJson = JsonSerializer.Serialize(productsInCart);

                            // Insert a new cart
                            var insertCartCommand = new SqlCommand(
                                "INSERT INTO dbo.Cart (customerID, productsInCart) VALUES (@CustomerID, @ProductsInCart);",
                                connection,
                                transaction
                            );
                            insertCartCommand.Parameters.AddWithValue("@CustomerID", customerID);
                            insertCartCommand.Parameters.AddWithValue("@ProductsInCart", productsJson);
                            insertCartCommand.ExecuteNonQuery();

                             // Notify the observer about the product addition
                            foreach (var product in productsInCart)
                            {
                                _observer.onProductAdded(customerID, product.Key, product.Value);
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in AddCart: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        // Updates the cart for a specific customer in the database.
        public bool updateCart(int customerID, Dictionary<int, int> productsInCart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Retrieve the current cart from the database
                        var query = "SELECT productsInCart FROM dbo.Cart WHERE customerID = @CustomerID";
                        var command = new SqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@CustomerID", customerID);

                        var productsJson = (string)command.ExecuteScalar();
                        var currentCart = JsonSerializer.Deserialize<Dictionary<int, int>>(productsJson) ?? new Dictionary<int, int>();

                        Console.WriteLine("Current cart from database:");
                        foreach (var item in currentCart)
                        {
                            Console.WriteLine($"ProductID: {item.Key}, Quantity: {item.Value}");
                        }

                        // Merge the current cart with the updated cart
                        foreach (var item in productsInCart)
                        {
                            currentCart[item.Key] = item.Value; // Update or add the product

                            // Notify the observer about the product update
                            _observer.onProductQuantityUpdated(customerID, item.Key, item.Value);
                        }

                        // Serialize the updated cart back to JSON
                        var updatedProductsJson = JsonSerializer.Serialize(currentCart);

                        // Update the cart in the database
                        var updateCommand = new SqlCommand("UPDATE dbo.Cart SET productsInCart = @ProductsInCart WHERE customerID = @CustomerID", connection, transaction);
                        updateCommand.Parameters.AddWithValue("@CustomerID", customerID);
                        updateCommand.Parameters.AddWithValue("@ProductsInCart", updatedProductsJson);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in UpdateCart: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }


        // Check if a specific product exists in a customer's cart
        public bool hasProductInCart(int customerID, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT COUNT(1) FROM dbo.Cart WHERE customerID = @CustomerID AND JSON_VALUE(productsInCart, '$.\"' + CAST(@ProductID AS NVARCHAR) + '\"') IS NOT NULL";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);
                    command.Parameters.AddWithValue("@ProductID", productId);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // Removes a product from the cart for a specific customer.
        public bool removeFromCart(int customerID, int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Retrieve the current cart from the database
                        var query = "SELECT productsInCart FROM dbo.Cart WHERE customerID = @CustomerID";
                        var command = new SqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@CustomerID", customerID);

                        var productsJson = (string)command.ExecuteScalar();
                        var currentCart = JsonSerializer.Deserialize<Dictionary<int, int>>(productsJson) ?? new Dictionary<int, int>();

                        Console.WriteLine("Current cart from database before removal:");
                        foreach (var item in currentCart)
                        {
                            Console.WriteLine($"ProductID: {item.Key}, Quantity: {item.Value}");
                        }

                      // Remove the product from the cart
                        if (currentCart.ContainsKey(productId))
                        {
                            currentCart.Remove(productId);

                            // Notify the observer about the product removal
                            _observer.onProductRemoved(customerID, productId);
                        }

                        // Serialize the updated cart back to JSON
                        var updatedProductsJson = JsonSerializer.Serialize(currentCart);

                        // Update the cart in the database
                        var updateCommand = new SqlCommand("UPDATE dbo.Cart SET productsInCart = @ProductsInCart WHERE customerID = @CustomerID", connection, transaction);
                        updateCommand.Parameters.AddWithValue("@CustomerID", customerID);
                        updateCommand.Parameters.AddWithValue("@ProductsInCart", updatedProductsJson);

                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        Console.WriteLine($"Rows affected: {rowsAffected}");

                        transaction.Commit();
                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in RemoveFromCart: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
        
       // Retrieves the cart for a specific customer.
       public bool getCart(int customerID, out Dictionary<int, int> cartData)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var query = "SELECT productsInCart FROM dbo.Cart WHERE customerID = @CustomerID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerID);

                    var productsJson = (string)command.ExecuteScalar();
                    if (!string.IsNullOrEmpty(productsJson))
                    {
                        cartData = JsonSerializer.Deserialize<Dictionary<int, int>>(productsJson) ?? new Dictionary<int, int>();
                        return true;
                    }
                }
            }

            cartData = new Dictionary<int, int>();
            return false;
                }

        // Clears the cart for a specific customer.
        public bool clearCart(int customerID)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Delete the cart row for the customer
                        var query = "DELETE FROM dbo.Cart WHERE customerID = @CustomerID";
                        var command = new SqlCommand(query, connection, transaction);
                        command.Parameters.AddWithValue("@CustomerID", customerID);

                        int rowsAffected = command.ExecuteNonQuery();
                        transaction.Commit();

                        // Notify the observer about the cart clearance
                        _observer.onCartCleared(customerID);

                        return rowsAffected > 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in clearCart: {ex.Message}");
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }          
    }
}