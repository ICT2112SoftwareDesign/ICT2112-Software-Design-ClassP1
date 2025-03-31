using CleanBrilliantCompany.DTO;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Interfaces.Forecast;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Linq;

namespace CleanBrilliantCompany.Models.Forecast
{
    public class ForecastDataAdapter: IForecastDataAdapter
    {
        private readonly TempForecastIProduct _productService;
        private readonly IOrderRange _salesService;
        private readonly IProduct _iProduct;
        private readonly IItem _iItem;
        private readonly IBatch _iBatch;

        public ForecastDataAdapter(TempForecastIProduct productService, IOrderRange salesService, IProduct iProduct, IItem iItem, IBatch iBatch)
        {
            _productService = productService;
            _salesService = salesService;
            _iProduct = iProduct;
            _iBatch = iBatch;
            _iItem = iItem;
        }

        //public void GetForecastInputs(DateTime selectedMonth, out List<ProductDTO> products, out Dictionary<int, int> aggregatedSales)
        //{
        //    // Get all products
        //    var productsInfo = _iProduct.getAllProducts(); // List of products with dictionary info
        //    products = new List<ProductDTO>();


        //    foreach (var productInfo in productsInfo)
        //    { 
        //        var productDetails = productInfo.retrieveProductInfo();
        //        int id = productDetails.ContainsKey("ProductId") ? (int)productDetails["ProductId"] : 0;
        //        string name = productDetails.ContainsKey("ProductName") ? productDetails["ProductName"]?.ToString() ?? string.Empty : string.Empty;

        //        var dto = new ProductDTO(id, name);
        //        products.Add(dto);
        //    }

        //    // Get all sales in the selected month
        //    var sales = _salesService.getSalesData(selectedMonth.Month);

        //    // Map to final ProductID through Item → Batch → Product
        //    var productSales = new Dictionary<int, int>();

        //    foreach (var sale in sales)
        //    {
        //        //var itemDetails = _iItem.getItemById(sale.ItemID).Result;
        //        //int productId = (int)itemDetails.retrieveItemInfo()["ProductId"];

        //        if (!productSales.ContainsKey(sale.ProductID))
        //            productSales[sale.ProductID] = 0;

        //        productSales[sale.ProductID] += sale.Quantity;
        //    }

        //    aggregatedSales = productSales;

        //}
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

