using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Services
{
    /// <summary>
    /// service implementation for calculating emission data using existing db mappers.
    /// </summary>
    public class EmissionDataService : IEmissionDataService
    {
        private readonly IItemCarbonFootprintDB _itemCFDB;
        private readonly IOrderCarbonFootprintDB _orderCFDB;

        public EmissionDataService(IItemCarbonFootprintDB itemCFDB, IOrderCarbonFootprintDB orderCFDB)
        {
            _itemCFDB = itemCFDB ?? throw new ArgumentNullException(nameof(itemCFDB));
            _orderCFDB = orderCFDB ?? throw new ArgumentNullException(nameof(orderCFDB));
        }

        /// <summary>
        /// calculates total emissions by fetching all items and orders and filtering/summing in memory.
        /// </summary>
        public Task<decimal> GetTotalEmissionsForMonthAsync(int month, int year)
        {
            try
            {
                // retrieve all records using existing synchronous methods
                var allItems = _itemCFDB.retrieveAllItemCarbonFootprint();
                var allOrders = _orderCFDB.retrieveAllOrderCarbonFootprint();

                // filter items for the specified month and year, then sum emissions
                var itemEmissions = allItems
                    .Where(item => item.retrieveDateCreated().Year == year && item.retrieveDateCreated().Month == month)
                    .Sum(item => (decimal)item.calculateSelfEmission());

                // filter orders for the specified month and year, then sum emissions
                var orderEmissions = allOrders
                    .Where(order => order.retrieveDateCreated().Year == year && order.retrieveDateCreated().Month == month)
                    .Sum(order => (decimal)order.calculateSelfEmission());

                // calculate total
                decimal totalEmissions = itemEmissions + orderEmissions;

                // wrap the synchronous result in a completed task to match the interface signature
                return Task.FromResult(totalEmissions);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error calculating total emissions for {month}/{year}: {ex.Message}");
                // rethrow or return a specific error state if desired
                return Task.FromResult(0m); // return 0 decimal on error
            }
        }
    }
}