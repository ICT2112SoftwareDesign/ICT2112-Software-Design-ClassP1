using System;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Mappers;

namespace CleanBrilliantCompany.Models
{
    public class CarbonFootprintCalculatorControl
    {
        private readonly ICarbonFootprint _carbonFootprint;

        public CarbonFootprintCalculatorControl(ICarbonFootprint carbonFootprint)
        {
            _carbonFootprint = carbonFootprint;
        }
        public List<CarbonFootprintRecordRDM> getAllProductCarbonFootprint()
        {
            //impl
        }
        public List<CarbonFootprintRecordRDM> getAllOrderCarbonFootprint()
        {
            //impl
        }
        
        public float getAllCarbonFootprint()
        {
            //impl
        }
        public List<CarbonFootprintRecordRDM> getCarbonFootprintComparison(List<int> entityId, string entityType)
        {
            //impl
        }
        public List<CarbonFootprintRecordRDM> getEcoFriendlyReport(DateTime startDate, DateTime endDate, string entityType)
        {
            //impl
        }
    }
}