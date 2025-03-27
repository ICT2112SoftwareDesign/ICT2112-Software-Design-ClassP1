using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IGoalsDB
    {
        Task InsertGoal(GoalsSDM goal);
        Task UpdateGoal(GoalsSDM goal);
        Task DeleteGoal(int goalId);
        Task<GoalsSDM> FindGoal(int goalId);
        Task<List<GoalsSDM>> GetAllGoals();
        Task<GoalsSDM> FindGoalByDate(int goalYear, int goalMonth);
    }
}