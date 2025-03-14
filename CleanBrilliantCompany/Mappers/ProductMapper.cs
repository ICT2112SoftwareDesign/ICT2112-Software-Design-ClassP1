using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace CleanBrilliantCompany.Mappers
{
    public class ProductMapper : iProductDatabase
    {
        private readonly string _connectionString;

        public ProductMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Product> findByProductId(int productId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT productId, productName, productCategory, productCost, manufacturerId, 
                        productWeight, quantity, volume, toxicityPercentage, carbonFootprint, CAST(productState AS INT) AS productState
                    FROM dbo.Product
                    WHERE productId = @ProductId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Product
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("productId")),
                                ProductName = reader.GetString(reader.GetOrdinal("productName")),
                                ProductCategory = reader.GetString(reader.GetOrdinal("productCategory")),
                                ProductCost = (float)reader.GetDouble(reader.GetOrdinal("productCost")),
                                ManufacturerId = reader.GetInt32(reader.GetOrdinal("manufacturerId")),
                                ProductWeight = (float)reader.GetDouble(reader.GetOrdinal("productWeight")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                Volume = reader.GetInt32(reader.GetOrdinal("volume")),
                                ToxicityPercentage = (float)reader.GetDouble(reader.GetOrdinal("toxicityPercentage")),
                                CarbonFootprint = reader.GetInt32(reader.GetOrdinal("carbonFootprint")),
                                ProductState = reader.GetInt32(reader.GetOrdinal("productState"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<string> insert(string productName, string category, float productCost, 
        int manufacturerId, float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

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

                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
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
                return $"Error inserting product: {ex.Message}"; // ✅ Ensure an error message is returned
            }
        }

        public async Task<List<Product>> findAll()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT productId, productName, productCategory, productCost, manufacturerId, 
                           productWeight, quantity, volume, toxicityPercentage, carbonFootprint
                    FROM dbo.Product";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                ProductId = reader.GetInt32(reader.GetOrdinal("productId")),
                                ProductName = reader.GetString(reader.GetOrdinal("productName")),
                                ProductCategory = reader.GetString(reader.GetOrdinal("productCategory")),
                                ProductCost = (float)reader.GetDouble(reader.GetOrdinal("productCost")),
                                ManufacturerId = reader.GetInt32(reader.GetOrdinal("manufacturerId")),
                                ProductWeight = (float)reader.GetDouble(reader.GetOrdinal("productWeight")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                Volume = reader.GetInt32(reader.GetOrdinal("volume")),
                                ToxicityPercentage = (float)reader.GetDouble(reader.GetOrdinal("toxicityPercentage")),
                                CarbonFootprint = reader.GetInt32(reader.GetOrdinal("carbonFootprint")),
                                // ProductState = reader.GetInt32(reader.GetOrdinal("productState"))
                            });
                        }
                    }
                }
            }
            return products;
        }

        // Interface Methods
        public async Task<Product> getDatabaseQueryStatus(Task<Product> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database query failed: {ex.Message}");
                return null;
            }
        }

        public async Task<string> getDatabaseQueryStatus(Task<string> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database insert failed: {ex.Message}");
                return $"Database insert failed: {ex.Message}";
            }
        }

        public async Task<(string status, List<Product> products)> getDatabaseQueryStatus(Task<List<Product>> task)
        {
            try
            {
                List<Product> products = await task;

                if (products.Count > 0)
                {
                    return ("Query executed successfully", products);
                }
                else
                {
                    return ("No products found", products);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database query failed: {ex.Message}");
                return ($"Database query failed: {ex.Message}", new List<Product>());
            }
        }
    }
}