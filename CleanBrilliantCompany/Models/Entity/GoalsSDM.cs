namespace CleanBrilliantCompany.Models.Entity

{
    public class GoalsSDM
    {
        private int goalId;
        private float targetEmission;
        private int goalYear;
        private int goalMonth;

        // Getters and Setters
        public int GetGoalId()
        {
            return goalId;
        }

        public void SetGoalId(int id)
        {
            goalId = id;
        }

        public float GetTargetEmission()
        {
            return targetEmission;
        }

        public void SetTargetEmission(float emission)
        {
            targetEmission = emission;
        }

        public int GetGoalYear()
        {
            return goalYear;
        }

        public void SetGoalYear(int year)
        {
            goalYear = year;
        }

        public int GetGoalMonth()
        {
            return goalMonth;
        }

        public void SetGoalMonth(int month)
        {
            goalMonth = month;
        }

        // Constructor
        public GoalsSDM(int goalId, float targetEmission, int goalYear, int goalMonth)
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