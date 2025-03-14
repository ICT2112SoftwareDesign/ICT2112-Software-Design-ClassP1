using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using Microsoft.Extensions.Configuration;

namespace CleanBrilliantCompany.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductControl _productControl;

        public ProductController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _productControl = new ProductControl(connectionString);
        }

        public IActionResult displayProducts()
        {
            var products = _productControl.getAllProducts();
            Console.WriteLine($"Product: {string.Join(", ", products.Select(p => p.ProductName))}");
            return View("~/Views/Product/TestProduct.cshtml", products);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product){
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            _productControl.createProduct(product.ProductName, product.ProductCategory, 
                                        product.ProductCost, product.ManufacturerId, product.ProductWeight, 
                                        product.Quantity, product.Volume, product.ToxicityPercentage, 
                                        product.CarbonFootprint, product.ProductState);

            return RedirectToAction("displayProducts"); // Refresh the page
        
        }

        [HttpPost]
        public IActionResult FetchProduct(int productId)
        {
            var product = _productControl.getProductDetails(productId);
            Console.WriteLine($"Product: TEST");
            return View("~/Views/Product/FetchProduct.cshtml", product);
        }
    }
}