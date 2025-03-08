using System;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Mappers
{
    public class CustomerMapper : iCustomerDatabase
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

                string query = @"
                    INSERT INTO dbo.Customer (username, password, email) 
                    VALUES (@Username, @Password, @Email)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
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
                string query = "SELECT COUNT(1) FROM dbo.Customer WHERE email = @Email AND password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();
                    return count == 1;
                }
            }
        }
    }
}