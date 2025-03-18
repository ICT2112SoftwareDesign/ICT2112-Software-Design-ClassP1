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

        // Product
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
                return $"Error inserting product: {ex.Message}"; 
            }
        }

        public async Task<string> delete(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 

                    string query = @"
                        DELETE FROM dbo.Product
                        WHERE productId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);

                        int result = await command.ExecuteNonQueryAsync(); 
                        return result > 0 ? $"Product ID {productId} deleted successfully." : $"Product ID {productId} not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
                return $"Error deleting product: {ex.Message}"; 
            }
        }


        public async Task<List<Product>> findAllProducts()
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

        // Batch
        public async Task<List<ProductBatch>> findAllProductBatch()
        {
            List<ProductBatch> batches = new List<ProductBatch>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync(); 

                    string query = @"
                        SELECT batchCode, productId, expiryDate, receiveDate, manufactureDate, 
                            quantity, batchCost
                        FROM dbo.ProductBatch"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync()) 
                            {
                                batches.Add(new ProductBatch
                                {
                                    BatchCode = reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    ProductId = reader.GetInt32(reader.GetOrdinal("productId")),
                                    ExpiryDate = reader.GetDateTime(reader.GetOrdinal("expiryDate")), 
                                    ReceiveDate = reader.GetDateTime(reader.GetOrdinal("receiveDate")), 
                                    ManufactureDate = reader.GetDateTime(reader.GetOrdinal("manufactureDate")), 
                                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                    BatchCost = (int)reader.GetDouble(reader.GetOrdinal("batchCost")) 
                                });
                            }
                        }
                    }
                }
                return batches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product batches: {ex.Message}");
                return new List<ProductBatch>();
            }
        }

        public async Task<ProductBatch> findByBatchCode(int batchCode)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT batchCode, productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost
                    FROM dbo.ProductBatch
                    WHERE batchCode = @BatchCode";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BatchCode", batchCode);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new ProductBatch
                            {
                                BatchCode = reader.GetInt32(reader.GetOrdinal("batchCode")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("productId")),
                                ExpiryDate = reader.GetDateTime(reader.GetOrdinal("expiryDate")),
                                ReceiveDate = reader.GetDateTime(reader.GetOrdinal("receiveDate")),
                                ManufactureDate = reader.GetDateTime(reader.GetOrdinal("manufactureDate")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                BatchCost = (int)reader.GetDouble(reader.GetOrdinal("batchCost"))
                            };
                        }
                    }
                }
            }

            return null; 
        }

        public async Task<string> insert(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    INSERT INTO dbo.ProductBatch (productId, expiryDate, receiveDate, manufactureDate, 
                                                quantity, batchCost)
                    SELECT @ProductId, @ExpiryDate, @ReceiveDate, @ManufactureDate, @Quantity, @BatchCost
                    WHERE EXISTS (SELECT 1 FROM dbo.Product WHERE productId = @ProductId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                    command.Parameters.AddWithValue("@ReceiveDate", receiveDate);
                    command.Parameters.AddWithValue("@ManufactureDate", manufactureDate);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@BatchCost", batchCost);

                    int result = await command.ExecuteNonQueryAsync();

                    if (result > 0)
                    {
                        Console.WriteLine("Batch inserted successfully.");
                        return "Batch inserted successfully.";
                    }
                    else
                    {
                        Console.WriteLine("Error: Product ID does not exist.");
                        return "Error: Product ID does not exist.";
                    }
                }
            }
        }

        // StockHistory
        public async Task<List<StockHistory>> findStockHistoryByBatchCode(int batchCode)
        {
            List<StockHistory> stockHistoryList = new List<StockHistory>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT stockId, batchCode, stockCheckDate, quantity, timeRecorded
                    FROM dbo.StockHistory
                    WHERE batchCode = @BatchCode
                    ORDER BY stockCheckDate DESC";  // Orders by most recent stock check

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BatchCode", batchCode);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // If timeRecorded is of type TIME, we convert it to DateTime
                            DateTime timeRecorded = DateTime.MinValue.Add(reader.GetTimeSpan(reader.GetOrdinal("timeRecorded")));
                            stockHistoryList.Add(new StockHistory
                            {
                                StockId = reader.GetInt32(reader.GetOrdinal("stockId")),
                                BatchCode = reader.GetInt32(reader.GetOrdinal("batchCode")),
                                StockCheckDate = reader.GetDateTime(reader.GetOrdinal("stockCheckDate")),
                                Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                // TimeRecorded = reader.GetDateTime(reader.GetOrdinal("timeRecorded"))
                                TimeRecorded = timeRecorded
                            });
                        }
                    }
                }
            }

            return stockHistoryList;
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

        public async Task<(string status, List<ProductBatch> batch)> getDatabaseQueryStatus(Task<List<ProductBatch>> batch)
        {
            try
            {
                List<ProductBatch> batchList = await batch;

                if (batchList.Count > 0)
                {
                    return ("Query executed successfully", batchList);
                }
                else
                {
                    return ("No product batches found", batchList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database query failed: {ex.Message}");
                return ($"Database query failed: {ex.Message}", new List<ProductBatch>()); 
            }
        }

        public async Task<ProductBatch> getDatabaseQueryStatus(Task<ProductBatch> task)
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

        public async Task<(string status, List<StockHistory> stockHistory)> getDatabaseQueryStatus(Task<List<StockHistory>> task)
        {
            try
            {
                List<StockHistory> stockHistoryList = await task;

                if (stockHistoryList.Count > 0)
                {
                    return ("Query executed successfully", stockHistoryList);
                }
                else
                {
                    return ("No stock history found", stockHistoryList);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database query failed: {ex.Message}");
                return ($"Database query failed: {ex.Message}", new List<StockHistory>());
            }
        }
    }
}