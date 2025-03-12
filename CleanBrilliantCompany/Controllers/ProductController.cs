using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductControl _productControl;

        public ProductController()
        {
            // _productControl = new ProductControl(new ProductMapper("your_connection_string"));
            _productControl = new ProductControl();
        }

        public IActionResult TestProduct()
        {
            var products = _productControl.GetProducts();
            Console.WriteLine($"Product: {string.Join(", ", products.Select(p => p.ProductName))}");
            // return View(products);
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