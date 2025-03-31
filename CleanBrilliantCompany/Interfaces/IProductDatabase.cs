using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;
namespace CleanBrilliantCompany.Interfaces
{
    public interface IProductDatabase
    {
        bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}