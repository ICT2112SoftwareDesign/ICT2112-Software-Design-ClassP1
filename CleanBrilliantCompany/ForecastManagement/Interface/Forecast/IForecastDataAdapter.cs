using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces.Forecast
{
    public interface IForecastDataAdapter
    {
        void GetForecastInputs(DateTime selectedMonth, out List<ProductDTO> products, out Dictionary<int, int> aggregatedSales);

    }
}
