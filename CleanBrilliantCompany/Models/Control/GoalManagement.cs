using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;


namespace CleanBrilliantCompany.Models.Control
{
    public class GoalManagementController
    {
        private readonly IGoalsDB _goalDb;

        public GoalManagementController(IGoalsDB goalDb)
        {
            _goalDb = goalDb;
        }

        public async Task CreateGoal(int id, float target, int year, int month)
        {
            GoalsSDM newGoal = new GoalsSDM(id, target, year, month);
            await _goalDb.InsertGoal(newGoal);
        }

        public async Task UpdateGoal(int id, float newTarget, int year, int month)
        {
            await _goalDb.UpdateGoal(id, newTarget, year, month);
        }

        public async Task DeleteGoal(int id)
        {
            await _goalDb.DeleteGoal(id);
        }
    }

}