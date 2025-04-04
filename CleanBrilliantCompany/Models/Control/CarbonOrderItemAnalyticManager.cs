using System.Collections.Generic;
using System.Threading.Tasks;
using CleanBrilliantCompany.Data;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.DTO;


namespace CleanBrilliantCompany.Models.Control
{
    
    public class CarbonOrderItemAnalyticManager : ICarbonNotification
    {
        private List<ItemCarbonFootprintRDM> itemEmission;
        private List<OrderCarbonFootprintRDM> orderEmission;
        private List<EmissionPredDTO> predictEmission;
        private IPredictionStrategy _strategy;
        private readonly IOrderCF _IOrderCFService;
        private readonly IItemCF _IItemCFService;
        private readonly IGoals _goalService;

        //Constructor
        public CarbonOrderItemAnalyticManager(IGoals goalService, IItemCF IItemCFService, IOrderCF IOrderCFService)
        {
            _goalService = goalService;
            _IItemCFService = IItemCFService;
            _IOrderCFService = IOrderCFService;
        }

        private void setStrategy(IPredictionStrategy strategy)
        {
            _strategy = strategy;
        }

        // implemented from ICarbonNotification
        public List<OrderCarbonFootprintRDM> GetOrderEmission()
        {
            return orderEmission;
        }

        // implemented from ICarbonNotification
        public List<ItemCarbonFootprintRDM> GetItemEmission()
        {
            return itemEmission;
        }

        // implemented from ICarbonNotification
        public async Task RetrieveItemEmission()
        {
            // itemEmission = new List<ItemCarbonFootprintRDM>();
            itemEmission = _IItemCFService.getAllItemCarbonFootprint();
        }

        // implemented from ICarbonNotification
        public async Task RetrieveOrderEmission()
        {
            orderEmission = _IOrderCFService.getAllOrderCarbonFootprint();
        }

        // new method implemented from ICarbonNotification
        public async Task<decimal> GetTotalEmissionsForMonthAsync(int month, int year)
        {
            try
            {
                // ensure data is loaded
                if (itemEmission == null)
                    await RetrieveItemEmission();
                if (orderEmission == null)
                    await RetrieveOrderEmission();

                // filter items for the specified month and year, then sum emissions
                var itemEmissions = itemEmission
                    .Where(item => item.retrieveDateCreated().Year == year && item.retrieveDateCreated().Month == month)
                    .Sum(item => (decimal)item.calculateSelfEmission());

                // filter orders for the specified month and year, then sum emissions
                var orderEmissions = orderEmission
                    .Where(order => order.retrieveDateCreated().Year == year && order.retrieveDateCreated().Month == month)
                    .Sum(order => (decimal)order.calculateSelfEmission());

                // calculate total
                decimal totalEmissions = itemEmissions + orderEmissions;
                return totalEmissions;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error calculating total emissions for {month}/{year}: {ex.Message}");
                return 0m; // return 0 decimal on error
            }
        }

        public List<OrderCarbonFootprintRDM> getOrderEmission(){
            return orderEmission;
        }
        public List<ItemCarbonFootprintRDM> getItemEmission(){
            return itemEmission;
        }
        public async Task retrieveItemEmission(){
            // itemEmission = new List<ItemCarbonFootprintRDM>();
            itemEmission = _IItemCFService.getAllItemCarbonFootprint();
        }
        public async Task retrieveOrderEmission(){
           orderEmission = _IOrderCFService.getAllOrderCarbonFootprint();
        }
        public async Task predictCarbonEmission(){
            List<OrderCarbonFootprintRDM> orderList = getOrderEmission();
            List<ItemCarbonFootprintRDM> itemList = getItemEmission();

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
            List<OrderCarbonFootprintRDM> orderList = getOrderEmission();
            List<ItemCarbonFootprintRDM> itemList = getItemEmission();

            Dictionary<DateTime, double> emissionsByMonth = new Dictionary<DateTime, double>();

            // Aggregate emissions from both lists
            AggregateEmissions(orderList, emissionsByMonth);
            AggregateEmissions(itemList, emissionsByMonth);
    
            // Group by Month and Sum Emissions
            List<EmissionPredDTO> graphEmission = emissionsByMonth
            // .GroupBy(e => new DateTime(e.Key.Year, e.Key.Month, 1)) // Group by Year-Month
            .Select(g => new EmissionPredDTO
            {
                day = g.Key,
                emisission = g.Value
            })
            .ToList();

            return graphEmission;
        }
        private void AggregateEmissions<T>(List<T> records, Dictionary<DateTime, double> monthlyEmissions)
        {
            if (records != null)
            {
                Dictionary<DateTime, double> emissionsByDate = new Dictionary<DateTime, double>();
                // Aggregate by date
                foreach (var record in records)
                {
                    
                    DateTime date;
                    double emission;

                    if (record is OrderCarbonFootprintRDM orderRecord)
                    {
                        date = orderRecord.retrieveDateCreated().Date; // Extract date part only
                        emission = orderRecord.calculateSelfEmission();
                    }
                    else if (record is ItemCarbonFootprintRDM itemRecord)
                    {
                        date = itemRecord.retrieveDateCreated().Date; // Extract date part only
                        emission = itemRecord.calculateSelfEmission();
                    }
                    else
                    {
                        continue; // Skip if the type is unsupported
                    }

                    if (emissionsByDate.ContainsKey(date))
                    {
                        emissionsByDate[date] += emission;
                    }
                    else
                    {
                        emissionsByDate[date] = emission;
                    }
                }
                // Convert to Monthly Aggregation sorted date by asc order
                foreach (var entry in emissionsByDate.OrderBy(e => e.Key))
                {
                    DateTime monthKey = new DateTime(entry.Key.Year, entry.Key.Month, 1); // First day of the month
                    if (monthlyEmissions.ContainsKey(monthKey))
                    {
                        monthlyEmissions[monthKey] += entry.Value;
                    }
                    else
                    {
                        monthlyEmissions[monthKey] = entry.Value;
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