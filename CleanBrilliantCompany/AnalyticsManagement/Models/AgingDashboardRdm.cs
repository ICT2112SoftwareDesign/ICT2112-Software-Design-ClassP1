using System.Collections.Generic;
using CleanBrilliantCompany.Models.Entity;

public class AgingDashboardRdm : Dashboard
{
    // batchCode to list of analytics 
    private Dictionary<int, List<AbstractAnalyticsDetails>> _batchAnalyticsMap;

    // productID to list of batchCode 
    private Dictionary<int, List<int>> _productToBatchMap = new Dictionary<int, List<int>>();

    // productID to productName 
    private Dictionary<int, string> _productIDToNameMap = new Dictionary<int, string>();

    public AgingDashboardRdm(int id, string name, DateTime requestedStartDate, DateTime requestedEndDate, int validityDuration, int type, DateTime? generatedDate = null)
        : base(id, name, requestedStartDate, requestedEndDate, validityDuration, type, generatedDate)
    {
        _batchAnalyticsMap = new Dictionary<int, List<AbstractAnalyticsDetails>>();
    }

    public Dictionary<int, List<int>> GetProductToBatchMap() => _productToBatchMap;

    public void AddBatchtoProductMap(int productId, int batchCode)
    {
        if (_productToBatchMap.ContainsKey(productId))
        {
            _productToBatchMap[productId].Add(batchCode);
        }
        else
        {
            _productToBatchMap.Add(productId, new List<int> { batchCode });
        }
    }

    public Dictionary<int, string> GetProductIDToNameMap() => _productIDToNameMap;

    public void AddProductToNameMap(int productId, string productName)
    {
        if (!_productIDToNameMap.ContainsKey(productId))
        {
            _productIDToNameMap.Add(productId, productName);
        }
    }

    public void PopulateAnalytics(List<ProductBatch> productBatchData, List<StockHistory> stockHistoryData) 
    {
        Dictionary<int, Dictionary<DateOnly, int>> stockHistoryMap = stockHistoryData
        .Select(s => {
            var info = s.retrieveStockHistory();
            return new 
            {
                batchCode = (int)info["BatchCode"],
                stockTakeDate = (DateOnly)info["StockTakeDate"],
                quantity = (int)info["Quantity"]
            };
        })
        .GroupBy(x => x.batchCode)
        .ToDictionary(
            g => g.Key,
            g => g.ToDictionary(x => x.stockTakeDate, x => x.quantity)
        );

        foreach (var batch in productBatchData)
        {
            var info = batch.retrieveProductBatchInfo();
            int batchCode = (int)info["BatchCode"];
            int productId = (int)info["ProductId"];
            DateTime receiveDate = (DateTime)info["ReceiveDate"];
            DateTime expiryDate = (DateTime)info["ExpiryDate"];
            int quantity = (int)info["Quantity"];

            if (!_batchAnalyticsMap.ContainsKey(batchCode))
            {
                _batchAnalyticsMap.Add(batchCode, new List<AbstractAnalyticsDetails>());
            }

            AddBatchtoProductMap(productId, batchCode);

            var storageLifeCycleAnalytics = new StorageLifeCycleAnalyticsDetails(
                batchCode,
                receiveDate,
                expiryDate);

            var stockTurnOverAnalytics = new StockTurnOverAnalyticsDetails(
                batchCode,
                quantity);

            if (stockHistoryMap.ContainsKey(batchCode))
            {
                stockTurnOverAnalytics.SetQuantityPerDay(stockHistoryMap[batchCode]);
            }

            AddBatchAnalytics(batchCode, storageLifeCycleAnalytics);
            AddBatchAnalytics(batchCode, stockTurnOverAnalytics);
        }
    }

    public void AddBatchAnalytics(int batchCode, AbstractAnalyticsDetails batchDetails)
    {
        if (_batchAnalyticsMap.ContainsKey(batchCode))
        {
            _batchAnalyticsMap[batchCode].Add(batchDetails);
        }
        else
        {
            _batchAnalyticsMap.Add(batchCode, new List<AbstractAnalyticsDetails> { batchDetails });
        }
    }

    public List<AbstractAnalyticsDetails>? GetBatchAnalytics(int batchCode)
    {
        return _batchAnalyticsMap.ContainsKey(batchCode) ? _batchAnalyticsMap[batchCode] : null;
    }
}
