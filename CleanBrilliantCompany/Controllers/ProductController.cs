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

        public IActionResult TestProduct()
        {
            var products = _productControl.GetProducts();
            Console.WriteLine($"Product: {string.Join(", ", products.Select(p => p.ProductName))}");
            return View("~/Views/Product/TestProduct.cshtml", products);
        }

        [HttpPost]
        public IActionResult CreateProduct(Product product){
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            _productControl.CreateProduct(product);

            return RedirectToAction("TestProduct"); // Refresh the page
        
        }

        [HttpPost]
        public IActionResult FetchProduct(int productId)
        {
            var product = _productControl.getProductDetails(productId);
            return View("~/Views/Product/FetchProduct.cshtml", product);
        }
    }
}