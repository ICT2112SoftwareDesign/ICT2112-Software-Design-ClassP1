using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class OrderCarbonFootprintControl : IOrderCF
    {
        private readonly IOrderCarbonFootprintDB _mapper;
        private bool _lastQuerySuccess;

        public OrderCarbonFootprintControl(IOrderCarbonFootprintDB mapper)
        {
            _mapper = mapper;
        }

        public bool addOrderCF(int orderId, string transportMode, double orderWeight, double distance, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            try
            {
                _mapper.insertOrderCF(orderId, transportMode, orderWeight, distance, carbonEmission, ecoStatus, dateCreated);
                _lastQuerySuccess = _mapper.getQueryStatus();
            }
            catch
            {
                _lastQuerySuccess = false;
            }

            return _lastQuerySuccess;
        }

        public double getOrderCarbonFootprint(int orderCFId)
        {
            try
            {
                double value = _mapper.retrieveOrderCarbonFootprint(orderCFId);
                _lastQuerySuccess = _mapper.getQueryStatus();
                return value;
            }
            catch
            {
                _lastQuerySuccess = false;
                return 0;
            }
        }

        public List<OrderCarbonFootprintRDM> getAllOrderCarbonFootprint()
        {
            try
            {
                var list = _mapper.retrieveAllOrderCarbonFootprint();
                _lastQuerySuccess = _mapper.getQueryStatus();
                return list;
            }
            catch
            {
                _lastQuerySuccess = false;
                return new List<OrderCarbonFootprintRDM>();
            }
        }

        public float getTotalCarbonFootprint()
        {
            try
            {
                float total = _mapper.retrieveTotalCarbonFootprint();
                _lastQuerySuccess = _mapper.getQueryStatus();
                return total;
            }
            catch
            {
                _lastQuerySuccess = false;
                return 0;
            }
        }

        public bool getDatabaseQueryStatus()
        {
            return _lastQuerySuccess;
        }
    }
}
