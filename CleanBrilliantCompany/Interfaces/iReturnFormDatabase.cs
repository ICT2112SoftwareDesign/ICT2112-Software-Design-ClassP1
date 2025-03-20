using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormDatabase<T>
	{
		//bool getDatabaseQueryStatus(SqlDataReader reader, int results = -1);


        T? getDatabaseQueryStatus(Task<T?> task);
        List<T> getDatabaseQueryStatus(Task<List<T>> task);

        bool getDatabaseQueryStatus(Task<bool> task);
    }
}
