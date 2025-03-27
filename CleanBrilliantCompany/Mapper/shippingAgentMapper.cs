using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Mapper
{
    public class ShippingAgentMapper : _IShippingAgentDB
    {
        private readonly string _connectionString;

        public ShippingAgentMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException("Connection string not found.");
        }

        public async Task<List<ShippingAgent>> GetShippingAgentsAsync()
        {
            var agents = new List<ShippingAgent>();

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

                            var agent = new ShippingAgent
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

        public async Task<ShippingAgent> GetShippingAgentByIdAsync(int id)
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
                                return new ShippingAgent
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
            
            return new ShippingAgent(); // Return empty object if not found
        }

        public async Task<bool> AddShippingAgentAsync(ShippingAgent shippingAgent)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    string query = @"
                        INSERT INTO ShippingAgent (shippingAgentCompany, shippingMethod, serviceType)
                        VALUES (@Company, @Method, @Service)";
                    
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Company", shippingAgent.ShippingAgentCompany);
                        cmd.Parameters.AddWithValue("@Method", shippingAgent.ShippingMethod);
                        cmd.Parameters.AddWithValue("@Service", shippingAgent.ServiceType);
                        
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ ERROR adding shipping agent: {ex.Message}\n{ex.StackTrace}");
                    return false;
                }
            }
        }

        public async Task<bool> UpdateShippingAgentAsync(ShippingAgent shippingAgent)
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
        public List<ShippingAgent> FetchShippingAgents()
        {
            return GetShippingAgentsAsync().GetAwaiter().GetResult();
        }
    }
}