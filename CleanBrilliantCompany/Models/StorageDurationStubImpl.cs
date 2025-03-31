using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Models
{
    public class StorageDurationStubImpl : IStorageDuration
    {
        public int GetStorageDuration(int batchCode)
        {
            // simulate storage duration retrieval
            return 3;
        }
    }
}
