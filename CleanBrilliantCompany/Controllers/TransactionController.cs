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
        [HttpGet("Index")] // Maps to /Transaction/Index
        public async Task<IActionResult> Index(DateTime? searchDate, string adjustmentType, int page = 1, int pageSize = 20)
        {
            // Get all transactions
            List<Transaction> allTransactions = _transactionControl.getAllTransactions();

            // Step 2: Count adjustment types from all records (before pagination)
            ViewData["TotalSoldCount"] = allTransactions.Count(t => t.retrieveTransactionInfo()["AdjustmentType"].ToString() == "Sold");
            ViewData["TotalReservedCount"] = allTransactions.Count(t => t.retrieveTransactionInfo()["AdjustmentType"].ToString() == "Reserved");
            ViewData["TotalTransferredCount"] = allTransactions.Count(t => t.retrieveTransactionInfo()["AdjustmentType"].ToString() == "Transferred");
            ViewData["TotalRefundedCount"] = allTransactions.Count(t => t.retrieveTransactionInfo()["AdjustmentType"].ToString() == "Refunded");


            // Filter by adjustment type if specified
            if (!string.IsNullOrEmpty(adjustmentType) && adjustmentType != "All")
            {

                allTransactions = allTransactions.Where(t => 
                {
                    var transactionInfo = t.retrieveTransactionInfo();
                    return transactionInfo["AdjustmentType"].ToString() == adjustmentType;}).ToList();
            }

            int totalFilteredCount = allTransactions.Count;
            int totalPages = (int)Math.Ceiling((double)totalFilteredCount / pageSize);

            List<Transaction> pagedTransactions = allTransactions
            .OrderByDescending(t => t.retrieveTransactionInfo()["TransactionDateTime"])  // <-- Sort by date descending
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            List<Transaction> transactions = pagedTransactions;
            List<Dictionary<string, object>> transactionsInfo = new List<Dictionary<string, object>>();

            foreach (var item in transactions)
            {
                transactionsInfo.Add(item.retrieveTransactionInfo());
            }

            ViewData["AllTransactions"] = transactionsInfo;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            int transactionsCount = _transactionControl.getTransactionCount();

            // Pass data to the view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;

            Console.WriteLine("TOTAL NUM OF ITEMS: " + transactionsCount);
            Console.WriteLine("TOTAL PAGE NUMBER: " + totalPages);
            Console.WriteLine("TOTAL PAGE SIZE: " + pageSize);

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

            // Adjustment Type Filter Options
            ViewData["AdjustmentTypes"] = new List<string> { "All", "Sold", "Reserved", "Transferred", "Refunded", "Returned" };


            return View();
        }
    }
}
