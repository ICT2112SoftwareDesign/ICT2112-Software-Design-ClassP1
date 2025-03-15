using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
    public interface iItemDatabase
    {
        bool getDatabaseQueryStatus(SqlDataReader reader);  
    }
}
