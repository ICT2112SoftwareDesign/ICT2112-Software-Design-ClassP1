using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastDataAdapter: IForecastDataAdapter
    {
        private readonly TempForecastIProduct _productService;
        private readonly ISales _salesService;

        public ForecastDataAdapter(TempForecastIProduct productService, ISales salesService)
        {
            _productService = productService;
            _salesService = salesService;
        }

        public void GetForecastInputs(DateTime selectedMonth, out List<ProductDTO> products, out Dictionary<int, int> aggregatedSales)
        {
            products = _productService.GetProductList();
            var sales = _salesService.getSalesData(selectedMonth.Month);

            aggregatedSales = sales
                .GroupBy(s => s.ProductID)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(s => s.Quantity)
                );
        }
    }
}

