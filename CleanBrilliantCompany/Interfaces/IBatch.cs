using CleanBrilliantCompany.DatabaseEntities;


namespace CleanBrilliantCompany.Interface
{
    public interface IBatch
    {
        List<ProductBatchTable> GetAllProductBatch();

        // ProductBatchTable GetBatchDetails(string batchCode);
    }

}