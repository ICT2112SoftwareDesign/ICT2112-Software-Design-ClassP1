using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CleanBrilliantCompany.Domain
{
    public class CarbonFootprintCalculatorControl : ICarbonCalculatorQuery
    {
        private readonly ICarbonRepositoryQuery _repository;

        public CarbonFootprintCalculatorControl(ICarbonRepositoryQuery repository)
        {
            _repository = repository;
        }

        public bool getDatabaseQueryStatus()
        {
            return _repository.getQueryStatus();
        }

        public List<CarbonFootprintRecordRDM> getAllProductCarbonFootprint()
        {
            return _repository.retrieveAllProductCarbonFootprint();
        }

        public List<CarbonFootprintRecordRDM> getAllOrderCarbonFootprint()
        {
            return _repository.retrieveAllOrderCarbonFootprint();
        }

        // Retrieve and sum total carbon footprint (products + orders)
        public float getAllCarbonFootprint()
        {
            var products = _repository.retrieveAllProductCarbonFootprint();
            var orders = _repository.retrieveAllOrderCarbonFootprint();

            // Sum all emissions from both lists
            float totalProductEmission = products.Sum(r => (float)r.getCarbonEmissionForCalculation());
            float totalOrderEmission = orders.Sum(r => (float)r.getCarbonEmissionForCalculation());

            return totalProductEmission + totalOrderEmission;
        }

        // Compare carbon footprints for given list of entityIds (products or orders)
        public List<CarbonFootprintRecordRDM> getCarbonFootprintComparison(List<int> entityIds, string entityType)
        {
            var comparisonList = new List<CarbonFootprintRecordRDM>();

            foreach (var id in entityIds)
            {
                float emission = (entityType == "Product")
                    ? _repository.retrieveProductCarbonFootprint(id, entityType)
                    : _repository.retrieveOrderCarbonFootprint(id, entityType);

                var record = new CarbonFootprintRecordRDM(
                    0, // placeholder
                    id,
                    entityType,
                    emission,
                    "Unknown", // eco status unknown in comparison
                    DateOnly.FromDateTime(DateTime.Now) // use current date as placeholder
                );

                comparisonList.Add(record);
            }

            return comparisonList;
        }

        public List<CarbonFootprintRecordRDM> getEcoFriendlyReport(DateTime startDate, DateTime endDate, string entityType)
        {
            List<CarbonFootprintRecordRDM> allRecords;

            if (entityType == "Product")
                allRecords = _repository.retrieveAllProductCarbonFootprint();
            else if (entityType == "Order")
                allRecords = _repository.retrieveAllOrderCarbonFootprint();
            else
                return new List<CarbonFootprintRecordRDM>();

            return allRecords.Where(r =>
                r.isEcoFriendly() &&
                r.isDateWithinRange(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate))
            ).ToList();
        }
    }
}
