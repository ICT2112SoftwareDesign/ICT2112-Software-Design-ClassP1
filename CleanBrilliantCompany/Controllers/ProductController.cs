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

        // To change idk where yall put the stuffs
        public async Task<IActionResult> Index()
        {
            var products = _productControl.getAllProducts();
            // return View("~/Views/Product/TestProduct.cshtml", products);

            return View(products);
        }

        public async Task<IActionResult> displayProducts()
        {
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();

            List<Product> products = _productControl.getAllProducts();

            foreach (var product in products)
            {
                productInfo.Add(product.retrieveProductInfo());
            }
            //_agingControl.testProductInterfaceMethods(); // Just to see the iProduct working

            return View("~/Views/Product/TestProduct.cshtml", productInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(string productName, string productCategory, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productControl.createProduct(productName, productCategory, 
                                        productCost, manufacturerId, weight, 
                                        quantity, volume, toxicityPercentage, 
                                        carbonFootprint, productState);

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
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
            Product product = _productControl.getProductDetails(productId);

            if (product != null)
            {
                productInfo.Add(product.retrieveProductInfo());
            }
            return View("~/Views/Product/TestProduct.cshtml", productInfo);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(int productId, string productName, string productCategory, float productCost, 
        int manufacturerId, float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productControl.updateProduct(productId, productName, productCategory, 
                                        productCost, manufacturerId, productWeight, 
                                        quantity, volume, toxicityPercentage, 
                                        carbonFootprint, productState);;
            return RedirectToAction("displayProducts");
        }

        // Product Batch
        public async Task<IActionResult> displayProductBatch()
        {
            var productBatches = _productControl.getAllProductBatch();

            return View("~/Views/Product/ProductBatch.cshtml", productBatches);
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
        public async Task<IActionResult> FetchBatchStockHistoryByCode(int batchCode)
        {
            var stockHistory = _productControl.getStockHistoryByBatch(batchCode);

            return View("~/Views/Product/StockHistoryBatch.cshtml", stockHistory);
        }

        [HttpPost]
        public async Task<IActionResult> FetchBatchStockHistoryByDate(DateOnly stockTakeDate)
        {
            var stockHistory = _productControl.getStockHistoryByDate(stockTakeDate);

            // SHow the query for the time being
            foreach (var kvp in stockHistory)
            {
                int batchCode = kvp.Key;
                List<StockHistory> records = kvp.Value;

                Console.WriteLine($"Batch Code: {batchCode}");
                Console.WriteLine("-----------------------------");

                foreach (var stock in records)
                {
                    Console.WriteLine($"Stock ID: {stock.StockId}");
                    Console.WriteLine($"Stock Take Date: {stock.StockTakeDate}");
                    Console.WriteLine($"Quantity: {stock.Quantity}");
                    Console.WriteLine($"Recorded Date: {stock.RecordedDate}");
                    Console.WriteLine();
                }
            }

            //Just for now, need to fix views
            return RedirectToAction("displayProducts");
        }        
    }
}