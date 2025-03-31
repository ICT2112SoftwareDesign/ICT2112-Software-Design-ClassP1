using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DTO;


namespace CleanBrilliantCompany.Models.Control
{
    
    public class CarbonOrderAnalyticManager
    {
        private List<CarbonFootprintRecordRDM> itemEmission;
        private List<CarbonFootprintRecordRDM> orderEmission;
        private List<EmissionPredDTO> predictEmission;
        private IPredictionStrategy _strategy;
        private readonly ICarbonFootprint _carbonFootprintService;
        private readonly IGoals _goalService;

        //Constructor
        public CarbonOrderAnalyticManager(IGoals goalService)
        {
            _goalService = goalService;
        }

        private void setStrategy(IPredictionStrategy strategy){
            _strategy = strategy;
        }
        public List<CarbonFootprintRecordRDM> getOrderEmission(){
            return orderEmission;
        }
        public List<CarbonFootprintRecordRDM> getItemEmission(){
            return itemEmission;
        }
        public async Task retrieveItemEmission(){
            itemEmission = new List<CarbonFootprintRecordRDM>();
            int i = 1;
            while(i < 4){
                CarbonFootprintRecordRDM dummy = new CarbonFootprintRecordRDM(
                    carbonFootprintId: i,
                    entityId: i,
                    entityType: "dummyType_" + i,
                    carbonEmission: 20.0 + i,
                    ecoStatus: "dummy-friendly",
                    dateCreated: DateTime.Today.AddMonths(-i)
                );
                itemEmission.Add(dummy);
                i++;
            }
        }
        public async Task retrieveOrderEmission(){
            orderEmission = new List<CarbonFootprintRecordRDM>();
            int i = 1;
            while(i < 5){
                CarbonFootprintRecordRDM dummy = new CarbonFootprintRecordRDM(
                    carbonFootprintId: i,
                    entityId: i,
                    entityType: "dummyType_" + i,
                    carbonEmission: 123.0 + i,
                    ecoStatus: "dummy-friendly",
                    dateCreated: DateTime.Today.AddMonths(-i)
                );
                orderEmission.Add(dummy);
                i++;
            }
            // orderEmission = _carbonFootprintService.getAllOrderCarbonFootprint();
        }
        public async Task predictCarbonEmission(){
            List<CarbonFootprintRecordRDM> orderList = getOrderEmission();
            List<CarbonFootprintRecordRDM> itemList = getItemEmission();

            // Cut off to get up to 3 years of data for yearly prediction
            DateTime cutoffYear = DateTime.Today.AddYears(-3);


            Dictionary<DateTime, double> emissionsMap = new Dictionary<DateTime, double>();

            // Aggregate emissions from both lists
            AggregateEmissions(orderList, emissionsMap);
            AggregateEmissions(itemList, emissionsMap);


            // Filter out emissions before the 3-year cutoff for yearly prediction
            var filteredYearlyEmissions = emissionsMap.Where(e => e.Key >= cutoffYear).ToDictionary(e => e.Key, e => e.Value);
            

            List<DateTime> daysYear = filteredYearlyEmissions.Keys.OrderBy(d => d).ToList();
            List<double> dataYear = daysYear.Select(d => filteredYearlyEmissions[d]).ToList();

            if(daysYear.Count > 10){
                setStrategy(new PredictionSSA());
            }
            else{
                setStrategy(new PredictionSMA());
            }
            List<EmissionPredDTO> resultYear = _strategy.retrievePrediction(daysYear, dataYear);

            setPredictEmission(resultYear);
        }
        public async Task<List<EmissionPredDTO>> getGraphEmission(){
            List<CarbonFootprintRecordRDM> orderList = getOrderEmission();
            List<CarbonFootprintRecordRDM> itemList = getItemEmission();

            Dictionary<DateTime, double> emissionsByDate = new Dictionary<DateTime, double>();

            // Aggregate emissions from both lists
            AggregateEmissions(orderList, emissionsByDate);
            AggregateEmissions(itemList, emissionsByDate);
    
            // Group by Month and Sum Emissions
            List<EmissionPredDTO> graphEmission = emissionsByDate
            .GroupBy(e => new DateTime(e.Key.Year, e.Key.Month, 1)) // Group by Year-Month
            .Select(g => new EmissionPredDTO
            {
                day = g.Key,
                emisission = g.Sum(e => e.Value)
            })
            .ToList();

            return graphEmission;
        }
        private void AggregateEmissions(List<CarbonFootprintRecordRDM> records, Dictionary<DateTime, double> emissionsByDate)
        {
            if (records != null)
            {
                foreach (var record in records)
                {
                    DateTime date = record.getDateCreatedForInsert().Date; // Extract date part only
                    if (emissionsByDate.ContainsKey(date))
                    {
                        emissionsByDate[date] += record.getCarbonEmissionForCalculation();
                    }
                    else
                    {
                        emissionsByDate[date] = record.getCarbonEmissionForCalculation();
                    }
                }
            }
        }
        public List<EmissionPredDTO> getPredictEmission(){
            return predictEmission;
        }
        private void setPredictEmission(List<EmissionPredDTO> predictEmission){
            this.predictEmission = predictEmission;
        }
        public void checkPredictedGoalThreshold(){

        }
        

        public async Task<List<GoalsSDM>> RetrieveGoalsForGraph(DateTime? startDate, DateTime? endDate)
        {
            var goals = await _goalService.GetAllGoals();

            // Apply date filtering using the GetGoalDate method from IGoals
            if (startDate.HasValue)
            {
                goals = goals.Where(g => _goalService.GetGoalDate(g) >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                goals = goals.Where(g => _goalService.GetGoalDate(g) <= endDate.Value).ToList();
            }

            return goals;
        }
    }
}