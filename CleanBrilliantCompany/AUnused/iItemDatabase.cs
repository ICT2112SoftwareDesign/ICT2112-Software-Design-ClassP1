using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IItemDatabase
    {
        bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}
