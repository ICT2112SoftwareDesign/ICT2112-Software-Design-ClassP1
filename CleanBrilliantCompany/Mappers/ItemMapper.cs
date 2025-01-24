using CleanBrilliantCompany.Models.Entity;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Mappers
{
    public class ItemMapper
    {
        private readonly string _connectionString;

        public ItemMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<Item> findByItemId(int itemId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"
                    SELECT itemId, productId, expiryDate, receiveDate, manufactureDate, salePrice, batchCode,
                            warehouseId, status, reservationId, orderId, transferId, returnId
                    FROM dbo.Item
                    WHERE itemId = @ItemId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemId", itemId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Item
                            {
                                ItemId = reader.GetInt32(reader.GetOrdinal("itemId")),
                                ProductId = reader.GetInt32(reader.GetOrdinal("productId")),
                                ExpiryDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("expiryDate"))),
                                ReceiveDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("receiveDate"))),
                                ManufactureDate = DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("manufactureDate"))),
                                SalePrice = reader.GetFloat(reader.GetOrdinal("salePrice")),
                                BatchCode = reader.GetInt32(reader.GetOrdinal("batchCode")),
                                WarehouseId = reader.GetInt32(reader.GetOrdinal("batchCode")),
                                Status = (Status)reader.GetInt32(reader.GetOrdinal("status")),
                                ReservationId = reader.GetInt32(reader.GetOrdinal("reservationId")),
                                OrderId = reader.GetInt32(reader.GetOrdinal("orderId")),
                                TransferId = reader.GetInt32(reader.GetOrdinal("transferId")),
                                ReturnId = reader.GetInt32(reader.GetOrdinal("returnId"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<int> getNextId()
        {
            try
            {

                int itemId = 0;

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        SELECT isnull(IDENT_CURRENT('Item') + IDENT_INCR('Item'),1) AS 'itemId'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                itemId = reader.GetInt32(reader.GetOrdinal("itemId"));
                            }
                        }
                    }
                }
                return itemId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing query: {ex.Message}");
                // Consider logging the exception or throwing a custom exception
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<string> insert(int itemId, int productId, DateOnly expiryDate, DateOnly receiveDate, DateOnly manufactureDate,
                    float salePrice, int batchCode, int warehouseId, Status status, int reservationId, int orderId,
                    int transferId, int returnId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO dbo.Reservation (itemId, productId, expiryDate, receiveDate, manufactureDate, salePrice, batchCode,
                                                        warehouseId, status, reservationId, orderId, transferId, returnId)
                        VALUES (@ItemId, @ProductId, @ExpiryDate, @ReceiveDate, @ManufactureDate, @SalePrice, @BatchCode, @WarehouseId
                                @Status, @ReservationId, @OrderId, @TransferId, @ReturnId)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemId", itemId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                        command.Parameters.AddWithValue("@ReceiveDate", receiveDate);
                        command.Parameters.AddWithValue("@ManufactureDate", manufactureDate);
                        command.Parameters.AddWithValue("@SalePrice", salePrice);
                        command.Parameters.AddWithValue("@BatchCode", batchCode);
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@TransferId", transferId);
                        command.Parameters.AddWithValue("@ReturnId", returnId);

                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            return $"Item '{itemId}' inserted successfully.";
                        }
                        else
                        {
                            return "Error inserting item.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting item: {ex.Message}");
                return $"Error inserting item: {ex.Message}";
            }
        }

        public async Task<string> update(int itemId, int productId, DateOnly expiryDate, DateOnly receiveDate, DateOnly manufactureDate,
                    float salePrice, int batchCode, int warehouseId, Status status, int reservationId, int orderId,
                    int transferId, int returnId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string query = @"
                        UPDATE dbo.Item
                        SET productId = @ProductId, expiryDate = @ExpiryDate, receiveDate = @ReceiveDate, manufactureDate = @ManufactureDate, salePrice = @SalePrice, 
                                batchCode = @BatchCode, warehouseId = @WarehouseId, status = @Status, reservationId = @ReservationId, orderId = @OrderId, transferId = @TransferId, returnId = @ReturnId)
                        WHERE itemId = @ItemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemId", itemId);
                        command.Parameters.AddWithValue("@ProductId", productId);
                        command.Parameters.AddWithValue("@ExpiryDate", expiryDate);
                        command.Parameters.AddWithValue("@ReceiveDate", receiveDate);
                        command.Parameters.AddWithValue("@ManufactureDate", manufactureDate);
                        command.Parameters.AddWithValue("@SalePrice", salePrice);
                        command.Parameters.AddWithValue("@BatchCode", batchCode);
                        command.Parameters.AddWithValue("@WarehouseId", warehouseId);
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@ReservationId", reservationId);
                        command.Parameters.AddWithValue("@OrderId", orderId);
                        command.Parameters.AddWithValue("@TransferId", transferId);
                        command.Parameters.AddWithValue("@ReturnId", returnId);

                        int result = await command.ExecuteNonQueryAsync();
                        if (result > 0)
                        {
                            return $"Item '{itemId}' updated successfully.";
                        }
                        else
                        {
                            return "Error updating Item.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating item: {ex.Message}");
                return $"Error updating item: {ex.Message}";
            }
        }
    }
}
