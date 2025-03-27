using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;

/*
namespace CleanBrilliantCompany.Models.Control
{
    public class GoalsManagement
    {
        private readonly IGoalsDB _goalDb;

        public GoalsManagement(IGoalsDB goalsDb)
        {
            _goalDb = goalsDb;
        }

        public async Task AddGoal(int goalId, float targetEmission, int goalYear, int goalMonth)
        {
            var goal = new GoalsSDM(goalId, targetEmission, goalYear, goalMonth);
            await _goalDb.InsertGoal(goal);
        }

        public async Task UpdateGoal(int goalId, float targetEmission)
        {
            var existingGoal = await _goalDb.FindGoal(goalId);
            if (existingGoal != null)
            {
                existingGoal.UpdateTargetEmission(targetEmission);
                await _goalDb.UpdateGoal(existingGoal);
            }
        }

        public async Task<GoalsSDM> GetGoal(int goalId)
        {
            return await _goalDb.FindGoal(goalId);
        }
    }
}
*/