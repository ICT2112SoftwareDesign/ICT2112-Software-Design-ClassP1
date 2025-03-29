using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace CleanBrilliantCompany.Data
{
    public class ProductMapper : IProductDatabase
    {
        private readonly string _connectionString;

        public ProductMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found");
        }

        public bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1)
        {
            try
            {
                // If rowsAffected is provided (not -1), check if rows were affected
                if (rowsAffected != -1)
                {
                    return rowsAffected > 0;
                }

                // Otherwise, check if the reader has rows (for SELECT queries)
                return reader.HasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in database query status: {ex.Message}");
                return false;
            }
        }

        // Product
        public Product findByProductId(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT productId, productName, productCategory, productCost, manufacturerId, 
                        productWeight, quantity, volume, toxicityPercentage, carbonFootprint, productState
                    FROM dbo.Product
                    WHERE productId = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            if (reader.Read())
                            {
                                return new Product(
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetString(reader.GetOrdinal("productCategory")),
                                    (float)reader.GetDouble(reader.GetOrdinal("productCost")),
                                    reader.GetInt32(reader.GetOrdinal("manufacturerId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("productWeight")),
                                    reader.GetInt32(reader.GetOrdinal("quantity")),
                                    reader.GetInt32(reader.GetOrdinal("volume")),
                                    (float)reader.GetDouble(reader.GetOrdinal("toxicityPercentage")),
                                    reader.GetInt32(reader.GetOrdinal("carbonFootprint")),
                                    reader.GetString(reader.GetOrdinal("productState"))
                                );
                            }
                        }
                    }
                }
            }

            return null;
        }

        public string insert(string productName, string category, float productCost, 
        int manufacturerId, float productWeight, int quantity, int volume, 
        float toxicityPercentage, int carbonFootprint, string productState)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        INSERT INTO dbo.Product (productName, productCategory, productCost, manufacturerId, 
                                                productWeight, quantity, volume, toxicityPercentage, carbonFootprint, productState)
                        VALUES (@ProductName, @Category, @ProductCost, @ManufacturerId, 
                                @ProductWeight, @Quantity, @Volume, @ToxicityPercentage, @CarbonFootprint, @ProductState)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@ProductCost", productCost);
                        command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);
                        command.Parameters.AddWithValue("@ProductWeight", productWeight);
                        command.Parameters.AddWithValue("@Quantity", quantity);
                        command.Parameters.AddWithValue("@Volume", volume);
                        command.Parameters.AddWithValue("@ToxicityPercentage", toxicityPercentage);
                        command.Parameters.AddWithValue("@CarbonFootprint", carbonFootprint);
                        command.Parameters.AddWithValue("@ProductState", productState);

                        // Execute the insert operation synchronously
                        int rowsAffected = command.ExecuteNonQuery();

                        // Generate Console.WriteLine output
                        Console.WriteLine($"Insert query executed for product: {productName}");
                        Console.WriteLine($"Category: {category}, Cost: {productCost}, Manufacturer ID: {manufacturerId}");
                        Console.WriteLine($"Weight: {productWeight}, Quantity: {quantity}, Volume: {volume}");
                        Console.WriteLine($"Toxicity: {toxicityPercentage}, Carbon Footprint: {carbonFootprint}, State: {productState}");

                        // Check if the insert was successful using getDatabaseQueryStatus
                        if (getDatabaseQueryStatus(null, rowsAffected))
                        {
                            return $"Product '{productName}' inserted successfully.";
                        }
                        else
                        {
                            return "Error inserting product.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting product: {ex.Message}");
                return $"Error inserting product: {ex.Message}"; 
            }
        }

        public List<Product> findAllProducts()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT productId, productName, productCategory, productCost, manufacturerId, 
                           productWeight, quantity, volume, toxicityPercentage, carbonFootprint, productState
                    FROM dbo.Product";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                Product product = new Product(
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetString(reader.GetOrdinal("productCategory")),
                                    (float)reader.GetDouble(reader.GetOrdinal("productCost")),
                                    reader.GetInt32(reader.GetOrdinal("manufacturerId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("productWeight")),
                                    reader.GetInt32(reader.GetOrdinal("quantity")),
                                    reader.GetInt32(reader.GetOrdinal("volume")),
                                    (float)reader.GetDouble(reader.GetOrdinal("toxicityPercentage")),
                                    reader.GetInt32(reader.GetOrdinal("carbonFootprint")),
                                    reader.GetString(reader.GetOrdinal("productState"))
                                );

                                // Add the product to the list
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            return products;
        }
    }
}