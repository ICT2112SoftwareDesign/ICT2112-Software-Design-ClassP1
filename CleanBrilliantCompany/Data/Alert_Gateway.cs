// Data/Alert_Gateway.cs
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Data
{
    public class Alert_Gateway : IAlertsDB
    {
        private readonly string? _connectionString;

        public Alert_Gateway(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Alert>> GetAllAlertsAsync()
        {
            var alerts = new List<Alert>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT alertID, alertDate, alertMessage FROM CarbonAlerts", connection);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        alerts.Add(new Alert
                        {
                            AlertID = reader.GetInt32(0),
                            AlertDate = reader.GetDateTime(1),
                            AlertMessage = reader.GetString(2)
                        });
                    }
                }
            }

            return alerts;
        }

        public async Task<Alert> GetAlertByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT alertID, alertDate, alertMessage FROM CarbonAlerts WHERE alertID = @alertID", connection);
                command.Parameters.AddWithValue("@alertID", id);

                await connection.OpenAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new Alert
                        {
                            AlertID = reader.GetInt32(0),
                            AlertDate = reader.GetDateTime(1),
                            AlertMessage = reader.GetString(2)
                        };
                    }
                }
            }

            return null;
        }

        public async Task<int> AddAlertAsync(Alert alert)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO CarbonAlerts (alertDate, alertMessage) VALUES (@alertDate, @alertMessage); SELECT SCOPE_IDENTITY();",
                    connection);

                command.Parameters.AddWithValue("@alertDate", alert.AlertDate);
                command.Parameters.AddWithValue("@alertMessage", alert.AlertMessage);

                await connection.OpenAsync();

                var result = await command.ExecuteScalarAsync();
                return Convert.ToInt32(result);
            }
        }

        public async Task<bool> UpdateAlertAsync(Alert alert)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "UPDATE CarbonAlerts SET alertDate = @alertDate, alertMessage = @alertMessage WHERE alertID = @alertID",
                    connection);

                command.Parameters.AddWithValue("@alertID", alert.AlertID);
                command.Parameters.AddWithValue("@alertDate", alert.AlertDate);
                command.Parameters.AddWithValue("@alertMessage", alert.AlertMessage);

                await connection.OpenAsync();

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }

        public async Task<bool> DeleteAlertAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM CarbonAlerts WHERE alertID = @alertID", connection);
                command.Parameters.AddWithValue("@alertID", id);

                await connection.OpenAsync();

                var rowsAffected = await command.ExecuteNonQueryAsync();
                return rowsAffected > 0;
            }
        }
    }
}
