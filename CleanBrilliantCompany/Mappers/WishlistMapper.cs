using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;

public class WishlistMapper : IWishlistDatabase
{
    private readonly string _connectionString;
    
    public WishlistMapper(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public string getWishlistProductIdsString(int customerId)
    {
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
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            using (var command = new SqlCommand(
                "SELECT COUNT(*) FROM wishlist WHERE customerID = @CustomerId", 
                connection))
            {
                command.Parameters.AddWithValue("@CustomerId", customerId);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
    
    public bool saveWishlistProductIdsString(int customerId, string productIdsString)
    {
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
            }
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}