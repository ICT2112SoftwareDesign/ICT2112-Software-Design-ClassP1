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

        // public ProductController(IConfiguration configuration, iReorderRequest reorderRequest, IItemCreation iItemCreation)
        public ProductController(IConfiguration configuration, iReorderRequest reorderRequest)
        {
            // _productControl = new ProductControl(configuration, reorderRequest, iItemCreation);
            _productControl = new ProductControl(configuration, reorderRequest);
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

            List<Dictionary<string, object>> batchInfo = new List<Dictionary<string, object>>();
            List<ProductBatch> productBatches = _productControl.getAllProductBatch();

            foreach (var productBatch in productBatches)
            {
                batchInfo.Add(productBatch.retrieveProductBatchInfo());
            }

            return View("~/Views/Product/ProductBatch.cshtml", batchInfo);
        }

        [HttpPost]
        public async Task<IActionResult> FetchBatch(int batchCode)
        {
            List<Dictionary<string, object>> batchInfo = new List<Dictionary<string, object>>();
            ProductBatch batch = _productControl.getBatchDetails(batchCode);

            if (batch != null)
            {
                batchInfo.Add(batch.retrieveProductBatchInfo());
            }
            return View("~/Views/Product/ProductBatch.cshtml", batchInfo);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            _productControl.createProductBatch(productId, expiryDate, 
                    receiveDate, manufactureDate, quantity, batchCost);

            return RedirectToAction("displayProductBatch");
        }

        [HttpPost]
        // WIP
        public async Task<IActionResult> FetchBatchStockHistoryByCode(int batchCode)
        {
            // Dictionary<string, List<StockHistory>> stockHistoryDictionary = _productControl.getStockHistoryByBatch(batchCode);

            // // Debug Line
            // foreach (var kvp in stockHistoryDictionary)
            // {
            //     string key = kvp.Key;
            //     List<StockHistory> records = kvp.Value;

            //     Console.WriteLine($"Group Key: {key}");
            //     Console.WriteLine("-----------------------------");

            //     foreach (var stock in records)
            //     {
            //         var stockData = stock.retrieveStockHistory();

            //         Console.WriteLine($"Stock ID: {stockData["StockId"]}");
            //         Console.WriteLine($"Batch Code: {stockData["BatchCode"]}");
            //         Console.WriteLine($"Stock Take Date: {stockData["StockTakeDate"]}");
            //         Console.WriteLine($"Quantity: {stockData["Quantity"]}");
            //         Console.WriteLine($"Recorded Date: {stockData["RecordedDate"]}");
            //         Console.WriteLine();
            //     }
            // }
            return RedirectToAction("displayProductBatch");
        }

        // Stock History
        [HttpPost]
        public async Task<IActionResult> FetchBatchStockHistoryByDate(DateOnly stockTakeDate)
        {
            // Dictionary<int, List<StockHistory>> stockHistoryDictionary = _productControl.getStockHistoryByDate(stockTakeDate);

            // // Debug Line
            // foreach (var kvp in stockHistoryDictionary)
            // {
            //     int key = kvp.Key;
            //     List<StockHistory> records = kvp.Value;

            //     Console.WriteLine($"Group Key: {key}");
            //     Console.WriteLine("-----------------------------");

            //     foreach (var stock in records)
            //     {
            //         var stockData = stock.retrieveStockHistory();

            //         Console.WriteLine($"Stock ID: {stockData["StockId"]}");
            //         Console.WriteLine($"Batch Code: {stockData["BatchCode"]}");
            //         Console.WriteLine($"Stock Take Date: {stockData["StockTakeDate"]}");
            //         Console.WriteLine($"Quantity: {stockData["Quantity"]}");
            //         Console.WriteLine($"Recorded Date: {stockData["RecordedDate"]}");
            //         Console.WriteLine();
            //     }
            // }

            return RedirectToAction("displayProductBatch");
        }

        // ProductManufecturer
        [HttpPost]
        public async Task<IActionResult> FetchProductManufecturer(int manufacturerId)
        {
            Dictionary<string, object> productManufacturerInfo = _productControl.getManufacturerDetails(manufacturerId)
            .retrieveProductManufacturerInfo();

            // Debug Line
            if (productManufacturerInfo != null)
            {
                int manufacturerId1 = (int)productManufacturerInfo["ManufacturerId"];
                string companyName = productManufacturerInfo["CompanyName"].ToString();
                string address = productManufacturerInfo["ManufacturerAddress"].ToString();
                string email = productManufacturerInfo["Email"].ToString();

                Console.WriteLine($"ID: {manufacturerId1}");
                Console.WriteLine($"Company: {companyName}");
                Console.WriteLine($"Address: {address}");
                Console.WriteLine($"Email: {email}");
            }
            
            return RedirectToAction("displayProducts");
        }

        // Reorder Request
        public async Task<IActionResult> ReorderRequest() 
        {
            _productControl.processReorderRequest();
            return RedirectToAction("displayProducts");
        }
    }
}