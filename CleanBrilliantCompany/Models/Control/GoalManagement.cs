using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;


namespace CleanBrilliantCompany.Models.Control
{
    public class GoalManagement
    {
        private List<Goal> goals;

        // Constructor to initialize the goal list
        public GoalManagement()
        {
            goals = new List<Goal>();
        }

        // Method to add a new goal
        public void AddGoal(int goalId, float targetEmission, int goalYear, int goalMonth)
        {
            Goal newGoal = new Goal(goalId, targetEmission, goalYear, goalMonth);
            goals.Add(newGoal);
        }

        // Method to retrieve target emission for a given year and month
        public float? RetrieveTargetEmission(int year, int month)
        {
            foreach (Goal goal in goals)
            {
                float? emission = goal.GetGoalFor(year, month);
                if (emission != null)
                {
                    return emission;
                }
            }
            return null;
        }
    }
}
