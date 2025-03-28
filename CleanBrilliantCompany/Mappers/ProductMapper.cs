using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;


namespace CleanBrilliantCompany.Mappers
{
    public class ProductMapper : IIProductDatabase
    {
        private readonly string _connectionString;

        public ProductMapper(string connectionString)
        {
            _connectionString = connectionString;
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
                                    ProductState = reader.GetString(reader.GetOrdinal("productState"))
                                };
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

        public void delete(int productId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        DELETE FROM dbo.Product
                        WHERE productId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);

                        // Execute the insert operation synchronously
                        int rowsAffected = command.ExecuteNonQuery();

                        // Check if the insert was successful using getDatabaseQueryStatus
                        if (getDatabaseQueryStatus(null, rowsAffected))
                        {
                            Console.WriteLine($"Product Id: '{productId}' deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Error deleting product.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
            }
        }

        public void update(int productId, string productName, string productCategory, 
        float productCost, int manufacturerId, float productWeight, int quantity, int volume, 
        float toxicityPercentage, int carbonFootprint, string productState)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE dbo.Product
                        SET productName = @ProductName,
                            productCategory = @ProductCategory,
                            productCost = @ProductCost,
                            manufacturerId = @ManufacturerId,
                            productWeight = @ProductWeight,
                            quantity = @Quantity,
                            volume = @Volume,
                            toxicityPercentage = @ToxicityPercentage,
                            carbonFootprint = @CarbonFootprint,
                            productState = @ProductState
                        WHERE productId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@ProductName", productName);
                        command.Parameters.AddWithValue("@ProductCategory", productCategory);
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

                         // Check if the insert was successful using getDatabaseQueryStatus
                        if (getDatabaseQueryStatus(null, rowsAffected))
                        {
                            Console.WriteLine($"Product: '{productId}' updated successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Error updating product.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
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
                                    ProductState = reader.GetString(reader.GetOrdinal("productState"))
                                });
                            }
                        }
                    }
                }
            }
            return products;
        }

        // Batch
        public List<ProductBatch> findAllProductBatch()
        {
            List<ProductBatch> batches = new List<ProductBatch>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); 

                    string query = @"
                        SELECT batchCode, productId, expiryDate, receiveDate, manufactureDate, 
                            quantity, batchCost
                        FROM dbo.ProductBatch"; 

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (getDatabaseQueryStatus(reader))
                            {
                                while (reader.Read())
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
                }
                return batches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching product batches: {ex.Message}");
                return new List<ProductBatch>();
            }
        }

        public ProductBatch findByBatchCode(int batchCode)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT batchCode, productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost
                    FROM dbo.ProductBatch
                    WHERE batchCode = @BatchCode";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BatchCode", batchCode);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            if (reader.Read())
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
            }
            return null; 
        }

        public void insert(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

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

                    int rowsAffected = command.ExecuteNonQuery();

                    if (getDatabaseQueryStatus(null, rowsAffected))
                    {
                        Console.WriteLine("Batch inserted successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Error: Product ID does not exist.");
                    }
                }
            }
        }

        // StockHistory
        public List<StockHistory> findStockHistoryByBatchCode(int batchCode)
        {
            List<StockHistory> stockHistoryList = new List<StockHistory>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT stockId, batchCode, stockTakeDate, quantity, recordedDate
                    FROM dbo.StockHistory
                    WHERE batchCode = @BatchCode
                    ORDER BY stockTakeDate DESC";  // Orders by most recent stock check

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BatchCode", batchCode);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                // If timeRecorded is of type TIME, we convert it to DateTime
                                DateTime timeRecorded = DateTime.MinValue.Add(reader.GetTimeSpan(reader.GetOrdinal("recordedDate")));
                                stockHistoryList.Add(new StockHistory
                                {
                                    StockId = reader.GetInt32(reader.GetOrdinal("stockId")),
                                    BatchCode = reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    StockCheckDate = reader.GetDateTime(reader.GetOrdinal("stockTakeDate")),
                                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                                    // TimeRecorded = reader.GetDateTime(reader.GetOrdinal("timeRecorded"))
                                    TimeRecorded = timeRecorded
                                });
                            }
                        }
                    }
                }
            }

            return stockHistoryList;
        }

        // // StockHistory
        // public async Task<List<StockHistory>> findStockHistoryByBatchCode(int batchCode)
        // {
        //     List<StockHistory> stockHistoryList = new List<StockHistory>();

        //     using (SqlConnection connection = new SqlConnection(_connectionString))
        //     {
        //         await connection.OpenAsync();

        //         string query = @"
        //             SELECT stockId, batchCode, stockCheckDate, quantity, timeRecorded
        //             FROM dbo.StockHistory
        //             WHERE batchCode = @BatchCode
        //             ORDER BY stockCheckDate DESC";  // Orders by most recent stock check

        //         using (SqlCommand command = new SqlCommand(query, connection))
        //         {
        //             command.Parameters.AddWithValue("@BatchCode", batchCode);

        //             using (SqlDataReader reader = await command.ExecuteReaderAsync())
        //             {
        //                 while (await reader.ReadAsync())
        //                 {
        //                     // If timeRecorded is of type TIME, we convert it to DateTime
        //                     DateTime timeRecorded = DateTime.MinValue.Add(reader.GetTimeSpan(reader.GetOrdinal("timeRecorded")));
        //                     stockHistoryList.Add(new StockHistory
        //                     {
        //                         StockId = reader.GetInt32(reader.GetOrdinal("stockId")),
        //                         BatchCode = reader.GetInt32(reader.GetOrdinal("batchCode")),
        //                         StockCheckDate = reader.GetDateTime(reader.GetOrdinal("stockCheckDate")),
        //                         Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
        //                         // TimeRecorded = reader.GetDateTime(reader.GetOrdinal("timeRecorded"))
        //                         TimeRecorded = timeRecorded
        //                     });
        //                 }
        //             }
        //         }
        //     }

        //     return stockHistoryList;
        // }


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