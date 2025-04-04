using System.ComponentModel.DataAnnotations;

namespace CleanBrilliantCompany.DTO
{
    public class ForecastDashboardDTO
    {
        [Key]

        private int _dashBoardID;
        private DateTime _startDate;
        private DateTime _endDate;
        private DateTime _generatedDateTime;

        public int DashBoardID
        {
            get => _dashBoardID;
            set => _dashBoardID = value;
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => _startDate = value;
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => _endDate = value;
        }

        public DateTime GeneratedDateTime
        {
            get => _generatedDateTime;
            set => _generatedDateTime = value;
        }

      

        public ForecastDashboardDTO(int dashBoardID, DateTime startDate, DateTime endDate, DateTime generatedDate)
        {
            _dashBoardID = dashBoardID;
            _startDate = startDate;
            _endDate = endDate;
            _generatedDateTime = generatedDate;
        }

        public ForecastDashboardDTO() { } // Required for deserialization or EF materialization
    }
}
