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

        public int createTransfer(int transferId, int productId, int sourceWarehouse, int destinationWarehouse, int quantity, TransferStatus status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                     -- Insert the transfer record and capture the new ID
                    INSERT INTO dbo.ItemTransfer (productId, sourceWarehouse, destinationWarehouse, quantity, transferStatus, staffId) 
                    VALUES (@productId, @sourceWarehouse, @destinationWarehouse, @quantity, @transferStatus, @staffId);

                    SELECT SCOPE_IDENTITY()
                    ;";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@transferId", transferId);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@sourceWarehouse", sourceWarehouse);
                    command.Parameters.AddWithValue("@destinationWarehouse", destinationWarehouse);
                    command.Parameters.AddWithValue("@quantity", quantity);
                    command.Parameters.AddWithValue("@transferStatus", TransferStatus.Pending.ToString());
                    command.Parameters.AddWithValue("@staffId", 1); // Hardcoded staff ID for now
                    //command.Parameters.AddWithValue("@itemStatus", ItemStatus.Transferred.ToString());

                    object scopedIdentity = command.ExecuteScalar(); // Get the new ID
                    if (scopedIdentity != null && int.TryParse(scopedIdentity.ToString(), out int newTransferId))
                    {
                        return newTransferId; // Return the new transferId
                    }
                    else
                    {
                        return -1; // Return -1 if insertion fails
                    }


                    // int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    // return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
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
                                    SET warehouseId = @destinationWarehouse
                                    WHERE transferId = @transferId;
                                END  

                                COMMIT TRANSACTION;
                            END TRY 

                            BEGIN CATCH
                                ROLLBACK TRANSACTION;
                                THROW;
                            END CATCH
                            ;";
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
                    w.warehouseId,
                    w.warehouseName, 
                    COUNT(i.itemId) AS TotalQuantity,
                    w.maxCapacity AS MaxCapacity,
                    w.currentCapacity AS CurrentCapacity,
                    (w.maxCapacity - w.currentCapacity) AS AvailableCapacity
                FROM Warehouse w
                CROSS JOIN Product p  -- Ensures every product is considered for every warehouse
                LEFT JOIN Item i 
                    ON p.productId = i.productId 
                    AND w.warehouseId = i.warehouseId 
                    AND i.itemStatus = 'Available'  -- Only count available items
                GROUP BY p.productId, p.productName, w.warehouseId, w.warehouseName, w.maxCapacity, w.currentCapacity
                HAVING COUNT(i.itemId) < 10; -- Adjust this threshold as needed
                ";
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
                                    reader.GetInt32(reader.GetOrdinal("TotalQuantity")),
                                    reader.GetInt32(reader.GetOrdinal("MaxCapacity")),
                                    reader.GetInt32(reader.GetOrdinal("CurrentCapacity")),
                                    reader.GetInt32(reader.GetOrdinal("AvailableCapacity"))
                                );
                                products.Add(product);
                            }
                        }
                    }
                }
            }
            return products;
        }

        public bool updateWarehouseCapacity(int warehouseId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string updateQuery = @"
                            -- Update inventory count at source warehouse (decrease)
                                UPDATE dbo.Warehouse
                                SET currentCapacity = (
                                    SELECT COUNT(*)
                                    FROM dbo.Item
                                    WHERE warehouseId = @warehouseId
                                    AND itemStatus IN ('Available', 'Reserved', 'ToReturn')
                                )
                                WHERE warehouseId = @warehouseId;

                            ;";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@warehouseId", warehouseId);
                    //command.Parameters.AddWithValue("@destinationWarehouse", destinationWarehouse);
                    //command.Parameters.AddWithValue("@quantity", quantity);
                    //command.Parameters.AddWithValue("@status", status.ToString());

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }




    }
}