using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;
using System.Globalization;

namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery, iProduct
    {
        private readonly ProductMapper _productMapper;
        private readonly iReorderRequest _ireorderRequest;
        public ProductControl(string connectionString, iReorderRequest ireorderRequest)
        {
            _productMapper = new ProductMapper(connectionString);
            _ireorderRequest = ireorderRequest;

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
            _productMapper.insert(productName, category, productCost, 
                                    manufacturerId, weight, quantity, volume, toxicityPercentage, carbonFootprint, productState);
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

        public void updateQuantity(int productId, int quantity)
        {
            _productMapper.update(productId, quantity);
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
        public Dictionary<string, List<StockHistory>> getStockHistoryByBatch(int batchCode)
        {
            List<StockHistory> stockHistories = _productMapper.findAllStockHistory(); // Return this if only want list of batches
            var stockHistoryDictionary = new Dictionary<string, List<StockHistory>>(); // Return this if dictionary
            foreach (var stockHistory in stockHistories)
            {
                if (stockHistory.GetBatchCode() == batchCode)
                {
                    string stockTakeDateKey = stockHistory.GetStockTakeDate().ToString("yyyy-MM-dd");
                    if (!stockHistoryDictionary.ContainsKey(stockTakeDateKey))
                    {
                        stockHistoryDictionary[stockTakeDateKey] = new List<StockHistory>();
                    }
                    stockHistoryDictionary[stockTakeDateKey].Add(stockHistory);
                }
            }
            return stockHistoryDictionary;
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
                //////////////////////////////////

                // Create a batch object to represent the batch that went in for every approved reorderRequest
                int batchCode = createProductBatch(request.ProductId, expiryDate, 
                request.ExpectedDeliveryDate, manufactureDate, request.Quantity, batchCost);

                // Create stock history, the dates are one to one with batch?
                // createStockHistory(int batchCode, DateOnly stockTakeDate, int quantity, DateTime recordedDate)
                DateOnly stockTakeDate = DateOnly.FromDateTime(request.ExpectedDeliveryDate); // Convert from DateTime to DateOnly
                createStockHistory(batchCode, stockTakeDate, request.Quantity, request.ExpectedDeliveryDate);

                // Grab the OG quantity from product
                Product product = getProductDetails(request.ProductId);
                Dictionary<string, object> productInfo = product.retrieveProductInfo();

                // Update Quantity of Product items with added stock
                if (productInfo != null)
                {
                    int productQuantity = (int)productInfo["Quantity"];
                    int finalQuantity = productQuantity + request.Quantity;
                    updateQuantity(request.ProductId, finalQuantity);
                }

                // Add the amount of items into the db.
                for (int i = 0; i < request.Quantity; i++)
                {
                    Console.WriteLine($"Added Item");
                    // Get method from interface for creation
                    // createItem(itemID?, request.ProductId, salePrice?, batchCode, warehouseId?, status = available)
                }

            }
                
        }

    }
}