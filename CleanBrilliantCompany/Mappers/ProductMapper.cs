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

        public void update(int productId, int quantity)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE dbo.Product
                        SET quantity = @Quantity
                        WHERE productId = @ProductId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@Quantity", quantity);

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
                                    ProductBatch batch = new ProductBatch(
                                        reader.GetInt32(reader.GetOrdinal("batchCode")),
                                        reader.GetInt32(reader.GetOrdinal("productId")),
                                        reader.GetDateTime(reader.GetOrdinal("expiryDate")), 
                                        reader.GetDateTime(reader.GetOrdinal("receiveDate")), 
                                        reader.GetDateTime(reader.GetOrdinal("manufactureDate")), 
                                        reader.GetInt32(reader.GetOrdinal("quantity")),
                                        (int)reader.GetDouble(reader.GetOrdinal("batchCost")) 
                                    );

                                    // Add the product to the list
                                    batches.Add(batch);
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
                                (
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    reader.GetDateTime(reader.GetOrdinal("expiryDate")),
                                    reader.GetDateTime(reader.GetOrdinal("receiveDate")),
                                    reader.GetDateTime(reader.GetOrdinal("manufactureDate")),
                                    reader.GetInt32(reader.GetOrdinal("quantity")),
                                    (int)reader.GetDouble(reader.GetOrdinal("batchCost"))
                                );
                            }
                        }
                    }
                }
            }
            return null; 
        }

        public int insert(int productId, DateTime expiryDate, 
    DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    DECLARE @OutputTable TABLE (batchCode INT);
                    DECLARE @NewBatchCode INT;

                    IF EXISTS (SELECT 1 FROM dbo.Product WHERE productId = @ProductId)
                    BEGIN
                        INSERT INTO dbo.ProductBatch (productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost)
                        OUTPUT INSERTED.batchCode INTO @OutputTable(batchCode)
                        VALUES (@ProductId, @ExpiryDate, @ReceiveDate, @ManufactureDate, @Quantity, @BatchCost);

                        SELECT @NewBatchCode = batchCode FROM @OutputTable;
                    END

                    SELECT ISNULL(@NewBatchCode, -1);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductId", productId);
                    command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                    command.Parameters.AddWithValue("@ReceiveDate", receiveDate);
                    command.Parameters.AddWithValue("@ManufactureDate", manufactureDate);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@BatchCost", batchCost);

                    object result = command.ExecuteScalar();
                    int newBatchCode = Convert.ToInt32(result);

                    if (newBatchCode == -1)
                    {
                        Console.WriteLine("Error: Product ID does not exist. Batch not inserted.");
                    }
                    else
                    {
                        Console.WriteLine($"Batch inserted successfully. New BatchCode: {newBatchCode}");
                    }

                    return newBatchCode;
                }
            }
        }


        // StockHistory
        public List<StockHistory> findAllStockHistory()
        {
            List<StockHistory> stockHistoryList = new List<StockHistory>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT stockId, batchCode, stockTakeDate, quantity, recordedDate
                    FROM dbo.StockHistory
                    ORDER BY stockTakeDate DESC";  // Orders by most recent stock check

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                DateTime stockTakeDateTime = reader.GetDateTime(reader.GetOrdinal("stockTakeDate"));
                                DateOnly stockTakeDate = DateOnly.FromDateTime(stockTakeDateTime);
                                StockHistory stockHistory = new StockHistory
                                (
                                    reader.GetInt32(reader.GetOrdinal("stockId")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    stockTakeDate,
                                    reader.GetInt32(reader.GetOrdinal("quantity")),
                                    reader.GetDateTime(reader.GetOrdinal("recordedDate"))
                                );

                                stockHistoryList.Add(stockHistory);
                            }
                        }
                    }

                }

            }
            return stockHistoryList;
        }

        public void insert(int batchCode, DateOnly stockTakeDate, int quantity, DateTime recordedDate)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    INSERT INTO dbo.StockHistory (batchCode, stockTakeDate, quantity, recordedDate)
                    VALUES (@BatchCode, @StockTakeDate, @Quantity, @RecordedDate);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@BatchCode", batchCode);
                    command.Parameters.AddWithValue("@StockTakeDate", stockTakeDate.ToDateTime(new TimeOnly(0, 0)));
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@RecordedDate", recordedDate);

                    int rowsAffected = command.ExecuteNonQuery();
                    // Console.WriteLine($"{rowsAffected} row(s) inserted into StockHistory.");

                    if (getDatabaseQueryStatus(null, rowsAffected))
                    {
                        Console.WriteLine($"{rowsAffected} row(s) inserted into StockHistory.");
                    }
                    else
                    {
                        Console.WriteLine("Error inserting StockHistory.");
                    }
                }
            }
        }

        // ProductManufacturer
        public ProductManufacturer getProductManufacturerById(int manufacturerId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT ManufacturerId, CompanyName, ManufacturerAddress, Email
                    FROM dbo.ProductManufacturer
                    WHERE ManufacturerId = @ManufacturerId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ManufacturerId", manufacturerId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ProductManufacturer manufacturer = new ProductManufacturer
                            (
                                reader.GetInt32(reader.GetOrdinal("ManufacturerId")),
                                reader.GetString(reader.GetOrdinal("CompanyName")),
                                reader.GetString(reader.GetOrdinal("ManufacturerAddress")),
                                reader.GetString(reader.GetOrdinal("Email"))
                            );

                            return manufacturer;
                        }
                    }
                }
            }
            // Return null if not found
            return null;
        }

    }
}