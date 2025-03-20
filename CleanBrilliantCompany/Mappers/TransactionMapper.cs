using System;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Models.Mapper
{
    public class TransactionMapper: iTransactionDatabase
    {
        private readonly string _connectionString;

        public TransactionMapper(string connectionString)
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

        //Fields to pass in: transactionDate, adjustmentType, productId, itemId, staffId
        //IMPT: If i need the result of this method later --> CHANGE THE RETURN TYPE TO BOOL
        public void createTransaction(DateTime dateTime, string adjustmentType, int productId, int itemId, int staffId)
        {
            Console.WriteLine("Item is reserved --> Called createTransaction in transactionMapper");
            // using (SqlConnection connection = new SqlConnection(_connectionString))
            // {
            //     connection.Open();

            //     string insertQuery = @"
            //     INSERT INTO dbo.ItemTransaction (transactionDateTime, )
            //     "
            // }
        }
        // public bool createTransaction(DateTime dateTime, ItemStatus adjustmentType, int productId, int itemId, int staffId)
        // {
        //     Console.WriteLine("Called createTransaction in transactionMapper");
        //     // using (SqlConnection connection = new SqlConnection(_connectionString))
        //     // {
        //     //     connection.Open();

        //     //     string insertQuery = @"
        //     // INSERT INTO dbo.Item (productId, salePrice, batchCode, warehouseId, itemStatus) 
        //     // VALUES (@productId, @salePrice, @batchCode, @warehouseId, @itemStatus);";

        //     //     using (SqlCommand command = new SqlCommand(insertQuery, connection))
        //     //     {
        //     //         command.Parameters.AddWithValue("@itemId", itemId);
        //     //         command.Parameters.AddWithValue("@productId", productId);
        //     //         command.Parameters.AddWithValue("@salePrice", salePrice);
        //     //         command.Parameters.AddWithValue("@batchCode", batchCode);
        //     //         command.Parameters.AddWithValue("@warehouseId", warehouseId);
        //     //         command.Parameters.AddWithValue("@itemStatus", status.ToString());

        //     //         int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
        //     //         return getDatabaseQueryStatus(null, rowsAffected); // Pass affected rows to the method
        //     //     }
        //     // }
        // }


        public List<Transaction> getAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = "SELECT transactionId, transactionDateTime, adjustmentType, productId, itemId, staffId FROM ItemTransaction";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                //ItemStatus status = (ItemStatus)Enum.Parse(typeof(ItemStatus), reader.GetString(reader.GetOrdinal("itemStatus")));
                                //replace with AT Enum later on 
                                //TODO
                                // Create the Transaction object using the constructor
                                Transaction transaction = new Transaction(
                                    reader.GetInt32(reader.GetOrdinal("transactionId")),
                                    reader.GetDateTime(reader.GetOrdinal("transactionDateTime")),
                                    reader.GetString(reader.GetOrdinal("adjustmentType")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("staffId"))
                                );

                                //Add transaction to List
                                transactions.Add(transaction);

                            }
                        }
                        else
                        {
                            Console.WriteLine("No data found for the query");
                        }
                    }
                }

            }
            return transactions;
        }

        // public bool getDatabaseQueryStatus(Microsoft.Data.SqlClient.SqlDataReader reader, int rowsAffected = -1)
        // {
        //     throw new NotImplementedException();
        // }

        // // Find a transaction by ID
        // public Transaction Find(int transactionId)
        // {
        //     Console.WriteLine($"Finding transaction with ID: {transactionId}");
        //     return new Transaction(transactionId, DateTime.Now, "Mock Adjustment", 100, 1); // Returning mock data
        // }

        // // Insert a new transaction
        // public void Insert(Transaction transaction)
        // {
        //     Console.WriteLine($"Inserting Transaction ID: {transaction.TransactionId}, Type: {transaction.AdjustmentType}, Date: {transaction.DateTime}");
        // }

        // // Update an existing transaction
        // public void Update(Transaction transaction)
        // {
        //     Console.WriteLine($"Updating Transaction ID: {transaction.TransactionId} to Type: {transaction.AdjustmentType}");
        // }

        // // Delete a transaction by ID
        // public void Delete(int transactionId)
        // {
        //     Console.WriteLine($"Deleting transaction with ID: {transactionId}");
        // }

        // // Query transaction status (Mock implementation)
        // public string QueryStatus(int transactionId)
        // {
        //     Console.WriteLine($"Querying status for Transaction ID: {transactionId}");
        //     return "Mock Status"; // Returning mock data
        // }
    }

}
