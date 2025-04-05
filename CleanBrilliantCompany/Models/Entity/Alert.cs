using System.ComponentModel.DataAnnotations;

namespace CleanBrilliantCompany.Models
{
    public class Alert
    {
        private int _alertId;
        private DateTime _alertTimestamp;
        private int _goalMonth;
        private int _goalYear;
        private decimal? _targetEmission;
        private decimal _actualTotalEmission;
        private string _status = string.Empty;
        private string _message = string.Empty;

        public int AlertId 
        { 
            get { return _alertId; } 
            set { _alertId = value; } 
        }

        [Required] // represents the timestamp when the alert was generated
        public DateTime AlertTimestamp 
        { 
            get { return _alertTimestamp; } 
            set { _alertTimestamp = value; } 
        } // renamed from alertdate

        [Required]
        public int GoalMonth 
        { 
            get { return _goalMonth; } 
            set { _goalMonth = value; } 
        }

        [Required]
        public int GoalYear 
        { 
            get { return _goalYear; } 
            set { _goalYear = value; } 
        }

        // target emission can be null if no goal was set for the month
        public decimal? TargetEmission 
        { 
            get { return _targetEmission; } 
            set { _targetEmission = value; } 
        }

        [Required]
        public decimal ActualTotalEmission 
        { 
            get { return _actualTotalEmission; } 
            set { _actualTotalEmission = value; } 
        }

        [Required]
        [StringLength(50)] // matches nvarchar(50) in sql
        public string Status 
        { 
            get { return _status; } 
            set { _status = value; } 
        } // e.g., "met", "missed", "no goal set"

        [Required]
        [StringLength(255)] // matches nvarchar(255) in sql
        public string Message 
        { 
            get { return _message; } 
            set { _message = value; } 
        } // renamed from alertmessage
    }
}
