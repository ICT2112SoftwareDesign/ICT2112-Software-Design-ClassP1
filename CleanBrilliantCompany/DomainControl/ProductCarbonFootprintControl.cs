using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.DomainControl
{
    public class ProductCarbonFootprintControl : IProductCFManagement, IProductCF, IProductCFQuery
    {
        private readonly IProductCarbonFootprintDB _mapper;
        private bool _lastQuerySuccess;

        public ProductCarbonFootprintControl(IProductCarbonFootprintDB mapper)
        {
            _mapper = mapper;
        }

        public bool addProductCF(int productId, string productName, string productCategory, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            try
            {
                _mapper.insertProductCF(productId, productName, productCategory, carbonEmission, ecoStatus, dateCreated);
                _lastQuerySuccess = true;
            }
            catch
            {
                _lastQuerySuccess = false;
            }

            return _lastQuerySuccess;
        }

        public double getProductCarbonFootprint(int productCFId)
        {
            try
            {
                double productCF = 0;
                productCF = _mapper.retrieveProductCarbonFootprint(productCFId);
                _lastQuerySuccess = true;
                return productCF;
            }
            catch
            {
                _lastQuerySuccess = false;
                return 0;
            }
        }

        public List<ProductCarbonFootprintRDM> getAllProductCarbonFootprint()
        {
            try
            {
                List<ProductCarbonFootprintRDM> list = _mapper.retrieveAllProductCarbonFootprint();
                _lastQuerySuccess = true;
                return list;
            }
            catch
            {
                _lastQuerySuccess = false;
                return new List<ProductCarbonFootprintRDM>();
            }
        }

        public float getTotalCarbonFootprint()
        {
            try
            {
                float total = 0;
                total = _mapper.retrieveTotalCarbonFootprint();
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
