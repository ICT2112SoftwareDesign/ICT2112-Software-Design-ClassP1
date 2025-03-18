using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.ViewModel;
using Microsoft.Extensions.Configuration;

namespace CleanBrilliantCompany.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductControl _productControl;
        private readonly AgingControl _agingControl; // Testing

        public ProductController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _productControl = new ProductControl(connectionString);
            _agingControl = new AgingControl(_productControl); // Testing
        }

        public async Task<IActionResult> displayProducts()
        {
            var (status, products) = await _productControl.getAllProducts();

            Console.WriteLine($"Query Status: {status}");
            return View("~/Views/Product/TestProduct.cshtml", products);
        }

        public async Task<IActionResult> displayProductBatch()
        {
            var productBatches = await _productControl.getAllProductBatch();

            return View("~/Views/Product/ProductBatch.cshtml", productBatches);
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
            var batches = await _productControl.getAllProductBatch();
            return View("~/Views/Product/ProductBatch.cshtml", batches);
        }

        [HttpPost]
        public async Task<IActionResult> FetchBatch(int batchCode)
        {
            var batch = await _productControl.getBatchDetails(batchCode);

             // Ensure we pass a List<ProductBatch> even for a single result
            List<ProductBatch> batchList = batch != null ? new List<ProductBatch> { batch } : new List<ProductBatch>();
            return View("~/Views/Product/ProductBatch.cshtml", batchList);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductBatch(ProductBatch productBatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            string status = await _productControl.createProductBatch(productBatch.ProductId, productBatch.ExpiryDate, 
                    productBatch.ReceiveDate, productBatch.ManufactureDate, productBatch.Quantity, productBatch.BatchCost);

            Console.WriteLine($"Insert Status: {status}");
            return RedirectToAction("displayProductBatch");
        }

        [HttpPost]
        public async Task<IActionResult> FetchBatchStockHistory(int batchCode)
        {
            var batch = await _productControl.getBatchDetails(batchCode);
            var (status, stockHistory) = await _productControl.getStockHistoryByBatch(batchCode);

            var viewModel = new BatchDetailsViewModel
            {
                ProductBatches = batch != null ? new List<ProductBatch> { batch } : new List<ProductBatch>(),
                StockHistory = stockHistory ?? new List<StockHistory>()
            };

            await _agingControl.generateNewDashboard(); // Testing AgingControl

            return View("~/Views/Product/StockHistoryBatch.cshtml", viewModel);
        }

        
    }
}