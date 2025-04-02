using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;
using System;

public class WishlistMapper : IWishlistDatabase
{
    private readonly string _connectionString;
    private readonly IWishlistQueryObserver _observer;
    
    public WishlistMapper(string connectionString, IWishlistQueryObserver observer)
    {
        _connectionString = connectionString;
        _observer = observer;
    }
    
    public string getWishlistProductIdsString(int customerId)
    {
        _observer.LogGetWishlist(customerId);
        
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(
                "SELECT productIdWIshlist FROM wishlist WHERE customerID = @CustomerId", 
                connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                var result = command.ExecuteScalar();
                
                if (result != null && result != DBNull.Value)
                {
                    return (string)result;
                }
                
                return string.Empty;
            }
        }
    }
    
    public bool customerWishlistExists(int customerId)
    {
        bool exists;
        
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(
                "SELECT COUNT(*) FROM wishlist WHERE customerID = @CustomerId", 
                connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                int count = (int)command.ExecuteScalar();
                exists = count > 0;
            }
        }
        
        _observer.LogWishlistExists(customerId, exists);
        return exists;
    }
    
    public bool saveWishlistProductIdsString(int customerId, string productIdsString)
    {
        bool success = false;
        
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                if (customerWishlistExists(customerId))
                {
                    // Update existing wishlist
                    using (var updateCommand = new SqlCommand(
                        "UPDATE wishlist SET productIdWIshlist = @ProductIds WHERE customerID = @CustomerId", 
                        connection))
                    {
                        updateCommand.Parameters.AddWithValue("@CustomerId", customerId);
                        updateCommand.Parameters.AddWithValue("@ProductIds", productIdsString);
                        updateCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Insert new wishlist record
                    using (var insertCommand = new SqlCommand(
                        "INSERT INTO wishlist (customerID, productIdWIshlist) VALUES (@CustomerId, @ProductIds)", 
                        connection))
                    {
                        insertCommand.Parameters.AddWithValue("@CustomerId", customerId);
                        insertCommand.Parameters.AddWithValue("@ProductIds", productIdsString);
                        insertCommand.ExecuteNonQuery();
                    }
                }
                
                success = true;
            }
        }
        catch (Exception)
        {
            success = false;
        }
        
        _observer.LogSaveWishlist(customerId, productIdsString, success);
        return success;
    }
}