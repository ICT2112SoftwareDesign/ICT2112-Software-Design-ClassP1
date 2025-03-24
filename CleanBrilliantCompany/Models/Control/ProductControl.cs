using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models.Entity;
using CleanBrilliantCompany.Mappers;


namespace CleanBrilliantCompany.Models.Control
{
    public class ProductControl : iProductQuery, iProduct
    {
        private readonly ProductMapper _productMapper;

         public ProductControl(string connectionString)
        {
            _productMapper = new ProductMapper(connectionString);

            Console.WriteLine("Products loaded from database.");
        }

        // Interface methods
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

        public List<ProductBatch> getAllProductBatch()
        {
            return _productMapper.findAllProductBatch(); 
        }

        public ProductBatch getBatchDetails(int batchCode) 
        {
            return _productMapper.findByBatchCode(batchCode);
        }

        public void createProductBatch(int productId, DateTime expiryDate, 
            DateTime receiveDate, DateTime manufactureDate, int quantity, int batchCost)
        {
            _productMapper.insert(productId, expiryDate, receiveDate, manufactureDate, quantity, batchCost);
        }

        // public List<Dictionary<string, object>> getStockHistoryByBatch(int batchCode)
        // {
        //     List<StockHistory> stockHistories = _productMapper.findAllStockHistory();
        //     List<Dictionary<string, object>> stockHistoryInfo = new List<Dictionary<string, object>>();

        //     foreach (var stockHistory in stockHistories)
        //     {
        //         if (stockHistory.GetBatchCode() == batchCode)
        //         {
        //             stockHistoryInfo.Add(stockHistory.retrieveStockHistory());
        //         }
        //     }
        //     return stockHistoryInfo;
        // }

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

        // ProductManufacturer
        public ProductManufacturer getManufacturerDetails(int manufacturerId)
        {
            ProductManufacturer productManufacturer = _productMapper.getProductManufacturerById(manufacturerId);
            return productManufacturer;
        }
    }
}