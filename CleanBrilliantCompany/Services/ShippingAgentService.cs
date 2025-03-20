using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Services;
using CleanBrilliantCompany.Data;
using Microsoft.EntityFrameworkCore; // Required for ToListAsync()


namespace CleanBrilliantCompany.Services
{
    public class ShippingAgentService : IShippingAgentService
    {
        private readonly ApplicationDbContext _context;

        public ShippingAgentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShippingAgent>> GetShippingAgentsAsync()
        {
            return await _context.ShippingAgents.ToListAsync(); // Fetch from DB
        }
    }
}

