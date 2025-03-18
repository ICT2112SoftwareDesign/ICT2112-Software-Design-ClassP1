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
        private readonly TransactionControl _transactionControl;

        public TransactionController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _transactionControl = new TransactionControl(connectionString);
        }

        public async Task<IActionResult> Index()
        {
            List<Transaction> transactions = _transactionControl.getAllTransactions();

            List<Dictionary<string, object>> transactionsInfo = new List<Dictionary<string, object>>();

            foreach (var item in transactions)
            {
                transactionsInfo.Add(item.retrieveTransactionInfo());
            }


            return View(transactionsInfo);

        }
    }
}
