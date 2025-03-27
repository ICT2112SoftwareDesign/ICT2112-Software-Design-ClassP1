using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http.Features;

namespace CleanBrilliantCompany.Mappers
{
    public class ItemMapper : IItemDatabase
    {
        private readonly string _connectionString;

        public ItemMapper(string connectionString)
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

        // get all items
        public List<Item> getAllItems()
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve items
                string query = @"
                    SELECT itemId, Item.productId, Product.productName, salePrice, Item.batchCode, itemStatus, 
                        ProductBatch.expiryDate, warehouseId, reservationId, orderId, transferId, returnId 
                    FROM Item
                    INNER JOIN ProductBatch ON Item.batchCode = ProductBatch.batchCode
                    INNER JOIN Product ON Item.productId = Product.productId
                    ORDER BY ProductBatch.expiryDate ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Execute the query and get the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            // Iterate through each row in the result set
                            while (reader.Read())
                            {
                                ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));
                                // Create the Item object using the constructor
                                Item item = new Item(
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("salePrice")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    status,
                                    reader.IsDBNull(reader.GetOrdinal("reservationId")) ? null : reader.GetInt32(reader.GetOrdinal("reservationId")),
                                    reader.IsDBNull(reader.GetOrdinal("orderId")) ? null : reader.GetInt32(reader.GetOrdinal("orderId")),
                                    reader.IsDBNull(reader.GetOrdinal("transferId")) ? null : reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetDateTime(reader.GetOrdinal("expiryDate"))
                                );

