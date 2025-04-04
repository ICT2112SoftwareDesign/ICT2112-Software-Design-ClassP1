using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormDatabase<T>
	{
        T? getDatabaseQueryStatus(Task<T?> task);
        List<T> getDatabaseQueryStatus(Task<List<T>> task);

        bool getDatabaseQueryStatus(Task<bool> task);

        Task<List<ReturnForm>> findAll();
        Task<ReturnForm?> findByItemId(int itemId);
        Task<bool> delete(int itemId);
        Task<ReturnForm?> insert(ReturnForm entity);

    }
}
