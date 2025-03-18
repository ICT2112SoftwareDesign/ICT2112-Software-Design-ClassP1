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

        public CustomerMapper(string connectionString)
        {
            _connectionString = connectionString;
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
        public int GetIdByEmail(string email)
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

                string query = "SELECT username, email, password, customerAddress FROM dbo.Customer WHERE customerId = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerId", loggedInId);
                    

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            CustomerRDM customer = new CustomerRDM();

                            customer.SetSession("username", reader.GetString(reader.GetOrdinal("username")));
                            customer.SetSession("email", reader.GetString(reader.GetOrdinal("email"))); 
                            customer.SetSession("password", reader.GetString(reader.GetOrdinal("password"))); 
                            customer.SetSession("customerAddress", reader.IsDBNull(reader.GetOrdinal("customerAddress")) ? null : reader.GetString(reader.GetOrdinal("customerAddress")));

                            return customer;
                        }
                    }
                }
            }

            return null;
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

        public bool updateCustomer(int customerId, string field, string value)
        {
            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(value))
            {
                return false;
            }

            // Validate the field to prevent SQL injection
            if (field != "username" && field != "email" && field != "customerAddress")
            {
                return false;
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Use dynamic SQL safely by validating the field name
                string query = $"UPDATE dbo.Customer SET {field} = @Value WHERE customerID = @CustomerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Value", value);
                    command.Parameters.AddWithValue("@CustomerId", customerId);

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        public bool notifyDBCustomerQueryStatus()
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

                    return result == PasswordVerificationResult.Success;
                }
            }
        }
    }
}