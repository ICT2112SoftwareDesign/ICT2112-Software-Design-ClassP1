using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;
namespace CleanBrilliantCompany.Interfaces
{
    public interface iProductDatabase
    {
        bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}