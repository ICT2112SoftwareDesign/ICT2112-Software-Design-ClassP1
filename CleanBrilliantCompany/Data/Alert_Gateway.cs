// Data/Alert_Gateway.cs
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Data
{
    /// <summary>
    /// gateway implementation for accessing alert data using ado.net.
    /// </summary>
    public class Alert_Gateway : IAlertsDB
    {
        private readonly string? _connectionString;

        public Alert_Gateway(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(_connectionString))
            {
                throw new InvalidOperationException("database connection string 'defaultconnection' not configured.");
            }
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            var alerts = new List<Alert>();
            const string query = @"
                select alertid, alerttimestamp, goalmonth, goalyear,
                       targetemission, actualtotalemission, status, message
                from carbonalerts
                order by alerttimestamp desc";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        alerts.Add(MapReaderToAlert(reader));
                    }
                }
            }
            return alerts;
        }

        public async Task<int> AddAlertAsync(Alert alert)
        {
            const string query = @"
                insert into carbonalerts (alerttimestamp, goalmonth, goalyear, targetemission,
                                  actualtotalemission, status, message)
                values (@alerttimestamp, @goalmonth, @goalyear, @targetemission,
                        @actualtotalemission, @status, @message);
                select scope_identity();";

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@alerttimestamp", alert.AlertTimestamp);
                command.Parameters.AddWithValue("@goalmonth", alert.GoalMonth);
                command.Parameters.AddWithValue("@goalyear", alert.GoalYear);
                // handle nullable targetemission
                command.Parameters.AddWithValue("@targetemission", (object?)alert.TargetEmission ?? DBNull.Value);
                command.Parameters.AddWithValue("@actualtotalemission", alert.ActualTotalEmission);
                command.Parameters.AddWithValue("@status", alert.Status);
                command.Parameters.AddWithValue("@message", alert.Message);

                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }

        public async Task<bool> CheckAlertExistsAsync(int year, int month)
        {
            const string query = "select top 1 1 from carbonalerts where goalyear = @year and goalmonth = @month";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@month", month);
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                // if execute scalar returns non-null (i.e., found a record), it exists
                return result != null && result != DBNull.Value;
            }
        }

        public async Task<bool> DeleteAlertByPeriodAsync(int year, int month)
        {
            const string query = "delete from carbonalerts where goalyear = @year and goalmonth = @month";
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@year", year);
                command.Parameters.AddWithValue("@month", month);
                await connection.OpenAsync();
                int rowsAffected = await command.ExecuteNonQueryAsync();
                // return true if at least one row was deleted
                return rowsAffected > 0;
            }
        }

        // helper method to map sql data reader to alert object
        private static Alert MapReaderToAlert(SqlDataReader reader)
        {
            return new Alert
            {
                AlertId = reader.GetInt32(reader.GetOrdinal("alertid")),
                AlertTimestamp = reader.GetDateTime(reader.GetOrdinal("alerttimestamp")),
                GoalMonth = reader.GetInt32(reader.GetOrdinal("goalmonth")),
                GoalYear = reader.GetInt32(reader.GetOrdinal("goalyear")),
                // handle nullable decimal from db
                TargetEmission = reader.IsDBNull(reader.GetOrdinal("targetemission")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("targetemission")),
                ActualTotalEmission = reader.GetDecimal(reader.GetOrdinal("actualtotalemission")),
                Status = reader.GetString(reader.GetOrdinal("status")),
                Message = reader.GetString(reader.GetOrdinal("message"))
            };
        }
    }
}
