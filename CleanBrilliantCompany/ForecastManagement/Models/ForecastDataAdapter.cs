using CleanBrilliantCompany.ForecastManagement.DTO;
using CleanBrilliantCompany.ForecastManagement.Interface;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;

namespace CleanBrilliantCompany.ForecastManagement.Models
{
    public class ForecastDataAdapter : IForecastDataAdapter
    {
        private readonly IOrderRange _salesService;
        private readonly IProduct _iProduct;

        public ForecastDataAdapter(IOrderRange salesService, IProduct iProduct)
        {
            _salesService = salesService;
            _iProduct = iProduct;

        }

     
        public void GetForecastInputs(
            DateTime selectedMonth,
            out List<ProductDTO> products,
            out Dictionary<int, int> aggregatedSales)
        {
            // Get all products
            var productsInfo = _iProduct.getAllProducts();
            products = new List<ProductDTO>();

            foreach (var productInfo in productsInfo)
            {
                var productDetails = productInfo.retrieveProductInfo();
                int id = productDetails.ContainsKey("ProductId") ? (int)productDetails["ProductId"] : 0;
                string name = productDetails.ContainsKey("ProductName") ? productDetails["ProductName"]?.ToString() ?? string.Empty : string.Empty;

                var dto = new ProductDTO(id, name);
                products.Add(dto);
            }

            // Get all sales in the selected month
            var sales = _salesService.getSalesData(selectedMonth.Month);

            // Map to final ProductID
            var localAggregatedSales = new Dictionary<int, int>();

            foreach (var sale in sales)
            {
                if (!localAggregatedSales.ContainsKey(sale.ProductID))
                    localAggregatedSales[sale.ProductID] = 0;

                localAggregatedSales[sale.ProductID] += sale.Quantity;
            }

            // Now filter products to only those with sales
            products = products.Where(p => localAggregatedSales.ContainsKey(p.ID)).ToList();

            // Finally assign to the out parameter
            aggregatedSales = localAggregatedSales;
        }



    }
}

