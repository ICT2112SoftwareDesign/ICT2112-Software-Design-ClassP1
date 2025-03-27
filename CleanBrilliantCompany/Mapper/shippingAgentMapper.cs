using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System.Linq;

namespace CleanBrilliantCompany.Mapper
{
    public class ShippingAgentMapper : IShippingAgent
    {
        private readonly string _connectionString;

        public ShippingAgentMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string not found.");
        }

        // IShippingAgent interface implementation
        public async Task<IEnumerable<ShippingAgent_RDM>> GetAllShippingAgentsAsync()
        {
            return await GetShippingAgentsAsync();
        }

        public async Task<ShippingAgent_RDM> GetShippingAgentByIdAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    string query = "SELECT * FROM ShippingAgent WHERE shippingAgentId = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new ShippingAgent_RDM
                                {
                                    ShippingAgentId = reader.GetInt32(reader.GetOrdinal("shippingAgentId")),
                                    ShippingAgentCompany = reader.IsDBNull(reader.GetOrdinal("shippingAgentCompany")) ? "N/A" : reader.GetString(reader.GetOrdinal("shippingAgentCompany")),
                                    ShippingMethod = reader.IsDBNull(reader.GetOrdinal("shippingMethod")) ? "N/A" : reader.GetString(reader.GetOrdinal("shippingMethod")),
                                    ServiceType = reader.IsDBNull(reader.GetOrdinal("serviceType")) ? "N/A" : reader.GetString(reader.GetOrdinal("serviceType"))
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR getting shipping agent by ID: {ex.Message}\n{ex.StackTrace}");
                }
            }

            return new ShippingAgent_RDM(); // Return empty object instead of null
        }

        public async Task<ShippingAgent_RDM> AddShippingAgentAsync(ShippingAgent_RDM shippingAgent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();

                    string insertQuery = @"
                INSERT INTO ShippingAgent (shippingAgentCompany, shippingMethod, serviceType)
                VALUES (@Company, @Method, @Service);
                SELECT SCOPE_IDENTITY();"; // Get last inserted ID

                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Company", shippingAgent.ShippingAgentCompany);
                        cmd.Parameters.AddWithValue("@Method", shippingAgent.ShippingMethod);
                        cmd.Parameters.AddWithValue("@Service", shippingAgent.ServiceType);

                        // Execute and get the newly created ID
                        int shippingAgentId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        // Set the ID on the returned object
                        shippingAgent.ShippingAgentId = shippingAgentId;

                        return shippingAgent;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR adding shipping agent: {ex.Message}\n{ex.StackTrace}");
                    return new ShippingAgent_RDM(); // Return empty object instead of null
                }
            }
        }

        public async Task<ShippingAgent_RDM> UpdateShippingAgentAsync(int id, ShippingAgent_RDM shippingAgent)
        {
            shippingAgent.ShippingAgentId = id;
            bool success = await UpdateShippingAgentAsync(shippingAgent);

            if (success)
            {
                return shippingAgent;
            }

            return new ShippingAgent_RDM(); // Return empty object instead of null
        }

        // Helper methods (not part of the interface)
        public async Task<List<ShippingAgent_RDM>> GetShippingAgentsAsync()
        {
            var agents = new List<ShippingAgent_RDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    Console.WriteLine("✅ Connected to Azure SQL Database!");

                    string query = "SELECT * FROM ShippingAgent";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        int rowCount = 0;
                        Console.WriteLine("🔍 Reading rows...");
                        while (await reader.ReadAsync())
                        {
                            rowCount++;
                            Console.WriteLine($"📌 Processing Row {rowCount}...");

                            var agent = new ShippingAgent_RDM
                            {
                                ShippingAgentId = reader.GetInt32(reader.GetOrdinal("shippingAgentId")),
                                ShippingAgentCompany = reader.IsDBNull(reader.GetOrdinal("shippingAgentCompany")) ? "N/A" : reader.GetString(reader.GetOrdinal("shippingAgentCompany")),
                                ShippingMethod = reader.IsDBNull(reader.GetOrdinal("shippingMethod")) ? "N/A" : reader.GetString(reader.GetOrdinal("shippingMethod")),
                                ServiceType = reader.IsDBNull(reader.GetOrdinal("serviceType")) ? "N/A" : reader.GetString(reader.GetOrdinal("serviceType"))
                            };

                            agents.Add(agent);
                            Console.WriteLine($"✅ Added: ID={agent.ShippingAgentId}, Company={agent.ShippingAgentCompany}, Method={agent.ShippingMethod}, Service={agent.ServiceType}");
                        }

                        Console.WriteLine($"✅ Total Rows Retrieved: {rowCount}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR: {ex.Message}\n{ex.StackTrace}");
                }
            }

            return agents;
        }

      

        public async Task<bool> UpdateShippingAgentAsync(ShippingAgent_RDM shippingAgent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    string query = @"
                        UPDATE ShippingAgent 
                        SET shippingAgentCompany = @Company, 
                            shippingMethod = @Method, 
                            serviceType = @Service
                        WHERE shippingAgentId = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", shippingAgent.ShippingAgentId);
                        cmd.Parameters.AddWithValue("@Company", shippingAgent.ShippingAgentCompany);
                        cmd.Parameters.AddWithValue("@Method", shippingAgent.ShippingMethod);
                        cmd.Parameters.AddWithValue("@Service", shippingAgent.ServiceType);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR updating shipping agent: {ex.Message}\n{ex.StackTrace}");
                    return false;
                }
            }
        }

        public async Task<bool> DeleteShippingAgentAsync(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    string query = "DELETE FROM ShippingAgent WHERE shippingAgentId = @Id";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR deleting shipping agent: {ex.Message}\n{ex.StackTrace}");
                    return false;
                }
            }
        }

        // Non-async method for compatibility with existing code
        public List<ShippingAgent_RDM> FetchShippingAgents()
        {
            return GetShippingAgentsAsync().GetAwaiter().GetResult();
        }
    }
}