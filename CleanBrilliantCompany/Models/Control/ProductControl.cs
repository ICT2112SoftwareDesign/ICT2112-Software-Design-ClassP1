using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;
using System.Globalization;
using CleanBrilliantCompany.Models.Factory;

namespace CleanBrilliantCompany.Models.Control
{

    public class ProductControl : IProductQuery, IProduct, IProductQuantity, IManufacturer, IBatch
    {
        private readonly ProductMapper _productMapper;
        private readonly iReorderRequest _ireorderRequest;
        private readonly Lazy<IItemCreation> _lazyItemCreation;
        private readonly ProductFactory _liquidProductFactory;
        private readonly ProductFactory _solidProductFactory;

        public ProductControl(IConfiguration configuration, iReorderRequest ireorderRequest, Lazy<IItemCreation> lazyItemCreation)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _productMapper = new ProductMapper(connectionString);
            _ireorderRequest = ireorderRequest;
            _lazyItemCreation = lazyItemCreation;
            _liquidProductFactory = new LiquidProductFactory(_productMapper);
            _solidProductFactory = new SolidProductFactory(_productMapper);
            Console.WriteLine("Products loaded from database.");
        }

        // Product
        public Product getProductDetails(int productId)
        {
            return  _productMapper.findByProductId(productId);
        }

        public List<Product> getAllProducts()
        {
            return  _productMapper.findAllProducts();
        }

        public int createProduct(string productName, string category, float productCost,
                              int manufacturerId, float weight, int quantity, int volumeOrZero,
                              float toxicityPercentage, int carbonFootprint, bool isLiquid)
        {
            // Log the isLiquid value for debugging
            Console.WriteLine($"[DEBUG] isLiquid: {isLiquid}");
            ProductFactory productFactory = isLiquid ? _liquidProductFactory : _solidProductFactory;

            int productId = productFactory.CreateProduct(productName, category, productCost, 
                                                        manufacturerId, weight, quantity, volumeOrZero,
                                                        toxicityPercentage, carbonFootprint, isLiquid);

            return productId;
        }


        public bool updateProduct(int productId, string productName, string category, float productCost, 
        int manufacturerId, float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            bool error = _productMapper.update(productId, productName, category, productCost, 
                                    manufacturerId, productWeight, quantity, volume, toxicityPercentage, carbonFootprint, productState);
            return error;
        }

        public void updateQuantity(int productId, int quantity, string arithmeticOperations)
        {
            Product product = getProductDetails(productId);
            List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
            productInfo.Add(product.retrieveProductInfo());
            int oldQty = Convert.ToInt32(productInfo[0]["Quantity"]);
            _productMapper.update(productId, oldQty, quantity, arithmeticOperations);
        }


        // Product Batch
        public List<ProductBatch> getAllProductBatch()
        {
            return _productMapper.findAllProductBatch(); 
        }

        public ProductBatch getBatchDetails(int batchCode) 
        {
            return _productMapper.findByBatchCode(batchCode);
        }

