using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Mappers
{
    public class TransferMapper : ITransferDatabase
    {
        private readonly string _connectionString;

        public TransferMapper(string connectionString)
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

        public List<Transfer> getAllTransfers()
        {
            List<Transfer> transfers = new List<Transfer>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = @"
                    SELECT it.transferId, it.productId, p.productName, it.quantity, it.sourceWarehouse, it.destinationWarehouse, source.warehouseName AS sourceWarehouseName, 
                    destination.warehouseName AS destinationWarehouseName, it.staffId, it.transferStatus
                    FROM dbo.ItemTransfer it
                    JOIN dbo.Product p 
                    ON it.productId = p.productId
                    JOIN dbo.Warehouse AS source 
                    ON it.sourceWarehouse = source.warehouseId
                    JOIN dbo.Warehouse AS destination 
                    ON it.destinationWarehouse = destination.warehouseId
                    ;";
                // SELECT * FROM dbo.ItemTransfer


                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                TransferStatus status = (TransferStatus)Enum.Parse(typeof(TransferStatus), reader.GetString(reader.GetOrdinal("transferStatus")));
                                Transfer transfer = new Transfer(
                                    reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    status,
                                    reader.GetInt32(reader.GetOrdinal("quantity")),
                                    reader.GetInt32(reader.GetOrdinal("sourceWarehouse")),
                                    reader.GetInt32(reader.GetOrdinal("destinationWarehouse")),
                                    reader.GetInt32(reader.GetOrdinal("staffId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetString(reader.GetOrdinal("sourceWarehouseName")),
                                    reader.GetString(reader.GetOrdinal("destinationWarehouseName"))
                                );
                                transfers.Add(transfer);
                            }
                        }
                    }
                }
            }
            return transfers;
        }

        public bool createTransfer(int transferId, int productId, int sourceWarehouse, int destinationWarehouse, int quantity, TransferStatus status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                    DECLARE @NewTransferId INT;

                    BEGIN TRY 
							BEGIN TRANSACTION;
                    
                    -- Insert the transfer record and capture the new ID
                    INSERT INTO dbo.ItemTransfer (productId, sourceWarehouse, destinationWarehouse, quantity, transferStatus, staffId) 
                    VALUES (@productId, @sourceWarehouse, @destinationWarehouse, @quantity, @transferStatus, @staffId);
                    
                    SET @NewTransferId = SCOPE_IDENTITY();
                    
                    -- Update the specified quantity of items with earliest expiry dates
                    WITH ItemsToUpdate AS (
                        SELECT TOP(@quantity) i.itemId
                        FROM Item i
                        INNER JOIN productBatch pb ON i.batchCode = pb.batchCode
                        WHERE i.productId = @productId 
                        AND i.warehouseId = @sourceWarehouse 
                        AND (i.itemStatus = 'Available' OR i.itemStatus IS NULL)
                        AND (i.transferId IS NULL)
                        ORDER BY pb.expiryDate ASC
                    )
                    UPDATE Item
                    SET itemStatus = @itemStatus, transferId = @NewTransferId
                    -- for update status = completed, update warehouseId to destinationWarehouse for item with selected transferId
                    -- for delete transfer, update transferId in item to null
                    FROM Item i
                    INNER JOIN ItemsToUpdate itu ON i.itemId = itu.itemId;
                    
                    -- Return the count of updated items to verify
                    SELECT @@ROWCOUNT AS ItemsUpdated
                            COMMIT TRANSACTION;
                    END TRY 

					BEGIN CATCH
						ROLLBACK TRANSACTION;
						THROW;
					END CATCH;
                    ;";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@transferId", transferId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@sourceWarehouse", sourceWarehouse);
                    command.Parameters.AddWithValue("@destinationWarehouse", destinationWarehouse);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@transferStatus", "Pending");
                    command.Parameters.AddWithValue("@staffId", 1); // Hardcoded staff ID for now
                    command.Parameters.AddWithValue("@itemStatus", ItemStatus.Transferred.ToString());

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }

        public bool updateTransfer(int transferId, int destinationWarehhouse, TransferStatus status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                            BEGIN TRY 
                                BEGIN TRANSACTION;

                                UPDATE dbo.ItemTransfer 
                                SET transferStatus = @status, destinationWarehouse = @destinationWarehouse
                                WHERE transferId = @transferId;

                                if @status = 'Completed'
                                BEGIN
                                    UPDATE dbo.Item
                                    SET warehouseId = @destinationWarehouse, transferId = null, itemStatus = 'Available'
                                    WHERE transferId = @transferId;
                                END  

                                COMMIT TRANSACTION;
                            END TRY 

                            BEGIN CATCH
                                ROLLBACK TRANSACTION;
                                THROW;
                            END CATCH;";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@transferId", transferId);
                    command.Parameters.AddWithValue("@destinationWarehouse", destinationWarehhouse);
                    //command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@status", status.ToString());

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
            //To use updateItemStatus from iItemUpdate
        }

        public bool deleteTransfer(int transferId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string deleteQuery = @"
            DELETE FROM dbo.ItemTransfer 
            WHERE transferId = @transferId;";

                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@transferId", transferId);

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }

        public List<Product> getLowStockProductInWarehouse()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string selectQuery = @"
                        SELECT 
                        p.productId, 
                        p.productName, 
                        w.warehouseName, 
                        COUNT(i.itemId) AS TotalQuantity, 
                        i.warehouseId
                        FROM Item i
                        INNER JOIN Product p ON p.productId = i.productId
                        INNER JOIN Warehouse w ON w.warehouseId = i.warehouseId  -- Join with Warehouse table
                        GROUP BY p.productId, p.productName, w.warehouseName, i.warehouseId
                        HAVING COUNT(i.itemId) < 10;";
                // SELECT * FROM dbo.ItemTransfer


                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                Product product = new Product(
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetString(reader.GetOrdinal("warehouseName")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    reader.GetInt32(reader.GetOrdinal("TotalQuantity"))
                                );
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