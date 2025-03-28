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
        private List<CarbonFootprintRecordRDM> orderEmission;
        private List<EmissionPredDTO> orderPredictEmission;
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
        public void retrieveOrderEmission(){
            orderEmission = new List<CarbonFootprintRecordRDM>();
            int i = 1;
            while(i < 14){
                CarbonFootprintRecordRDM dummy = new CarbonFootprintRecordRDM(
                    carbonFootprintId: i,
                    entityId: i,
                    entityType: "dummyType_" + i,
                    carbonEmission: 1.0 + i,
                    ecoStatus: "dummy-friendly",
                    dateCreated: DateOnly.FromDateTime(DateTime.Today.AddDays(-i))
                );
                orderEmission.Add(dummy);
                i++;
            }
            // orderEmission = _carbonFootprintService.getAllOrderCarbonFootprint();
        }
        public List<EmissionPredDTO> predictOrderEmission(List<DateOnly> days, List<float> data){
            if(days.Count > 10){
                setStrategy(new PredictionSDCA());
            }
            else{
                setStrategy(new PredictionSDCA());
            }
            return _strategy.retrievePrediction(days, data);
        }
        public void checkPredictedGoalThreshold(){

        }
        

        public async Task<List<GoalsSDM>> RetrieveGoalsForGraph()
        {
            return await _goalService.GetAllGoals();
        }
    }
}