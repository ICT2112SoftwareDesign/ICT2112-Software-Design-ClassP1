using System;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Models.Mapper
{
    public class TransactionMapper
    {
        public TransactionMapper()
        {
            Console.WriteLine("TransactionMapper initialized.");
        }

        // Find a transaction by ID
        public Transaction Find(int transactionId)
        {
            Console.WriteLine($"Finding transaction with ID: {transactionId}");
            return new Transaction(transactionId, DateTime.Now, "Mock Adjustment", 100, 1); // Returning mock data
        }

        // Insert a new transaction
        public void Insert(Transaction transaction)
        {
            Console.WriteLine($"Inserting Transaction ID: {transaction.TransactionId}, Type: {transaction.AdjustmentType}, Date: {transaction.DateTime}");
        }

        // Update an existing transaction
        public void Update(Transaction transaction)
        {
            Console.WriteLine($"Updating Transaction ID: {transaction.TransactionId} to Type: {transaction.AdjustmentType}");
        }

        // Delete a transaction by ID
        public void Delete(int transactionId)
        {
            Console.WriteLine($"Deleting transaction with ID: {transactionId}");
        }

        // Query transaction status (Mock implementation)
        public string QueryStatus(int transactionId)
        {
            Console.WriteLine($"Querying status for Transaction ID: {transactionId}");
            return "Mock Status"; // Returning mock data
        }
    }
}