        public int createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, float batchCost)
        {
            return _productMapper.insert(productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost);
        }

        // Product Stock History
        public List<StockHistory> getStockHistoryByBatch(int batchCode)
        {
            List<StockHistory> allHistories = _productMapper.findAllStockHistory();
            var filteredHistories = allHistories
                 .Where(history => history.GetBatchCode() == batchCode)
                 .ToList();
 
             return filteredHistories;
        }

        public Dictionary<int, List<StockHistory>> getStockHistoryByDate(DateOnly stockTakeDate) 
        {
            var stockHistoryDictionary = new Dictionary<int, List<StockHistory>>();
            List<StockHistory> stockHistories = _productMapper.findAllStockHistory(); 

            foreach (var stockHistory in stockHistories)
            {
                if (stockHistory.GetStockTakeDate() == stockTakeDate)
                {
                    int stockBatchCodeKey = stockHistory.GetBatchCode();
                    if (!stockHistoryDictionary.ContainsKey(stockBatchCodeKey))
                    {
                        stockHistoryDictionary[stockBatchCodeKey] = new List<StockHistory>();
                    }
                    stockHistoryDictionary[stockBatchCodeKey].Add(stockHistory);
                }
            }
            return stockHistoryDictionary;
        }

        public void createStockHistory(int batchCode, DateOnly stockTakeDate, int quantity, DateTime recordedDate) 
        {
            _productMapper.insert(batchCode, stockTakeDate, quantity, recordedDate);
        }

        // ProductManufacturer
        public ProductManufacturer getManufacturerDetails(int manufacturerId)
        {
            ProductManufacturer productManufacturer = _productMapper.findProductManufacturerById(manufacturerId);
            return productManufacturer;
        }

        public List<ProductManufacturer> getAllProductManufacturer()
        {
            List<ProductManufacturer> productManufacturer = _productMapper.findAllManufacturer();
            return productManufacturer;
        }

        // Product Reorder Request ** SIMULATION **
        public void processReorderRequest()
        {
            List<ReorderRequestSample> reorderRequests = _ireorderRequest.displayListOfReorders();
            List<ReorderRequestSample> ApprovedReorderRequests = new List<ReorderRequestSample>();
            // Grab only Approved reorderRequests
            foreach (var reorderRequest in reorderRequests)
            {
                if (reorderRequest.Status == "Approved")
                {
                    ApprovedReorderRequests.Add(reorderRequest);
                }
            }

            // Check output
            foreach (var request in ApprovedReorderRequests)
            {
                Console.WriteLine("----- Approved Reorder -----");
                Console.WriteLine($"Reorder ID: {request.ReorderId}");
                Console.WriteLine($"Product ID: {request.ProductId}");
                Console.WriteLine($"Quantity: {request.Quantity}");
                Console.WriteLine($"Manufacturer ID: {request.ManufacturerId}");
                Console.WriteLine($"Expected Delivery Date: {request.ExpectedDeliveryDate:dd/MM/yyyy}");
                Console.WriteLine($"Status: {request.Status}");
                Console.WriteLine($"Defect Quantity: {request.DefectQuantity}");
                Console.WriteLine();

                /// Dynamically generated dates based on ExpectedDeliveryDate ///
                DateTime expiryDate = request.ExpectedDeliveryDate.AddMonths(1);
                DateTime manufactureDate = request.ExpectedDeliveryDate.AddMonths(-1);
  
                List<Dictionary<string, object>> productInfo = new List<Dictionary<string, object>>();
                Product product = getProductDetails(request.ProductId);
                productInfo.Add(product.retrieveProductInfo());
                float costPrice = Convert.ToSingle(productInfo[0]["ProductCost"]);


                float batchCost = costPrice * request.Quantity; // Total qty * productCost
                Console.WriteLine($"Batch COst: {batchCost}");
                int warehouseId = 1;
                //////////////////////////////////

                // Create a batch object to represent the batch that went in for every approved reorderRequest
                int batchCode = createProductBatch(request.ProductId, expiryDate, 
                request.ExpectedDeliveryDate, manufactureDate, request.Quantity, batchCost);

                // Create stock history, the dates are one to one with batch?
                DateOnly stockTakeDate = DateOnly.FromDateTime(request.ExpectedDeliveryDate); // Convert from DateTime to DateOnly
                createStockHistory(batchCode, stockTakeDate, request.Quantity, request.ExpectedDeliveryDate);

                // Change quantity
                updateQuantity(request.ProductId, request.Quantity, "increase");

                // Add the amount of items into the db.
                for (int i = 0; i < request.Quantity; i++)
                {
                    _lazyItemCreation.Value.createItem(request.ProductId, batchCode, warehouseId, ItemStatus.Available);
                    Console.WriteLine($"Added Item");
                }
            }
                
        }

    }
}