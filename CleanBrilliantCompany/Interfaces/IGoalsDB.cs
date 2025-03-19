using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IGoalDB
    {
        Task InsertGoals(Goal goal);
        Task UpdateGoals(int goalId, float targetEmission, int goalYear, int goalMonth);
        Task DeleteGoals(int goalId);
        Task<Goal> FindGoals(int goalId);
    }
}