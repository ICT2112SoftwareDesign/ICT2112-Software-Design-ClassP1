using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Data;
using Microsoft.EntityFrameworkCore; // Required for ToListAsync()


namespace CleanBrilliantCompany.Services
{
    public class ShippingAgentService : IShippingAgentService
    {
        private readonly ApplicationDbContext _context;

        public ShippingAgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShippingAgent>> GetShippingAgentsAsync()
        {
            return await _context.ShippingAgents.ToListAsync(); // Fetch from DB
        }
    }
}


public class ShippingAgentDB
{
    private readonly string _connectionString;

    public ShippingAgentDB(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
                        ?? throw new ArgumentNullException("Connection string not found.");
    }

    public List<ShippingAgent> FetchShippingAgents()
    {
        var agents = new List<ShippingAgent>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            try
            {
                conn.Open();
                Console.WriteLine("✅ Connected to Azure SQL Database!");

                string query = "SELECT * FROM ShippingAgent";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int rowCount = 0;
                    Console.WriteLine("🔍 Reading rows...");
                    while (reader.Read())
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
}
