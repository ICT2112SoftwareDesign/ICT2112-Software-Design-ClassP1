using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CleanBrilliantCompany.Models.Entity
{
    [Table("CarbonGoals")]
    public class GoalsSDM
    {
        [Key]
        private int goalId { get; set; }
        private double targetEmission { get; set; }
        private int goalYear { get; set; }
        private int goalMonth { get; set; }

        public GoalsSDM() { } // EF requires parameterless constructor

        public GoalsSDM(int goalId, double targetEmission, int goalYear, int goalMonth)
        {
            this.goalId = goalId;
            this.targetEmission = targetEmission;
            this.goalYear = goalYear;
            this.goalMonth = goalMonth;
        }

        public int GetGoalId() => goalId;
        public double GetTargetEmission() => targetEmission;
        public int GetGoalYear() => goalYear;
        public int GetGoalMonth() => goalMonth;

        public void UpdateTargetEmission(double newEmission) => targetEmission = newEmission;
        public void UpdateGoalDate(int year, int month)
        {
            goalYear = year;
            goalMonth = month;
        }
    }
}