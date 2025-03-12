using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace CleanBrilliantCompany.Mappers
{
    public class ProductMapper
    {
        private readonly string _connectionString;

        public ProductMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Product getProductDetails(int productId)
        {
            // Console.WriteLine($"Fetching product details for Product ID: {productId}");

            // using (SqlConnection connection = new SqlConnection(_connectionString))
            // {
            //     connection.Open();

            //     string query = @"
            //         SELECT productId, productName, productCategory, costPrice, manufacturerId, 
            //             weight, quantity, volume, toxicityPercentage, carbonFootprint, productState
            //         FROM dbo.Product
            //         WHERE productId = @ProductId";

            //     using (SqlCommand command = new SqlCommand(query, connection))
            //     {
            //         command.Parameters.AddWithValue("@ProductId", productId);

            //         using (SqlDataReader reader = command.ExecuteReader())
            //         {
            //             if (reader.Read())
            //             {
            //                 return new Product
            //                 {
            //                     ProductId = reader.GetInt32(reader.GetOrdinal("productId")),
            //                     ProductName = reader.GetString(reader.GetOrdinal("productName")),
            //                     ProductCategory = reader.GetString(reader.GetOrdinal("productCategory")),
            //                     CostPrice = (float)reader.GetDouble(reader.GetOrdinal("costPrice")),
            //                     ManufacturerId = reader.GetInt32(reader.GetOrdinal("manufacturerId")),
            //                     Weight = (float)reader.GetDouble(reader.GetOrdinal("weight")),
            //                     Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
            //                     Volume = reader.GetInt32(reader.GetOrdinal("volume")),
            //                     ToxicityPercentage = (float)reader.GetDouble(reader.GetOrdinal("toxicityPercentage")),
            //                     CarbonFootprint = reader.GetInt32(reader.GetOrdinal("carbonFootprint")),
            //                     ProductState = reader.IsDBNull(reader.GetOrdinal("productState")) 
            //                         ? null 
            //                         : reader.GetString(reader.GetOrdinal("productState")) 
            //                 };
            //             }
            //         }
            //     }
            // }
            return null; // No product found
        }


        public void createProduct(string productName, string category, float costPrice, 
        int manufacturerId, float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, int productState)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO dbo.Product (productName, productCategory, productCost, manufacturerId, 
                                            productWeight, quantity, volume, toxicityPercentage, carbonFootprint, productState)
                    VALUES (@ProductName, @Category, @CostPrice, @ManufacturerId, 
                            @ProductWeight, @Quantity, @Volume, @ToxicityPercentage, @CarbonFootprint, @ProductState)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductName", productName);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.AddWithValue("@CostPrice", costPrice);
                    command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);
                    command.Parameters.AddWithValue("@ProductWeight", productWeight);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Volume", volume);
                    command.Parameters.AddWithValue("@ToxicityPercentage", toxicityPercentage);
                    command.Parameters.AddWithValue("@CarbonFootprint", carbonFootprint);
                    command.Parameters.AddWithValue("@ProductState", productState);

                    int result = command.ExecuteNonQuery();
                    if (result > 0)
                    {
                        Console.WriteLine($"Product '{productName}' inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Error inserting product.");
                    }
                }
            }
        }

        // Fetch all products from the database
        public List<Product> GetAllProducts()
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
                                CostPrice = (float)reader.GetDouble(reader.GetOrdinal("productCost")),
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
    }
}