using System.Collections.Generic;
using System.Linq;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.Control;
using Microsoft.EntityFrameworkCore;
using CleanBrilliantCompany.Data;

namespace CleanBrilliantCompany.Data
{
    public class GoalsGateway : IGoalsDB
    {
        private static List<GoalsSDM> goals = new List<GoalsSDM>();

        public List<GoalsSDM> GetAllGoals()
        {
            return goals;
        }

        public GoalsSDM GetGoalById(int id)
        {
            return goals.FirstOrDefault(g => g.GetGoalId() == id);
        }

        public void AddGoal(GoalsSDM goal)
        {
            goals.Add(goal);
        }

        public void UpdateGoal(GoalsSDM goal)
        {
            var existingGoal = GetGoalById(goal.GetGoalId());
            if (existingGoal != null)
            {
                existingGoal.UpdateTargetEmission(goal.GetTargetEmission());
            }
        }

        public void DeleteGoal(int id)
        {
            goals.RemoveAll(g => g.GetGoalId() == id);
        }
    }
}