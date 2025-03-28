using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iTransactionDatabase
    {
        bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}