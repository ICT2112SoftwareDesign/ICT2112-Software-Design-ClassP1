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

        public async Task<ShippingAgent> GetShippingAgentByIdAsync(int id)
        {
            return await _context.ShippingAgents.FindAsync(id) ?? new ShippingAgent();
        }


        public async Task<bool> AddShippingAgentAsync(ShippingAgent shippingAgent)
        {
            try
            {
                _context.ShippingAgents.Add(shippingAgent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR adding shipping agent: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }


        public async Task<bool> UpdateShippingAgentAsync(ShippingAgent shippingAgent)
        {
            try
            {
                _context.ShippingAgents.Update(shippingAgent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR updating shipping agent: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> DeleteShippingAgentAsync(int id)
        {
            try
            {
                var shippingAgent = await _context.ShippingAgents.FindAsync(id);
                if (shippingAgent == null)
                {
                    return false;
                }

                _context.ShippingAgents.Remove(shippingAgent);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR deleting shipping agent: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }
    }
}