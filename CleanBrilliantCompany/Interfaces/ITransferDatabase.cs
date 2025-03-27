using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
    public interface ITransferDatabase
    {
        bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}