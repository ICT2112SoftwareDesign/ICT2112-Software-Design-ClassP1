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
            // Implementation logic here
            return false;
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