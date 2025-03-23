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
            bool result = _mapper.insertItemCF(itemId, productId, carbonEmission, ecoStatus, dateCreated);
            _lastQuerySuccess = result;
            return result;
        }

        public bool updateAllItemCF()
        {
            bool result = _mapper.updateAllItemCF();
            _lastQuerySuccess = result;
            return result;
        }

        public double getItemCarbonFootprint(int itemCFId)
        {
            try
            {
                double value = _mapper.retrieveItemCarbonFootprint(itemCFId);
                _lastQuerySuccess = true;
                return value;
            }
            catch
            {
                _lastQuerySuccess = false;
                return 0;
            }
        }

        public List<ItemCarbonFootprintRDM> getAllItemCarbonFootprint()
        {
            try
            {
                var list = _mapper.retrieveAllItemCarbonFootprint();
                _lastQuerySuccess = true;
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
                _lastQuerySuccess = true;
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
