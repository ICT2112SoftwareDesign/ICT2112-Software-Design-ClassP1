namespace CleanBrilliantCompany.Models.Entity

{
    public class GoalsSDM
    {
        private int goalId { get; set; }
        private float targetEmission { get; set; }
        private int goalYear { get; set; }
        private int goalMonth { get; set; }

        // Constructor
        public GoalsSDM(int goalId, float targetEmission, int goalYear, int goalMonth)
        {
            this.goalId = goalId;
            this.targetEmission = targetEmission;
            this.goalYear = goalYear;
            this.goalMonth = goalMonth;
        }

        // Getters
        public int GetGoalId() => goalId;
        public float GetTargetEmission() => targetEmission;
        public int GetGoalYear() => goalYear;
        public int GetGoalMonth() => goalMonth;

        // Public methods to access private fields
        public void UpdateTargetEmission(float newEmission)
        {
            targetEmission = newEmission;
        }

        public void UpdateGoalDate(int year, int month)
        {
            goalYear = year;
            goalMonth = month;
        }


    }
}