                                // Add the item to the list
                                items.Add(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found for the query.");
                        }
                    }
                }
            }

            return items;
        }

        // get 1 item by id
        public Item getItemById(int itemId)
        {
            Item item = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the item by its ID
                string query =
                         @"SELECT itemId, Item.productId, Product.productName, salePrice, Item.batchCode, itemStatus, 
                        ProductBatch.expiryDate, warehouseId, reservationId, orderId, transferId, returnId 
                        FROM Item
                        INNER JOIN ProductBatch ON Item.batchCode = ProductBatch.batchCode
                        INNER JOIN Product ON Item.productId = Product.productId
                        WHERE itemId = @itemId
                        ORDER BY ProductBatch.expiryDate ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemId", itemId);
                    // Execute the query and get the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            // Iterate through each row in the result set
                            while (reader.Read())
                            {
                                ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));

                                // Create the Item object using the constructor
                                item = new Item(
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("salePrice")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    status,
                                    reader.IsDBNull(reader.GetOrdinal("reservationId")) ? null : reader.GetInt32(reader.GetOrdinal("reservationId")),
                                    reader.IsDBNull(reader.GetOrdinal("orderId")) ? null : reader.GetInt32(reader.GetOrdinal("orderId")),
                                    reader.IsDBNull(reader.GetOrdinal("transferId")) ? null : reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetDateTime(reader.GetOrdinal("expiryDate"))
                                );
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No item found with itemId {itemId}.");
                        }
                    }
                }
            }

            return item;
        }

        // get item by product name
        public List<Item> getItemByProductName(string productName)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the item by its ID
                string query =
                        @"SELECT itemId, Item.productId, Product.productName, salePrice, Item.batchCode, itemStatus, 
                        ProductBatch.expiryDate, warehouseId, reservationId, orderId, transferId, returnId 
                        FROM Item
                        INNER JOIN ProductBatch ON Item.batchCode = ProductBatch.batchCode
                        INNER JOIN Product ON Item.productId = Product.productId
                        WHERE Product.productName = @productName
                        ORDER BY ProductBatch.expiryDate ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productName", productName);
                    // Execute the query and get the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            // Iterate through each row in the result set
                            while (reader.Read())
                            {
                                ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));

                                // Create the Item object using the constructor
                                Item item = new Item(
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("salePrice")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    status,
                                    reader.IsDBNull(reader.GetOrdinal("reservationId")) ? null : reader.GetInt32(reader.GetOrdinal("reservationId")),
                                    reader.IsDBNull(reader.GetOrdinal("orderId")) ? null : reader.GetInt32(reader.GetOrdinal("orderId")),
                                    reader.IsDBNull(reader.GetOrdinal("transferId")) ? null : reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId")),
                                    reader.GetString(reader.GetOrdinal("productName")),
                                    reader.GetDateTime(reader.GetOrdinal("expiryDate"))
                                );
                                items.Add(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"No item found with searched product name.");
                        }
                    }
                }
            }

            return items;
        }

        // get item by status (for reserve feature)
        public List<Item> getItemByStatus(ItemStatus itemStatus)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the item by its ID
                string query = @"SELECT * FROM Item 
                INNER JOIN ProductBatch ON ProductBatch.productId = Item.productId
                AND ProductBatch.batchCode = Item.batchCode
                WHERE itemStatus = @status";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@itemStatus", itemStatus).ToString();
                    // Execute the query and get the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            // Iterate through each row in the result set
                            while (reader.Read())
                            {
                                ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));

                                // Create the Item object using the constructor
                                Item item = new Item(
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("salePrice")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    status,
                                    reader.IsDBNull(reader.GetOrdinal("reservationId")) ? null : reader.GetInt32(reader.GetOrdinal("reservationId")),
                                    reader.IsDBNull(reader.GetOrdinal("orderId")) ? null : reader.GetInt32(reader.GetOrdinal("orderId")),
                                    reader.IsDBNull(reader.GetOrdinal("transferId")) ? null : reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId"))
                                );
                                // Add the item to the list
                                items.Add(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"FAILED TO RETRIEVES ITEMS OF {itemStatus}.");
                        }
                    }
                }
            }

            return items;
        }

        // create item
        public bool createItem(int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
            INSERT INTO dbo.Item (productId, salePrice, batchCode, warehouseId, itemStatus) 
            VALUES (@productId, @salePrice, @batchCode, @warehouseId, @itemStatus);";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@salePrice", salePrice);
                    command.Parameters.AddWithValue("@batchCode", batchCode);
                    command.Parameters.AddWithValue("@warehouseId", warehouseId);
                    command.Parameters.AddWithValue("@itemStatus", status.ToString());

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }

        // update item sales price 
        public bool updateItem(int itemId, float salesPrice)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
            UPDATE dbo.Item SET salePrice = @salePrice WHERE itemId = @itemId;";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@itemId", itemId);
                    command.Parameters.AddWithValue("@salePrice", salesPrice);

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }

        // for iItemUpdate 
        public bool updateItemStatus(int itemId, int? reservationId, int? orderId, int? transferId, int? returnId, ItemStatus status)
        {
            Console.WriteLine("STATUS TO ADD IN DB" + status);
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                UPDATE dbo.Item SET reservationId = @reservationId, orderId = @orderId, transferId = @transferId, returnId = @returnId, itemStatus = @status
                WHERE itemId = @itemId;";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@itemId", itemId);
                    command.Parameters.AddWithValue("@reservationId", reservationId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@orderId", orderId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@transferId", transferId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@returnId", returnId ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@status", status.ToString());

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }


        public bool updateItemStatusOld(int itemId, ItemStatus status)
        {

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
            UPDATE dbo.Item SET itemStatus = @status WHERE itemId = @itemId;";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@itemId", itemId);
                    command.Parameters.AddWithValue("@status", status);

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
                }
            }
        }

        // get all warehouse details
        public Warehouse getWarehouseDetails(int warehouseId)
        {
            Warehouse warehouse = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve warehouse details
                string query = @"
                    SELECT Warehouse.warehouseId, warehouseAddress, currentCapacity, maxCapacity, Product.productId, Product.quantity, Item.ItemId
                    FROM Warehouse 
                    INNER JOIN Item ON Warehouse.warehouseId = Item.warehouseId
                    INNER JOIN Product ON Item.productId = Product.productId
                    WHERE warehouseId = @warehouseId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@warehouseID", warehouseId);
                    // Execute the query and get the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            // Iterate through each row in the result set
                            while (reader.Read())
                            {
                                // // Create the Item object using the constructor
                                // warehouse = new Warehouse(
                                //     reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                //     reader.GetString(reader.GetOrdinal("warehouseAddress")),
                                //     reader.GetInt32(reader.GetOrdinal("currentCapacity")),
                                //     reader.GetInt32(reader.GetOrdinal("maxCapactiy")),
                                //     reader.GetInt32(reader.GetOrdinal("productId")),
                                //     reader.GetInt32(reader.GetOrdinal("quantity")),
                                //     reader.GetInt32(reader.GetOrdinal("itemId"))
                                // );
                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found for the query.");
                        }
                    }
                }
            }

            return warehouse;
        }

        public List<Item> getItemByProductAndWarehouse(int productId, int warehouseId)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the item by its ID
                string query = @"SELECT * FROM Item 
                WHERE productId = @productId AND warehouseId = @warehouseId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@warehouseId", warehouseId);
                    // Execute the query and get the results
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        // Check if the query executed successfully and returned any rows
                        if (getDatabaseQueryStatus(reader))
                        {
                            // Iterate through each row in the result set
                            while (reader.Read())
                            {
                                ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));

                                // Create the Item object using the constructor
                                Item item = new Item(
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("salePrice")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    status,
                                    reader.IsDBNull(reader.GetOrdinal("reservationId")) ? null : reader.GetInt32(reader.GetOrdinal("reservationId")),
                                    reader.IsDBNull(reader.GetOrdinal("orderId")) ? null : reader.GetInt32(reader.GetOrdinal("orderId")),
                                    reader.IsDBNull(reader.GetOrdinal("transferId")) ? null : reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId"))
                                );
                                // Add the item to the list
                                items.Add(item);
                            }
                        }
                        else
                        {
                            Console.WriteLine($"FAILED TO RETRIEVES ITEMS {items}.");
                        }
                    }
                }
            }

            return items;
        }

        public int getProductQuantityByWarehouse(int productId, int warehouseId)
        {
            int totalQuantity = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the item by its ID
                string query = @"
                SELECT SUM(quantity) AS 'Total quantity' FROM Item 
                INNER JOIN Product ON Product.productId = Item.productId
                INNER JOIN Warehouse ON Warehouse.warehouseId = Item.warehouseId
                WHERE Product.productId = @productId AND Item.warehouseId = @warehouseId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("@warehouseId", warehouseId);

                    // Execute the query using ExecuteScalar()
                    object result = command.ExecuteScalar();

                    // Check if the query returned valid data
                    if (result != null && result != DBNull.Value)
                    {
                        int rowsAffected = Convert.ToInt32(result); // Total quantity result
                        if (getDatabaseQueryStatus(null, rowsAffected))
                        {
                            totalQuantity = rowsAffected;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Query returned no results.");
                    }
                }

                return totalQuantity;
            }
        }

        // handle refunded items
        public void returnItemToInventory(List<int> itemIds, string refundReason)
        {
            if (refundReason == "Defect")
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string updateQuery = $@"
                        UPDATE Item 
                        SET itemStatus = 'ToReturn', orderId = NULL
                        WHERE itemId IN ({string.Join(",", itemIds)}) 
                        AND orderId IS NOT NULL 
                        AND itemStatus = 'Sold';";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        // to deduct product qty & update item status to ordered
        public List<Item> adjustInventory(int orderId, Dictionary<int, int> orderProducts)
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                foreach (var kvp in orderProducts)
                {
                    int productId = kvp.Key;
                    int quantity = kvp.Value;

                    // first query to return the items to be updated
                    string selectOrderItemsQuery = @"
                    SELECT TOP (@quantity) * 
                    FROM Item
                    INNER JOIN ProductBatch ON Item.batchCode = ProductBatch.batchCode
                    INNER JOIN Product ON Item.productId = Product.productId
                    WHERE itemStatus = 'Available' AND Item.productId = @productId
                    ORDER BY ProductBatch.expiryDate ASC;";

                    List<int> updatedItemIds = new List<int>();

                    using (SqlCommand command = new SqlCommand(selectOrderItemsQuery, connection))
                    {
                        // command.Parameters.AddWithValue("@orderId", orderId);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.Parameters.AddWithValue("@productId", productId);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));
                                updatedItemIds.Add(reader.GetInt32(0)); // list of item id
                                Item item = new Item(
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    (float)reader.GetDouble(reader.GetOrdinal("salePrice")),
                                    reader.GetInt32(reader.GetOrdinal("batchCode")),
                                    reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                    status,
                                    reader.IsDBNull(reader.GetOrdinal("reservationId")) ? null : reader.GetInt32(reader.GetOrdinal("reservationId")),
                                    reader.IsDBNull(reader.GetOrdinal("orderId")) ? null : reader.GetInt32(reader.GetOrdinal("orderId")),
                                    reader.IsDBNull(reader.GetOrdinal("transferId")) ? null : reader.GetInt32(reader.GetOrdinal("transferId")),
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId"))
                                );
                                // Add the item to the list
                                items.Add(item);
                            }
                        }
                        Console.WriteLine("ITEMS: " + items);

                        Console.WriteLine("UPDATED ITEMS NUMBER: " + updatedItemIds.Count);

                        if (updatedItemIds.Count == 2)
                        {
                            // Update the selected items using updateItemStatus method
                            foreach (int itemId in updatedItemIds)
                            {
                                updateItemStatus(itemId, null, orderId, null, null, ItemStatus.Sold);
                                
                            }
                        }
                    }
                }
            }

            return items;
        }

        // to cancel order (would need to edit the orderId to null)
        public void processCancelledOrder(int orderId)
        {
            Console.WriteLine("PROCESS ITEM MAPPER: " + orderId.GetType());
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                UPDATE Item SET orderId = NULL, itemStatus = 'Available' WHERE orderId = @orderId";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@orderId", orderId);

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    Console.WriteLine("ROWS AFFECTED: " + rowsAffected);
                }


            }
        }

    }
}