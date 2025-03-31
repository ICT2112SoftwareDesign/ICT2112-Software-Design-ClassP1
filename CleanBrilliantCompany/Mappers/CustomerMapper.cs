using System;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;
using Microsoft.AspNetCore.Identity;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Mappers
{
    public class CustomerMapper : ICustomerDatabase
    {
        private readonly string _connectionString;
        private readonly ICustomerQueryObserver _observer;

        public CustomerMapper(string connectionString, ICustomerQueryObserver observer)
        {
            _connectionString = connectionString;
            _observer = observer;
        }

        // Login
        
        public bool verifyCustomerCredentials(string email, string password)
        {
            // Notify authentication attempt
            _observer.onAuthenticationAttempt(email);

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Step 1: Get stored hashed password based on email
                string query = "SELECT password FROM dbo.Customer WHERE email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    var storedPasswordHash = command.ExecuteScalar()?.ToString();

                    if (string.IsNullOrEmpty(storedPasswordHash))
                    {
                        // Notify authentication failure
                        _observer.onAuthenticationFailure(email, "User not found");
                        return false;
                    }
                        
                    var passwordHasher = new PasswordHasher<object>();
                    var result = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, password);

                    bool isAuthenticated = result == PasswordVerificationResult.Success;
                    
                    if (isAuthenticated)
                    {
                        int customerId = getIdByEmail(email);
                        // Notify authentication success
                        _observer.onAuthenticationSuccess(email, customerId);
                    }
                    else
                    {
                        // Notify authentication failure
                        _observer.onAuthenticationFailure(email, "Invalid password");
                    }

                    return isAuthenticated;
                }
            }
        }

        // Register
        public bool createCustomer(string username, string password, string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Hash the password before saving to the database
                var passwordHasher = new PasswordHasher<object>();
                string hashedPassword = passwordHasher.HashPassword(null, password);

                string query = @"
                    INSERT INTO dbo.Customer (username, password, email) 
                    VALUES (@Username, @Password, @Email)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Email", email);

                    int result = command.ExecuteNonQuery();
                    
                    if (result > 0)
                    {
                        // Notify customer registration
                        _observer.onCustomerRegistration(username, email);
                        return true;
                    }
                    return false;
                }
            }
        }

        // To check if customer exists 
        public bool customerExists(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Customer WHERE email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        public bool customerUsernameExists(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Customer WHERE username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // For session
        public int getIdByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return -1; // Return -1 or an invalid ID to signal failure
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT customerID FROM dbo.Customer WHERE email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int customerId))
                    {
                        return customerId; 
                    }
                    else
                    {
                        return -1; 
                    }
                }
            }
        }

        public CustomerRDM getCustomer(int loggedInId)
        {
            if (loggedInId <= 0)
            {
                return null;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT username, email, password, customerAddress, emailPreference FROM dbo.Customer WHERE customerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", loggedInId);
                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CustomerRDM customer = new CustomerRDM();

                            customer.setSession("username", reader.GetString(reader.GetOrdinal("username")));
                            customer.setSession("email", reader.GetString(reader.GetOrdinal("email"))); 
                            customer.setSession("password", reader.GetString(reader.GetOrdinal("password"))); 
                            customer.setSession("customerAddress", reader.IsDBNull(reader.GetOrdinal("customerAddress")) ? null : reader.GetString(reader.GetOrdinal("customerAddress")));
                            customer.setSession("emailPreference", reader.IsDBNull(reader.GetOrdinal("emailPreference")) ? null : reader.GetString(reader.GetOrdinal("emailPreference")));

                            return customer;
                        }
                    }
                }
            }

            return null;
        }

        // For update feature
        public bool customerUsernameExists(int customerId, string username){
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Customer WHERE username = @Username AND customerId != @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        public bool customerEmailExists(int customerId, string email){
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Customer WHERE email = @Email AND customerId != @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
        }
        
        public bool updateCustomerDetails(int customerId, string username, string email, string address){
            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(address)){
                return false;
            }
             using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Customer 
                    SET username = @Username, email = @Email, customerAddress = @Address 
                    WHERE customerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Address", address);

                    int result = command.ExecuteNonQuery();
                    if (result > 0){
                        // Notify authentication failure
                        _observer.onUpdateDetailsSuccess(customerId, username, email, address);
                        return true;
                    }
                    else{
                        _observer.onUpdatedDetailsFailure(customerId, username, email, address, "Unable to update details to database.");
                        return false;
                    }
                }
                
            }

        }

        public bool updatePassword(int customerId, string password){
            if(string.IsNullOrEmpty(password)){
                return false;
            }
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, password);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Customer 
                    SET password = @Password
                    WHERE customerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    int result = command.ExecuteNonQuery();
                    if(result > 0){
                        _observer.onUpdatePasswordSuccess(customerId, password);
                        return true;
                    }
                    else{
                        _observer.onUpdatePasswordFailure(customerId, password, "Unable to update details to database.");
                        return false;
                    }
                }
            }

        }
        public bool notifyDBCustomerQueryStatus()
        {
            // Implementation logic here
            return false;
        }

        public bool updateEmailPreference(int customerId, string? emailPreference)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Customer 
                    SET emailPreference = @EmailPreference 
                    WHERE customerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", customerId);
                    command.Parameters.AddWithValue("@EmailPreference", 
                        string.IsNullOrEmpty(emailPreference) ? (object)DBNull.Value : emailPreference);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }


    }
}