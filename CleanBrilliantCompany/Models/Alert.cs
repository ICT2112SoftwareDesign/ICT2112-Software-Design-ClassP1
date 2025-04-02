using System.ComponentModel.DataAnnotations;

namespace CleanBrilliantCompany.Models
{
    public class Alert
    {
        public int AlertId { get; set; }

        [Required] // represents the timestamp when the alert was generated
        public DateTime AlertTimestamp { get; set; } // renamed from alertdate

        [Required]
        public int GoalMonth { get; set; }

        [Required]
        public int GoalYear { get; set; }

        // target emission can be null if no goal was set for the month
        public decimal? TargetEmission { get; set; }

        [Required]
        public decimal ActualTotalEmission { get; set; }

        [Required]
        [StringLength(50)] // matches nvarchar(50) in sql
        public string Status { get; set; } = string.Empty; // e.g., "met", "missed", "no goal set"

        [Required]
        [StringLength(255)] // matches nvarchar(255) in sql
        public string Message { get; set; } = string.Empty; // renamed from alertmessage
    }
}
