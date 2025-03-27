using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;
using System.Globalization;

namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery, iProduct, iProductQuantity, iBatch
    {
        private readonly ProductMapper _productMapper;
        private readonly iReorderRequest _ireorderRequest;
        // private readonly IItemCreation _iItemCreation;
        private readonly Lazy<IItemCreation> _lazyItemCreation;

        // public ProductControl(IConfiguration configuration, iReorderRequest ireorderRequest, IItemCreation iItemCreation)
        // public ProductControl(IConfiguration configuration, iReorderRequest ireorderRequest)
        public ProductControl(IConfiguration configuration, iReorderRequest ireorderRequest, Lazy<IItemCreation> lazyItemCreation)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
            _productMapper = new ProductMapper(connectionString);
            _ireorderRequest = ireorderRequest;
            // _iItemCreation = iItemCreation;
            _lazyItemCreation = lazyItemCreation;

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

        public void createProduct(string productName, string category, float productCost, 
        int manufacturerId, float weight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            // Create the product
            int productId = _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);

            Console.WriteLine($"ProductId: '{productId}'");
            
            // Create Batch
            // createProductBatch(int productId, DateTime expiryDate, 
            // DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)

            // int batchCode = createProductBatch(productId, DateTime expiryDate, 
            // DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)

            // Create the amount of items
            for (int x = 0; x < quantity; x++)
            {
                double salePrice = productCost * 1.2;
                int warehouseId = 1;
                //createItem(int productId, float salePrice, int batchCode, int warehouseId, ItemStatus status); 
                // _lazyItemCreation.Value.createItem(productId, salePrice, ,warehouseId, ItemStatus.Available);
            }
        }

        public void deleteProduct(int productId)
        {
            _productMapper.delete(productId);
        }

        public void updateProduct(int productId, string productName, string category, float productCost, 
        int manufacturerId, float productWeight, int quantity, int volume, float toxicityPercentage, int carbonFootprint, string productState)
        {
            _productMapper.update(productId, productName, category, productCost, 
                                    manufacturerId, productWeight, quantity, volume, toxicityPercentage, carbonFootprint, productState);;
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
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            return _productMapper.insert(productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost);
        }

        // Product Stock History
        // public Dictionary<string, List<StockHistory>> getStockHistoryByBatch(int batchCode)
        public List<StockHistory> getStockHistoryByBatch(int batchCode)
        {
            List<StockHistory> stockHistories = _productMapper.findAllStockHistory(); // Return this if only want list of stockHistory
            // var stockHistoryDictionary = new Dictionary<string, List<StockHistory>>(); // Return this if dictionary, Key = stockDate
            // foreach (var stockHistory in stockHistories)
            // {
            //     if (stockHistory.GetBatchCode() == batchCode)
            //     {
            //         string stockTakeDateKey = stockHistory.GetStockTakeDate().ToString("yyyy-MM-dd");
            //         if (!stockHistoryDictionary.ContainsKey(stockTakeDateKey))
            //         {
            //             stockHistoryDictionary[stockTakeDateKey] = new List<StockHistory>();
            //         }
            //         stockHistoryDictionary[stockTakeDateKey].Add(stockHistory);
            //     }
            // }
            return stockHistories;
        }

        public Dictionary<int, List<StockHistory>> getStockHistoryByDate(DateOnly stockTakeDate) 
        {
            var stockHistoryDictionary = new Dictionary<int, List<StockHistory>>(); // Return this if dictionary
            List<StockHistory> stockHistories = _productMapper.findAllStockHistory(); // Return this if only want list of stockHistory, Key = batchCode

            foreach (var stockHistory in stockHistories) // Made the attributes here public, stockTakeDate, batchCode.
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
            ProductManufacturer productManufacturer = _productMapper.getProductManufacturerById(manufacturerId);
            return productManufacturer;
        }

        // Product Reorder Request
        public void processReorderRequest()
        {
            List<ReorderRequestSample> reorderRequests = _ireorderRequest.displayListOfReorders();
            List<ReorderRequestSample> ApprovedReorderRequests = new List<ReorderRequestSample>();
            // Grab only Approved reorderRequests? Assuming it means we receive the batch of items.
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

                
                /// Hardcoded random variables ///
                string expiryDateSample = "05/06/2025"; // in the format of the db, mm/dd/yyyy
                DateTime expiryDate = DateTime.ParseExact(expiryDateSample, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Console.WriteLine($"Expirey Date Formatted: {expiryDate}");

                string manufactureDateSample = "03/06/2025"; // in the format of the db, mm/dd/yyyy
                DateTime manufactureDate = DateTime.ParseExact(manufactureDateSample, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                Console.WriteLine($"Manufactured Date Formatted: {manufactureDate}");

                int batchCost = 300;
                int salePrice = 15;
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
                    // _iItemCreation.createItem(request.ProductId, salePrice, batchCode, warehouseId, ItemStatus.Available);
                    _lazyItemCreation.Value.createItem(request.ProductId, salePrice, batchCode, warehouseId, ItemStatus.Available);
                    Console.WriteLine($"Added Item");
                }
            }
                
        }

    }
}