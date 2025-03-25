using Microsoft.AspNetCore.Mvc;
using CleanBrilliantCompany.Models.Control;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Models.ViewModel;
using Microsoft.Extensions.Configuration;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductControl _productControl;
        private readonly AgingControl _agingControl; // Testing

        public ProductController(IConfiguration configuration)
        {
            // string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            // _productControl = new ProductControl(connectionString);
            _productControl = new ProductControl(configuration);
            _agingControl = new AgingControl(_productControl); // Testing
        }

        public async Task<IActionResult> displayProducts()
        {
            var products = _productControl.getAllProducts();
            //_agingControl.testProductInterfaceMethods(); // Just to see the iProduct working

            return View("~/Views/Product/TestProduct.cshtml", products);
        }

        public async Task<IActionResult> displayProductBatch()
        {
            var productBatches = _productControl.getAllProductBatch();

            return View("~/Views/Product/ProductBatch.cshtml", productBatches);
        }
        // public async Task<IActionResult> displayProductBatch()
        // {
        //     var productBatches = await _productControl.getAllProductBatch();

        //     return View("~/Views/Product/ProductBatch.cshtml", productBatches);
        // }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid product data.");
            }

            _productControl.createProduct(product.ProductName, product.ProductCategory, 
                                        product.ProductCost, product.ManufacturerId, product.ProductWeight, 
                                        product.Quantity, product.Volume, product.ToxicityPercentage, 
                                        product.CarbonFootprint, product.ProductState);

            return RedirectToAction("displayProducts");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            _productControl.deleteProduct(productId);

            return RedirectToAction("displayProducts");
        }


        [HttpPost]
        public async Task<IActionResult> FetchProduct(int productId)
        {
            var product = _productControl.getProductDetails(productId);
            return View("~/Views/Product/FetchProduct.cshtml", product);
        }

        [HttpPost]
        public async Task<IActionResult> FetchUpdateProduct(int productId)
        {
            var product = _productControl.getProductDetails(productId);
            return View("~/Views/Product/UpdateProduct.cshtml", product);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(Product product)
        {
            _productControl.updateProduct(product.ProductId, product.ProductName, product.ProductCategory, 
                                        product.ProductCost, product.ManufacturerId, product.ProductWeight, 
                                        product.Quantity, product.Volume, product.ToxicityPercentage, 
                                        product.CarbonFootprint, product.ProductState);;
            return RedirectToAction("displayProducts");
        }


        [HttpGet]
        public async Task<IActionResult> ProductBatch()
        {
            var batches = _productControl.getAllProductBatch();
            return View("~/Views/Product/ProductBatch.cshtml", batches);
        }


        [HttpPost]
        public async Task<IActionResult> FetchBatch(int batchCode)
        {
            var batch = _productControl.getBatchDetails(batchCode);

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

            _productControl.createProductBatch(productBatch.ProductId, productBatch.ExpiryDate, 
                    productBatch.ReceiveDate, productBatch.ManufactureDate, productBatch.Quantity, productBatch.BatchCost);

            return RedirectToAction("displayProductBatch");
        }

        [HttpPost]
        public async Task<IActionResult> FetchBatchStockHistory(int batchCode)
        {
            var batch =  _productControl.getBatchDetails(batchCode);
            var stockHistory = _productControl.getStockHistoryByBatch(batchCode);

            var viewModel = new BatchDetailsViewModel
            {
                ProductBatches = batch != null ? new List<ProductBatch> { batch } : new List<ProductBatch>(),
                StockHistory = stockHistory ?? new List<StockHistory>()
            };

            return View("~/Views/Product/StockHistoryBatch.cshtml", viewModel);
        }        
    }
}