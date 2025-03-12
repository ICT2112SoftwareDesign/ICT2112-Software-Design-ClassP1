using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using System;
using System.Collections.Generic;
using CleanBrilliantCompany.Models.Mapper;

namespace CleanBrilliantCompany.Controllers
{
    public class TransactionController : Controller
    {
        
        private readonly TransactionMapper _transactionMapper;

        public TransactionController()
        {
            _transactionMapper = new TransactionMapper();
        }

        public IActionResult Index()
        {
            TransactionControl transactionControl = new TransactionControl();

            // Sample Data
            transactionControl.AddTransaction(new Transaction(1, new DateTime(2025, 3, 8, 14, 30, 0), "Stock In", 101, 1));
            transactionControl.AddTransaction(new Transaction(2, new DateTime(2025, 3, 8, 18, 15, 0), "Stock Out", 102,1));
            transactionControl.AddTransaction(new Transaction(3, new DateTime(2025, 3, 8, 10, 0, 0), "Stock In", 103,1));

            DateTime searchDate = new DateTime(2025, 3, 8);
            List<Transaction> transactions = transactionControl.getTransactionsByDateTime(searchDate);

            // // Insert a transaction for testing
            // _transactionMapper.Insert(new Transaction(1, DateTime.Now, "Stock In", 101, 1));

            // // Fetch the inserted transaction
            // Transaction foundTransaction = _transactionMapper.Find(1);

            // ViewBag.Transactions = transactions;  // List of transactions
            // ViewBag.FoundTransaction = foundTransaction;  // Single transaction

            // Return results to the view
            return View(transactions);

        }
    }
}
