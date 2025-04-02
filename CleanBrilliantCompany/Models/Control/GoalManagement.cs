using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Data;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Globalization;


namespace CleanBrilliantCompany.Models.Control
{
    public class GoalManagement : IGoals
    {
        private readonly IGoalsDB _goalDb;
        private readonly ILogger<GoalManagement> _logger;

        public GoalManagement(IGoalsDB goalDb, ILogger<GoalManagement> logger)
        {
            _goalDb = goalDb;
            _logger = logger;
        }

        public async Task<bool> CreateGoal(GoalsSDM goal, string goalDate)
        {
            if (DateTime.TryParseExact(goalDate, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                int goalYear = parsedDate.Year;
                int goalMonth = parsedDate.Month;

                // Check if a goal already exists for this year and month
                var existingGoal = await _goalDb.FindGoalByDate(goalYear, goalMonth);
                if (existingGoal != null)
                {
                    _logger.LogWarning($"A goal for {goalYear}-{goalMonth:D2} already exists.");
                    return false; // Prevent duplicate creation
                }

                // Update goal's date and insert
                goal.UpdateGoalDate(goalYear, goalMonth);
                await _goalDb.InsertGoal(goal);
                return true;
            }

            _logger.LogError("Invalid date format received.");
            return false;
        }

        public async Task<bool> ModifyGoal(string goalDate, double targetEmission)
        {
            if (DateTime.TryParseExact(goalDate, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                int goalYear = parsedDate.Year;
                int goalMonth = parsedDate.Month;

                var existingGoal = await _goalDb.FindGoalByDate(goalYear, goalMonth);

                if (existingGoal != null)
                {
                    existingGoal.UpdateTargetEmission(targetEmission);
                    await _goalDb.UpdateGoal(existingGoal);
                    return true;
                }
            }

            _logger.LogWarning($"Goal not found for date: {goalDate}");
            return false;
        }

        public async Task<bool> DeleteGoal(string goalDate)
        {
            if (DateTime.TryParseExact(goalDate, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
            {
                int goalYear = parsedDate.Year;
                int goalMonth = parsedDate.Month;

                var existingGoal = await _goalDb.FindGoalByDate(goalYear, goalMonth);

                if (existingGoal != null)
                {
                    await _goalDb.DeleteGoal(existingGoal.GetGoalId());
                    return true;
                }
            }

            _logger.LogWarning($"Goal not found for deletion: {goalDate}");
            return false;
        }

        public async Task<List<GoalsSDM>> GetAllGoals()
        {
            return await _goalDb.GetAllGoals(); // Ensure `IGoalsDB` has `GetAllGoals` method
        }

        public DateTime GetGoalDate(GoalsSDM goal)
        {
            return new DateTime(goal.GetGoalYear(), goal.GetGoalMonth() , 1); // Construct the DateTime from GoalYear and GoalMonth
        }

    }
}
