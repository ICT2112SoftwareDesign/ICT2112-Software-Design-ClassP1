using System;
using CleanBrilliantCompany.Interfaces;
//using CleanBrilliantCompany.Mappers;

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
            throw new NotImplementedException();
        }
        public List<CarbonFootprintRecordRDM> getAllOrderCarbonFootprint()
        {
            throw new NotImplementedException();
        }
        
        public float getAllCarbonFootprint()
        {
            throw new NotImplementedException();
        }
        public List<CarbonFootprintRecordRDM> getCarbonFootprintComparison(List<int> entityId, string entityType)
        {
            throw new NotImplementedException();
        }
        public List<CarbonFootprintRecordRDM> getEcoFriendlyReport(DateTime startDate, DateTime endDate, string entityType)
        {
            throw new NotImplementedException();
        }
    }
}