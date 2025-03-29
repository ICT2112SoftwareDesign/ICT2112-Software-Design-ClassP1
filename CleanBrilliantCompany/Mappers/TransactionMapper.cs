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

        //FOR ITEMID --> INCLUDE A SORT BY ITEMID --> ALL TRANSACTIONS WILL BE GROUPED BY ITEMID AND SORTED IN ASC/DESC ORDER

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
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string insertQuery = @"
                INSERT INTO dbo.ItemTransaction (transactionDateTime, adjustmentType, productId, itemId, staffId)
                VALUES (@transactionDateTime, @adjustmentType, @productId, @itemid, 1);";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@transactionDateTime", dateTime);
                    command.Parameters.AddWithValue("@adjustmentType", adjustmentType);
                    command.Parameters.AddWithValue("@productId", productId);
                    command.Parameters.AddWithValue("itemId", itemId);

                    int rowsAffected = command.ExecuteNonQuery(); // Get the number of rows affected
                    
                    if (rowsAffected > 0 ) {
                        Console.WriteLine("Transaction successfully added into the database.");
                    }

                }
            }
        }

        //Add another one for getting transaction by productName after if have time

        public List<Transaction> getTransactionByDateTime(DateTime dateTime)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                
                string query = "SELECT transactionId, transactionDateTime, adjustmentType, productId, itemId, staffId " +
               "FROM ItemTransaction WHERE CAST(transactionDateTime AS DATE) = @transactionDateTime";

                using (SqlCommand command = new SqlCommand(query,connection))
                {
                    command.Parameters.AddWithValue("@transactionDateTime", dateTime);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                 Transaction transaction = new Transaction(
                                    reader.GetInt32(reader.GetOrdinal("transactionId")),
                                    reader.GetDateTime(reader.GetOrdinal("transactionDateTime")),
                                    reader.GetString(reader.GetOrdinal("adjustmentType")),
                                    reader.GetInt32(reader.GetOrdinal("productId")),
                                    reader.GetInt32(reader.GetOrdinal("itemId")),
                                    reader.GetInt32(reader.GetOrdinal("staffId"))
                                );         

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

        public List<Transaction> getAllTransactions()
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT transactionId, transactionDateTime, adjustmentType, ItemTransaction.productId, Product.productName, itemId, staffId FROM ItemTransaction"
                + " INNER JOIN Product ON ItemTransaction.productId = Product.productId";

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
                                    reader.GetString(reader.GetOrdinal("productName")),
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

        public List<Transaction> getAllTransactions(int pageNumber, int pageSize)
        {
            List<Transaction> transactions = new List<Transaction>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                int offset = (pageNumber - 1) * pageSize;

                string query = @"SELECT transactionId, transactionDateTime, adjustmentType, ItemTransaction.productId, Product.productName, itemId, staffId FROM ItemTransaction"
                + " INNER JOIN Product ON ItemTransaction.productId = Product.productId ORDER BY transactionID desc OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Offset", offset);
                    command.Parameters.AddWithValue("PageSize", pageSize);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (getDatabaseQueryStatus(reader))
                        {
                            while (reader.Read())
                            {
                                // Create the Transaction object using the constructor
                                Transaction transaction = new Transaction(
                                    reader.GetInt32(reader.GetOrdinal("transactionId")),
                                    reader.GetDateTime(reader.GetOrdinal("transactionDateTime")),
                                    reader.GetString(reader.GetOrdinal("adjustmentType")),
                                    reader.GetString(reader.GetOrdinal("productName")),
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

        public int getTransactionCount()
        {
            int count = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                String query = "SELECT COUNT(*) FROM ItemTransaction ";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        count = Convert.ToInt32(result);
                    }
                    else
                    {
                        Console.WriteLine("No data found for the query.");
                    }
                }
            }

            return count;
        }

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
