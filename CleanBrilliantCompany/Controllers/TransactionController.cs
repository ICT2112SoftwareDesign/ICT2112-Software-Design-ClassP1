using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Controllers
{
    [Route("Transaction")]  // Base route for the controller
    public class TransactionController : Controller
    {
        private readonly TransactionControl _transactionControl;
        
        // Constructor
        public TransactionController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _transactionControl = new TransactionControl(connectionString);
        }

        // Route for the Index action with optional searchDate parameter
        [Route("Index")] // Maps to /Transaction/Index
        public async Task<IActionResult> Index(DateTime? searchDate)
        {
            // Get all transactions
            List<Transaction> transactions = _transactionControl.getAllTransactions();
            List<Dictionary<string, object>> transactionsInfo = new List<Dictionary<string, object>>();

            foreach (var item in transactions)
            {
                transactionsInfo.Add(item.retrieveTransactionInfo());
            }

            // Get transactions for a specific date if searchDate is provided
            List<Transaction> transactionsByDate = new List<Transaction>();
            if (searchDate.HasValue)
            {
                transactionsByDate = _transactionControl.getTransactionByDateTime(searchDate.Value);
                List<Dictionary<string, object>> transactionsByDateInfo = new List<Dictionary<string, object>>();
                foreach (var item in transactionsByDate)
                {
                    transactionsByDateInfo.Add(item.retrieveTransactionInfo());
                }

                // Pass both tables to the view
                ViewData["TransactionsByDate"] = transactionsByDateInfo;
                ViewData["SearchDate"] = searchDate.Value.ToString("yyyy-MM-dd");  // Display the search date
            }

            // Pass all transactions to the view
            ViewData["AllTransactions"] = transactionsInfo;

            return View();
        }
    }
}
