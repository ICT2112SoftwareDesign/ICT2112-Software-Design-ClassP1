using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Mappers
{
    public class StaffMapper : IStaffDatabase
    {
        private readonly string _connectionString;

        public StaffMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ✅ Insert Staff (Register)
        public bool CreateStaff(string name, string contactNo, string address, string role, string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Hash the password before saving
                var passwordHasher = new PasswordHasher<object>();
                string hashedPassword = passwordHasher.HashPassword(null, password);

                string query = @"
                    INSERT INTO dbo.Staff (name, username, email, contactNo, address, password) 
                    VALUES (@Name, @Username, @Email, @ContactNo, @Address, @Password);
                    SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Username", email); // Using email as username
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@ContactNo", contactNo);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    object result = command.ExecuteScalar();
                    if (result != null && int.TryParse(result.ToString(), out int staffId))
                    {
                        return InsertStaffRole(staffId, role);
                    }
                }
            }
            return false;
        }

        // ✅ Insert Role into the Correct Table (GeneralStaff / ManagementStaff)
        private bool InsertStaffRole(int staffId, string role)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "";

                if (role.ToLower() == "general")
                {
                    query = "INSERT INTO dbo.GeneralStaff (staffId, department) VALUES (@StaffId, 'General Department')";
                }
                else if (role.ToLower() == "management")
                {
                    query = "INSERT INTO dbo.ManagementStaff (staffId, managementLevel) VALUES (@StaffId, 'Manager')";
                }

                if (!string.IsNullOrEmpty(query))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StaffId", staffId);
                        return command.ExecuteNonQuery() > 0;
                    }
                }
            }
            return false;
        }

        // ✅ Verify Staff Credentials (Login)
        public bool VerifyStaffCredentials(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT password FROM dbo.Staff WHERE email = @Email"; // ✅ Changed username to email
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email); // ✅ Changed to email
                    var storedPassword = command.ExecuteScalar()?.ToString();

                    if (string.IsNullOrEmpty(storedPassword))
                    {
                        return false;
                    }

                    return storedPassword == password; // ✅ Plaintext comparison for testing
                }
            }
        }

        // ✅ Check if Staff Exists (By ID)
        public bool StaffExists(int staffId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Staff WHERE staffId = @StaffId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        // ✅ Get Staff ID by Username (For Session)
        public int GetIdByUsername(string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT staffId FROM dbo.Staff WHERE username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    object result = command.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int staffId) ? staffId : -1;
                }
            }
        }

        // ✅ Get Staff Details
        public StaffRDM GetStaffDetails(int staffId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT name, username, email, contactNo, address FROM dbo.Staff WHERE staffId = @StaffId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new StaffRDM
                            {
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                ContactNo = reader.GetString(reader.GetOrdinal("contactNo")),
                                Address = reader.GetString(reader.GetOrdinal("address"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // ✅ Update Staff Details
        public bool UpdateStaff(int staffId, string name, string contactNo, string address, string role, string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    UPDATE dbo.Staff 
                    SET name = @Name, email = @Email, contactNo = @ContactNo, address = @Address
                    WHERE staffId = @StaffId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@ContactNo", contactNo);
                    command.Parameters.AddWithValue("@Address", address);

                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // ✅ Delete Staff
        public bool DeleteStaff(int staffId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "DELETE FROM dbo.Staff WHERE staffId = @StaffId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StaffId", staffId);
                    return command.ExecuteNonQuery() > 0;
                }
            }
        }

        // ✅ Check for Existing Email (For Update)
        public bool StaffEmailExists(int staffId, string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Staff WHERE email = @Email AND staffId != @StaffId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@StaffId", staffId);

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        // ✅ Check for Existing Username (For Update)
        public bool StaffUsernameExists(int staffId, string username)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(1) FROM dbo.Staff WHERE username = @Username AND staffId != @StaffId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@StaffId", staffId);

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        public int GetIdByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT staffId FROM dbo.Staff WHERE email = @Email";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    object result = command.ExecuteScalar();
                    return result != null && int.TryParse(result.ToString(), out int staffId) ? staffId : -1;
                }
            }
        }

        // ✅ Get All Staff (Admin)
        public List<StaffRDM> GetAllStaff()
        {
            List<StaffRDM> staffList = new List<StaffRDM>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT staffId, name, username, email, contactNo, address FROM dbo.Staff";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        staffList.Add(new StaffRDM
                        {
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            ContactNo = reader.GetString(reader.GetOrdinal("contactNo")),
                            Address = reader.GetString(reader.GetOrdinal("address"))
                        });
                    }
                }
            }
            return staffList;
        }
    }
}
