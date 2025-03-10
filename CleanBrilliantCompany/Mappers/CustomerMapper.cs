using System;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;
using Microsoft.AspNetCore.Identity;


namespace CleanBrilliantCompany.Mappers
{
    public class CustomerMapper : iCustomerDatabase
    {
        private readonly string _connectionString;
        private readonly iCustomerQueryObserver _observer;
        public CustomerMapper(string connectionString, iCustomerQueryObserver observer)
        {
            _connectionString = connectionString;
            _observer = observer;
        }


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
                    return result > 0;
                }
            }
        }

        public bool CustomerExists(string email)
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

        public bool updateCustomer(string username, string password, string customerAddress, string email)
        {
            // Implementation logic here
            return false;
        }

        public bool VerifyCustomerCredentials(string email, string password)
        {
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
                        return false; 
                        
                    var passwordHasher = new PasswordHasher<object>();
                    var result = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, password);
                    _observer.notifyDBCustomerQueryStatus();
                    return result == PasswordVerificationResult.Success;
                }
            }
        }
    }
}