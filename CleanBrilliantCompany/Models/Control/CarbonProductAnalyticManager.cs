using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DTO;


namespace CleanBrilliantCompany.Models.Control
{
    
    public class CarbonProductAnalyticManager
    {
        private List<ProductCarbonFootprintRDM> productEmission;
        
        private readonly IProductCF _IProductCFService;


        //Constructor
        public CarbonProductAnalyticManager(IProductCF IProductCFService)
        {
            _IProductCFService = IProductCFService;
        }
        public async Task retrieveProductEmission(){
            productEmission = _IProductCFService.getAllProductCarbonFootprint();
        }
        public async Task<double> retreiveToxicity(int Id){
            return _IProductCFService.getProductCarbonFootprint(Id);
        }
        public async Task<String> retrieveEcoStatus(int Id){
            foreach (ProductCarbonFootprintRDM item in productEmission)
            {
                if(item.retrieveProductId() == Id){
                    return item.retrieveEcoStatus();
                }
            } 
            return "";
        }
    }
}