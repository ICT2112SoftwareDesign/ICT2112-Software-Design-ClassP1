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

        public async Task<IActionResult> displayProducts()
        {
            var (status, products) = await _productControl.getAllProducts();

            Console.WriteLine($"Query Status: {status}");
            return View("~/Views/Product/TestProduct.cshtml", products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            string status = await _productControl.createProduct(product.ProductName, product.ProductCategory, 
                                        product.ProductCost, product.ManufacturerId, product.ProductWeight, 
                                        product.Quantity, product.Volume, product.ToxicityPercentage, 
                                        product.CarbonFootprint, product.ProductState);

            Console.WriteLine($"Insert Status: {status}");
            return RedirectToAction("displayProducts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            string status = await _productControl.deleteProduct(productId);

            Console.WriteLine($"Delete Status: {status}");
            return RedirectToAction("displayProducts");
        }


        [HttpPost]
        public async Task<IActionResult> FetchProduct(int productId)
        {
            var product = await _productControl.getProductDetails(productId);
            return View("~/Views/Product/FetchProduct.cshtml", product);
        }


        [HttpGet]
        public async Task<IActionResult> ProductBatch()
        {
            var batches = await _productControl.getAllProductBatches();
            return View("~/Views/Product/ProductBatch.cshtml", batches);
        }
    }
}