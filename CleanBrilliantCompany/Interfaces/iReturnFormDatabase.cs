using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
	public interface iReturnFormDatabase
	{
		bool getDatabaseQueryStatus(SqlDataReader reader, int results = -1);
	}
}
