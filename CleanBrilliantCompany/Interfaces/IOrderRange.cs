using System.Collections.Generic;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IOrderRange
    {
        // Fetches all orders for a given month across all years - For team 6
        List<OrderRDM> getOrdersByDateRange(int monthNumber);
    }
}