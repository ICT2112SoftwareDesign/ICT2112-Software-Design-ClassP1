using System.Collections.Generic;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Controllers
{
    public class FrontendProductManagement
    {
        private readonly IProduct _productService;

        public FrontendProductManagement(IProduct productService)
        {
            _productService = productService;
        }

        public List<Product> SearchQuery(string query)
        {
            var allProducts = _productService.getAllProducts();
            return allProducts.FindAll(p =>
            {
                var details = p.GetProductDetails();
                return details.ContainsKey("ProductName") && details["ProductName"].ToString().Contains(query, System.StringComparison.OrdinalIgnoreCase);
            });
        }

        public List<Product> SearchQuery(string query, List<string> filters)
        {
            var allProducts = _productService.getAllProducts();
            return allProducts.FindAll(p =>
            {
                var details = p.GetProductDetails();
                return details.ContainsKey("ProductName") && details["ProductName"].ToString().Contains(query, System.StringComparison.OrdinalIgnoreCase) &&
                    details.ContainsKey("Category") && filters.Contains(details["Category"].ToString());
            });
        }


        public void DisplayCarbonData()
        {
            var allProducts = _productService.getAllProducts();
            foreach (var product in allProducts)
            {
                var details = product.GetProductDetails();
                string productName = details.ContainsKey("ProductName") ? details["ProductName"].ToString() : "Unknown";
                string carbonFootprint = details.ContainsKey("CarbonFootprint") ? details["CarbonFootprint"].ToString() : "N/A";

                System.Console.WriteLine($"Product: {productName}, Carbon Footprint: {carbonFootprint}");
            }
        }


        public float CalculateProductCF(float weight, float toxicPercent)
        {
            return weight * (toxicPercent / 100);
        }
    }
}
