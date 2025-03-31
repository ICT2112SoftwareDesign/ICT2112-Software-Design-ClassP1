using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.DomainControl
{
    public class ItemCarbonFootprintControl : IItemCFManagement, IItemCF, IItemCFQuery
    {
        private readonly IItemCarbonFootprintDB _mapper;
        private bool _lastQuerySuccess;

        public ItemCarbonFootprintControl(IItemCarbonFootprintDB mapper)
        {
            _mapper = mapper;
        }

        public bool addItemCF(int itemId, int productId, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            try
            {
                _mapper.insertItemCF(itemId, productId, carbonEmission, ecoStatus, dateCreated);
                _lastQuerySuccess = _mapper.getQueryStatus();
            }
            catch
            {
                _lastQuerySuccess = false;
            }
            return _lastQuerySuccess;
        }

        public bool updateAllItemCF()
        {
            try
            {
                _mapper.updateAllItemCF();
                _lastQuerySuccess = _mapper.getQueryStatus();
            }
            catch
            {
                _lastQuerySuccess = false;
            }
            return _lastQuerySuccess;
        }

        public double getItemCarbonFootprint(int itemCFId)
        {
            try
            {
                double value = _mapper.retrieveItemCarbonFootprint(itemCFId);
                _lastQuerySuccess = _mapper.getQueryStatus();
                return value;
            }
            catch
            {
                _lastQuerySuccess = false;
                return 0;
            }
        }

        public double getItemCarbonFootprintByItemId(int itemId)
        {
            try
            {
                double value = _mapper.retrieveItemCarbonFootprintByItemId(itemId);
                _lastQuerySuccess = _mapper.getQueryStatus();
                return value;
            }
            catch
            {
                _lastQuerySuccess = false;
                return 0;
            }
        }

        public List<ItemCarbonFootprintRDM> getItemCarbonFootprintByProductId(int itemProductId)
        {
            try
            {
                var list = _mapper.retrieveItemCarbonFootprintByProductId(itemProductId);
                _lastQuerySuccess = _mapper.getQueryStatus();
                return list;
            }
            catch
            {
                _lastQuerySuccess = false;
                return new List<ItemCarbonFootprintRDM>();
            }
        }

        public List<ItemCarbonFootprintRDM> getAllItemCarbonFootprint()
        {
            try
            {
                var list = _mapper.retrieveAllItemCarbonFootprint();
                _lastQuerySuccess = _mapper.getQueryStatus();
                return list;
            }
            catch
            {
                _lastQuerySuccess = false;
                return new List<ItemCarbonFootprintRDM>();
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

        public float getTotalEcoFriendlyCarbonFootprint()
        {
            try
            {
                float total = _mapper.retrieveTotalEcoFriendlyCarbonFootprint();
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
