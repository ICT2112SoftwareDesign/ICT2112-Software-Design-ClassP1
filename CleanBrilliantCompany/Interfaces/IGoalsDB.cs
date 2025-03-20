using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IGoalsDB
    {
        Task<List<GoalsSDM>> GetAllGoals();
        Task InsertGoal(GoalsSDM goal);
        Task UpdateGoal(int goalId, float targetEmission, int goalYear, int goalMonth);
        Task DeleteGoal(int goalId);
        Task<GoalsSDM> FindGoals(int goalId);       
    }
}