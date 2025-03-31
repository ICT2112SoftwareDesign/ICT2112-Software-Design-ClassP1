using CleanBrilliantCompany.DatabaseEntities;


namespace CleanBrilliantCompany.Interface
{
    public interface CostIBatch
    {
        List<ProductBatchTable> GetAllProductBatch();

        // ProductBatchTable GetBatchDetails(string batchCode);
    }

}