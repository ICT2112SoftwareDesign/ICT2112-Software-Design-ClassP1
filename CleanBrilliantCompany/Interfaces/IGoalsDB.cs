using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IGoalsDB
    {
        List<GoalsSDM> GetAllGoals();
        GoalsSDM GetGoalById(int id);
        void AddGoal(GoalsSDM goal);
        void UpdateGoal(GoalsSDM goal);
        void DeleteGoal(int id);
    }
}