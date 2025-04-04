// Interfaces/ICarbonNotification.cs
using CleanBrilliantCompany.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Interfaces
{
    /// <summary>
    /// interface for retrieving carbon emission data from items and orders
    /// </summary>
    public interface ICarbonNotification
    {
        /// <summary>
        /// retrieves item carbon footprint data
        /// </summary>
        Task RetrieveItemEmission();

        /// <summary>
        /// retrieves order carbon footprint data
        /// </summary>
        Task RetrieveOrderEmission();

        /// <summary>
        /// gets the list of item carbon footprints
        /// </summary>
        List<ItemCarbonFootprintRDM> GetItemEmission();

        /// <summary>
        /// gets the list of order carbon footprints
        /// </summary>
        List<OrderCarbonFootprintRDM> GetOrderEmission();

        /// <summary>
        /// calculates total emissions for a specific month and year
        /// </summary>
        Task<decimal> GetTotalEmissionsForMonthAsync(int month, int year);
    }
}
