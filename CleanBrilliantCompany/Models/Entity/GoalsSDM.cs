namespace CleanBrilliantCompany.Models.Entity

{
    public class Goal
    {
        private int goalId;
        private float targetEmission;
        private int goalYear;
        private int goalMonth;

        // Getters and Setters (Private)
        private int GetGoalId()
        {
            return goalId;
        }

        private void SetGoalId(int id)
        {
            goalId = id;
        }

        private float GetTargetEmission()
        {
            return targetEmission;
        }

        private void SetTargetEmission(float emission)
        {
            targetEmission = emission;
        }

        private int GetGoalYear()
        {
            return goalYear;
        }

        private void SetGoalYear(int year)
        {
            goalYear = year;
        }

        private int GetGoalMonth()
        {
            return goalMonth;
        }

        private void SetGoalMonth(int month)
        {
            goalMonth = month;
        }

        // Constructor
        public Goal(int goalId, float targetEmission, int goalYear, int goalMonth)
        {
            this.goalId = goalId;
            this.targetEmission = targetEmission;
            this.goalYear = goalYear;
            this.goalMonth = goalMonth;
        }

        // Public Methods to access entity
        public void UpdateTargetEmission(float newEmission)
        {
            SetTargetEmission(newEmission);
        }


    }
}