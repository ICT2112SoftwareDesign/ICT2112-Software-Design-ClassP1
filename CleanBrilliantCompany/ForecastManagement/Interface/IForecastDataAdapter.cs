using CleanBrilliantCompany.ForecastManagement.DTO;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.ForecastManagement.Interface
{
    public interface IForecastDataAdapter
    {
        void GetForecastInputs(DateTime selectedMonth, out List<ProductDTO> products, out Dictionary<int, int> aggregatedSales);

    }
}
