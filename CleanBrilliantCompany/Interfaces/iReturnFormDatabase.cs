using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormDatabase<T>
	{
		T? getDatabaseQueryStatus(Task<T?> task);
		List<T> getDatabaseQueryStatus(Task<List<T>> task);
		
		bool getDatabaseQueryStatus(Task<bool> task);

        // TEMP!!-------------------
        string getDatabaseQueryStatus(Task<string> task);
		int getDatabaseQueryStatus(Task<int> task);
		// TEMP!!-------------------

		Task<List<T>> findAll();
		Task<T?> findByItemId(int id);
		Task<bool> delete(int id);
		Task<T?> insert(T entity);

	}
}
