using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.PortableExecutable;

namespace CleanBrilliantCompany.Mappers
{
    public class ReservationMapper
    {
        private readonly string _connectionString;

        public ReservationMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> GetNextId()
        {
            try
            {

                int reservationId = 0;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT isnull(IDENT_CURRENT('Reservation') + IDENT_INCR('Reservation'),1) AS 'reservationId'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                reservationId = reader.GetInt32(reader.GetOrdinal("reservationId"));
                            }
                        }
                    }
                }
                return reservationId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                // Consider logging the exception or throwing a custom exception
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<string> insert(int reservationId, int productId, int warehouseId, DateOnly reservationDate,
                                         string reservationPurpose, int reservedQuantity, int staffId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO dbo.Reservation (reservationId, productId, warehouseId, reservationDate, reservationPurpose,
                                                        reservedQuantity, staffId)
                        VALUES (@ReservationId, @ProductId, @WarehouseId, @ReservationDate, @ReservationPurpose, @ReservedQuantity, 
                                @StaffId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        command.Parameters.AddWithValue("@ReservationDate", reservationDate);
                        command.Parameters.AddWithValue("@ReservationPurpose", reservationPurpose);
                        command.Parameters.AddWithValue("@ReservedQuantity", reservedQuantity);
                        command.Parameters.AddWithValue("@StaffId", staffId);
                        //command.Parameters.AddWithValue("@ReservedItems", reservedItems);

                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            return $"Reservation '{reservationId}' inserted successfully.";
                        }
                        else
                        {
                            return "Error inserting reservation.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting reservation: {ex.Message}");
                return $"Error inserting reservation: {ex.Message}";
            }
        }

        public async Task<string> update(int reservationId, int productId, int warehouseId, DateOnly reservationDate,
                                         string reservationPurpose, int reservedQuantity, int staffId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        UPDATE dbo.Reservation
                        SET productId = @ProductId, warehouseId = @WarehouseId, reservationDate = @ReservationDate, reservationPurpose = @ReservationPurpose, reservedQuantity = @ReservedQuantity, 
                                staffId = @StaffId)
                        WHERE reservationId = @ReservationId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        command.Parameters.AddWithValue("@ReservationDate", reservationDate);
                        command.Parameters.AddWithValue("@ReservationPurpose", reservationPurpose);
                        command.Parameters.AddWithValue("@ReservedQuantity", reservedQuantity);
                        command.Parameters.AddWithValue("@StaffId", staffId);

                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            return $"Reservation '{reservationId}' updated successfully.";
                        }
                        else
                        {
                            return "Error updated reservation.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updated reservation: {ex.Message}");
                return $"Error updated reservation: {ex.Message}";
            }
        }

        public async Task<string> delete(int reservationId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        DELETE FROM dbo.Reservation
                        WHERE reservationId = @ReservationId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ReservationId", reservationId);

                        int result = await command.ExecuteNonQueryAsync();
                        return result > 0 ? $"Reservation ID {reservationId} deleted successfully." : $"Reservation ID {reservationId} not found.";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting reservation: {ex.Message}");
                return $"Error deleting reservation: {ex.Message}";
            }
        }

        public Reservation findByReservationId(int reservationId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT reservationId, productId, warehouseId, reservationDate, reservationPurpose, 
                        reservedQuantity, staffId
                    FROM dbo.Reservation
                    WHERE reservationId = @ReservationId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ReservationId", reservationId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Reservation(
                                reader.GetInt32(reader.GetOrdinal("reservationId")),
                                reader.GetInt32(reader.GetOrdinal("productId")),
                                reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("reservationDate"))),
                                reader.GetString(reader.GetOrdinal("reservationPurpose")),
                                reader.GetInt32(reader.GetOrdinal("reservedQuantity")),
                                reader.GetInt32(reader.GetOrdinal("staffId")),
                                null
                            );
                        }
                    }
                }
            }

            return null;
        }

        public List<Reservation> findAllReservations()
        {
            List<Reservation> reservation = new List<Reservation>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT reservationId, productId, warehouseId, reservationDate, reservationPurpose, 
                        reservedQuantity, staffId
                    FROM dbo.Reservation";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reservation.Add(new Reservation(
                                reader.GetInt32(reader.GetOrdinal("reservationId")),
                                reader.GetInt32(reader.GetOrdinal("productId")),
                                reader.GetInt32(reader.GetOrdinal("warehouseId")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("reservationDate"))),
                                reader.GetString(reader.GetOrdinal("reservationPurpose")),
                                reader.GetInt32(reader.GetOrdinal("reservedQuantity")),
                                reader.GetInt32(reader.GetOrdinal("staffId")),
                                null //ReservedItems = reader.GetFieldValue(reader.GetOrdinal("reservedItems"))
                            ));
                        }
                    }
                }
            }
            return reservation;
        }

        // Interface Methods
        public async Task<Reservation> getDatabaseQueryStatus(Task<Reservation> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database query failed: {ex.Message}");
                return null;
            }
        }

        public async Task<string> getDatabaseQueryStatus(Task<string> task)
        {
            try
            {
                return await task;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database insert failed: {ex.Message}");
                return $"Database insert failed: {ex.Message}";
            }
        }

        public async Task<(string status, List<Reservation> reservations)> getDatabaseQueryStatus(Task<List<Reservation>> task)
        {
            try
            {
                List<Reservation> reservations = await task;

                if (reservations.Count > 0)
                {
                    return ("Query executed successfully", reservations);
                }
                else
                {
                    return ("No reservation found", reservations);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database query failed: {ex.Message}");
                return ($"Database query failed: {ex.Message}", new List<Reservation>());
            }
        }
    }
}
