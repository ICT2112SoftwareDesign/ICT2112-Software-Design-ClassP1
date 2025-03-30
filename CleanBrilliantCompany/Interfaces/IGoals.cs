using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Models.Entity;

public interface IGoals
{
    Task<List<GoalsSDM>> GetAllGoals();
    DateTime GetGoalDate(GoalsSDM goal);
}