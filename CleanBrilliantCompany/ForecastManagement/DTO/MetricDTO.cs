using System.ComponentModel.DataAnnotations;

namespace CleanBrilliantCompany.DTO
{
    public class MetricDTO
    {
        [Key]
        private int _metricsID;
        private int _dashboardID;
        private int _productID;
        private int _forecastedStock;

        public int DashboardID
        {
            get => _dashboardID;
            set => _dashboardID = value;
        }
        public int MetricsID
        {
            get => _metricsID;
            set => _metricsID = value;
        }

        public int ProductID
        {
            get => _productID;
            set => _productID = value;
        }

        public int ForecastedStock
        {
            get => _forecastedStock;
            set => _forecastedStock = value;
        }

        public MetricDTO() { } // Required for deserialization or EF materialization

        public MetricDTO(int dashboardID,int metricsID, int productID, int forecastedStock)
        {
            _dashboardID = dashboardID;
            _metricsID = metricsID;
            _productID = productID;
            _forecastedStock = forecastedStock;
        }
    }
}
