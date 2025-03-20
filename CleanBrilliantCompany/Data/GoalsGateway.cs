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
    public class GoalsGateway : IGoalsDB
    {
        private static List<GoalsSDM> goals = new List<GoalsSDM>(); // Store GoalsSDM objects

        public async Task<List<GoalsSDM>> GetAllGoals()
        {
            return await Task.FromResult(goals);
        }

        public async Task InsertGoal(GoalsSDM goal)
        {
            goals.Add(goal);
            await Task.CompletedTask;
        }

        public async Task UpdateGoal(int goalId, float targetEmission, int goalYear, int goalMonth)
        {
            var goal = goals.FirstOrDefault(g => g.GetGoalId() == goalId);
            if (goal != null)
            {
                goal.UpdateTargetEmission(targetEmission);
                goal.UpdateGoalDate(goalYear, goalMonth);
            }
            await Task.CompletedTask;
        }

        public async Task DeleteGoal(int goalId)
        {
            goals.RemoveAll(g => g.GetGoalId() == goalId);
            await Task.CompletedTask;
        }

        public async Task<GoalsSDM> FindGoals(int goalId)
        {
            var goal = goals.FirstOrDefault(g => g.GetGoalId() == goalId);
            return await Task.FromResult(goal);
        }
    }
}