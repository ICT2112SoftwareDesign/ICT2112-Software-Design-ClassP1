using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;


public class ShippingAgent
{
    public int ShippingAgentId { get; set; }
    public required string? ShippingAgentCompany { get; set; }
    public required string? ShippingMethod { get; set; }
    public required string? ServiceType { get; set; }
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
                Console.WriteLine("Connected to Azure SQL Database successfully!");

                string query = "SELECT * FROM ShippingAgent";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int rowCount = 0;  // 🔹 Track row count
                    while (reader.Read())
                    {
                        rowCount++;
                        agents.Add(new ShippingAgent
                        {
                            ShippingAgentId = Convert.ToInt32(reader["shippingAgentId"]),
                            ShippingAgentCompany = reader["shippingAgentCompany"].ToString(),
                            ShippingMethod = reader["shippingMethod"].ToString(),
                            ServiceType = reader["serviceType"].ToString()
                        });
                    }

                    Console.WriteLine($"Rows Retrieved: {rowCount}");  // 🔹 Print the number of rows fetched
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        return agents;
    }

}