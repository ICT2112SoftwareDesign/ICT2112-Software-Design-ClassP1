using CleanBrilliantCompany.Models.Entity;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormDatabase
	{
		Task<ReturnForm?> getDatabaseQueryStatus(Task<ReturnForm?> task);
		Task<List<ReturnForm>> getDatabaseQueryStatus(Task<List<ReturnForm>> task);

		Task<bool> getDatabaseQueryStatus(Task<bool> task);

		// TEMP!!-------------------
		Task<string> getDatabaseQueryStatus(Task<string> task);
		Task<int> getDatabaseQueryStatus(Task<int> task);
		// TEMP!!-------------------

		Task<List<ReturnForm>> findAll();
		Task<ReturnForm?> findByItemId(int id);
		Task<bool> delete(int id);
		Task<ReturnForm?> insert(ReturnForm entity);

	}
}
