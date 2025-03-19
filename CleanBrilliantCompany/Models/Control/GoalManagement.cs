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

        public void CreateGoal(int id, float target, int year, int month)
        {
            GoalsSDM newGoal = new GoalsSDM(id, target, year, month);
            _goalDb.AddGoal(newGoal);
        }

        public void UpdateGoal(int id, float newTarget)
        {
            GoalsSDM goal = _goalDb.GetGoalById(id);
            if (goal != null)
            {
                goal.UpdateTargetEmission(newTarget);
                _goalDb.UpdateGoal(goal);
            }
        }
    }

}