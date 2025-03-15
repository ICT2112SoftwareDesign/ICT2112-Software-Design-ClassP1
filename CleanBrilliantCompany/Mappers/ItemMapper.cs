using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Mappers
{
    public class ItemMapper : iItemDatabase
    {
        private readonly string _connectionString;

        public ItemMapper(string connectionString)
        {
            _connectionString = connectionString;
        }
        public bool getDatabaseQueryStatus(SqlDataReader reader)
        {
            try
            {
                return reader.HasRows; // Returns true if the query returned any rows
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in database query status: {ex.Message}");
                return false; // Return false if there is an exception
            }
        }

        public List<Item> getAllItems()
        {
            List<Item> items = new List<Item>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve items
                string query = "SELECT itemId, productId, salePrice, batchCode, warehouseId, itemStatus, reservationId, orderId, transferId, returnId FROM Item";

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
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId"))
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

        public Item getItem(int itemId)
        {
            Item item = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Define the SQL query to retrieve the item by its ID
                string query = @"SELECT itemId, productId, salePrice, batchCode, warehouseId, itemStatus, reservationId, orderId, transferId, returnId 
                         FROM Item WHERE itemId = @itemId";

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
                                    reader.IsDBNull(reader.GetOrdinal("returnId")) ? null : reader.GetInt32(reader.GetOrdinal("returnId"))
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

    }
}