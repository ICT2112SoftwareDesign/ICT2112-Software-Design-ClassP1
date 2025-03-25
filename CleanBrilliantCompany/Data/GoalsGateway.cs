using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Data
{
    public class GoalsGateway : IGoalsDB, IGoalsQuery
    {
        private readonly ApplicationDbContext _context;

        public GoalsGateway(ApplicationDbContext context)
        {
            _context = context;
        }

        // Insert Goal
        public async Task InsertGoal(GoalsSDM goal)
        {
            _context.Goals.Add(goal);
            await _context.SaveChangesAsync();
        }

        // Update Goal
        public async Task UpdateGoal(GoalsSDM goal)
        {
            _context.Goals.Update(goal);
            await _context.SaveChangesAsync();
        }

        // Delete Goal
        public async Task DeleteGoal(int goalId)
        {
            var goal = await _context.Goals.FindAsync(goalId);
            if (goal != null)
            {
                _context.Goals.Remove(goal);
                await _context.SaveChangesAsync();
            }
        }

        // Find Goal by ID
        public async Task<GoalsSDM> FindGoal(int goalId)
        {
            return await _context.Goals.FindAsync(goalId);
        }

        // Get All Goals
        public async Task<List<GoalsSDM>> GetAllGoals()
        {
            return await _context.Goals.ToListAsync();
        }

        // Check if Goal Exists for Year & Month
        public async Task<bool> CheckGoalsQuery(int goalYear, int goalMonth)
        {
            return await _context.Goals.AnyAsync(g => g.GetGoalYear() == goalYear && g.GetGoalMonth() == goalMonth);
        }
    }
}

