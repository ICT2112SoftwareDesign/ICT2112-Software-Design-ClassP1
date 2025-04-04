using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IReservationDatabase
    {
        bool GetDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1);
    }
}
