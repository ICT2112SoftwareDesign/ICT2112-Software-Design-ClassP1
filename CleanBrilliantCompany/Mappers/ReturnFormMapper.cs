using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Models.Entity;
using System.Diagnostics;
using CleanBrilliantCompany.Interfaces;
using System.Reflection.PortableExecutable;

namespace CleanBrilliantCompany.Mapper
{
	public class ReturnFormMapper : iReturnFormDatabase
	{
		private readonly string _connectionString;

		public ReturnFormMapper(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnectionString");
		}

		// Get all Return Forms.
		public async Task<List<ReturnForm>> findAll()
		{
				List<ReturnForm> allReturnForms = new List<ReturnForm>();

				using (SqlConnection connection = new SqlConnection(_connectionString))
				{
					try
					{
						await connection.OpenAsync();

						// Define SQL query
						string query = @"BEGIN TRANSACTION;

							SELECT rf.returnId, rf.manufacturerId, rf.itemId, i.warehouseId, rf.returnReason, rf.staffId 
							FROM [dbo].[ReturnForm] rf 
							INNER JOIN [dbo].[Item] i ON rf.itemId = i.itemId;

							COMMIT TRANSACTION;";

						// Create a command object to execute query
						SqlCommand command = new SqlCommand(query, connection);

						// Execute command and use data reader to get the results
						using (SqlDataReader reader = await command.ExecuteReaderAsync())
						{
							if (getDatabaseQueryStatus(reader))
							{
								// Loop through the results and map them to ReturnForm objects
								while (reader.Read())
								{
									int returnId = reader.GetInt32(reader.GetOrdinal("returnId"));
									int manufacturerId = reader.GetInt32(reader.GetOrdinal("manufacturerId"));
									int itemId = reader.GetInt32(reader.GetOrdinal("itemId"));
									int warehouseId = reader.GetInt32(reader.GetOrdinal("warehouseId"));
									string returnReason = reader.GetString(reader.GetOrdinal("returnReason"));
									int staffId = reader.GetInt32(reader.GetOrdinal("staffId"));

									// Map to objects.
									ReturnForm returnForm = ReturnForm.createForm(returnId, manufacturerId, itemId, warehouseId, returnReason, staffId);

									// Add object to list.
									allReturnForms.Add(returnForm);
								}
							}
						}
					}

					catch (Exception ex)
					{
						Debug.WriteLine($"Error finding all return forms: {ex.Message}");
					}

					finally
					{
						await connection.CloseAsync();
					}
				}

				return allReturnForms;
		}


		// Get Return Form by ItemId.
		public async Task<ReturnForm?> findByItemId(int returnId)
		{
			ReturnForm? returnForm = null;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					await connection.OpenAsync();

					string query = @"BEGIN TRANSACTION;

						SELECT * 
						FROM [dbo].[ReturnForm] rf 
						INNER JOIN [dbo].[Item] i ON rf.returnId = i.returnId WHERE i.returnId = @returnId;

						COMMIT TRANSACTION;";

					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@returnId", returnId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						if (getDatabaseQueryStatus(reader))
						{
							if (reader.Read())
							{
								// Loop through the results and map them to ReturnForm objects

								int itemId = reader.GetInt32(reader.GetOrdinal("itemId"));
								int manufacturerId = reader.GetInt32(reader.GetOrdinal("manufacturerId"));
								int warehouseId = reader.GetInt32(reader.GetOrdinal("warehouseId"));
								string returnReason = reader.GetString(reader.GetOrdinal("returnReason"));
								int staffId = reader.GetInt32(reader.GetOrdinal("staffId"));

								// Map to objects.
								returnForm = ReturnForm.createForm(returnId, manufacturerId, itemId, warehouseId, returnReason, staffId);
							}
						}
					}
				}

				catch (Exception ex)
				{
					Debug.WriteLine($"Error finding one return form: {ex.Message}");
				}

				finally
				{
					await connection.CloseAsync();
				}
			}
			return returnForm;
		}


		// Delete Return Form from database.
		public async Task<bool> delete(int returnId)
		{
			bool isSuccess = false;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					await connection.OpenAsync();

					string query = @"BEGIN TRY 
										BEGIN TRANSACTION;

										UPDATE [dbo].[Item] 
										SET itemStatus = 'Available', returnId = NULL 
										WHERE returnId = @returnId;

										DELETE FROM [dbo].[ReturnForm] WHERE returnId = @returnId;

										COMMIT TRANSACTION;
									END TRY 

									BEGIN CATCH 
										ROLLBACK;

										PRINT ERROR_MESSAGE();
									END CATCH;";

					SqlCommand command = new SqlCommand(query, connection);

					command.Parameters.AddWithValue("@returnId", returnId);

					int rowsAffected = await command.ExecuteNonQueryAsync();

                    isSuccess = getDatabaseQueryStatus(null, rowsAffected);

				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Error deleting return form: {ex.Message}");
				}
				finally
				{
					await connection.CloseAsync();
				}
			}

			return isSuccess;
		}


		// Insert Return Form into database.
		public async Task<ReturnForm?> insert(ReturnForm entity)
		{

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					await connection.OpenAsync();

					string query = @"
						DECLARE @ReturnIdTable TABLE (returnId INT);

						BEGIN TRY 
							BEGIN TRANSACTION;

							INSERT INTO [dbo].[ReturnForm] (manufacturerId, itemId, returnReason, staffId) 
							OUTPUT INSERTED.returnId INTO @ReturnIdTable 
							VALUES (@manufacturerId, @itemId, @returnReason, @staffId);

							DECLARE @ReturnId INT;
							SELECT @ReturnId = returnId FROM @ReturnIdTable;

							UPDATE [dbo].[Item] 
							SET [itemStatus] = 'Refunded', [returnId] = @ReturnId 
							WHERE [itemId] = @itemId;
							
							COMMIT TRANSACTION;

							SELECT @ReturnId;
						END TRY 

						BEGIN CATCH
							ROLLBACK TRANSACTION;
							THROW;
						END CATCH;";

					SqlCommand command = new SqlCommand(query, connection);

					command.Parameters.AddWithValue("@manufacturerId", entity.GetManufacturerId());
					command.Parameters.AddWithValue("@itemId", entity.GetItemId());
					command.Parameters.AddWithValue("@returnReason", entity.GetReturnReason());
					command.Parameters.AddWithValue("@staffId", entity.GetStaffId());

					int returnId = (int)await command.ExecuteScalarAsync();

					if (getDatabaseQueryStatus(null, returnId))
					{
						entity.SetReturnId(returnId);
					}
					else 
					{
                        entity = null;
                    }
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Error inserting return form: {ex.Message}");
                    entity = null;
                }
				finally
				{
					await connection.CloseAsync();
				}
			}

			return entity;
		}

        public bool getDatabaseQueryStatus(SqlDataReader reader, int rowsAffected = -1)
        {
            try
            {
                // If rowsAffected is provided (not -1), check if rows were affected
                if (rowsAffected != -1)
                {
                    return rowsAffected > 0;
                }

                // Otherwise, check if the reader has rows (for SELECT queries)
                return reader.HasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in database query status: {ex.Message}");
                return false;
            }
        }

        //public async Task<ReturnForm?> getDatabaseQueryStatus(Task<ReturnForm?> task)
        //{
        //	return await task;
        //}

        //public async Task<bool> getDatabaseQueryStatus(Task<bool> task)
        //{
        //	return await task;
        //}


        // TEMP!!----------------------------------------------------------------------------
        public async Task<int> getWarehouseIdByItemId(int itemId)
		{
			int warehouseId = -1;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					await connection.OpenAsync();

					string query = "SELECT * FROM [dbo].[Item] WHERE [itemId] = @itemId";
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@itemId", itemId);

					using (SqlDataReader reader = command.ExecuteReader())
					{
						// Loop through the results and map them to Item objects
						if (reader.Read())
						{
							warehouseId = reader.GetInt32(reader.GetOrdinal("warehouseId"));
						}

					}
				}

				catch (Exception ex)
				{
					Debug.WriteLine($"Error occurred: {ex.Message}");
				}

				finally
				{
					connection.Close();
				}
			}

			return warehouseId;
		}


		public async Task<string> getItemStatusByItemId(int itemId)
		{
            string status = "Refunded";

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					await connection.OpenAsync();

					string query = "SELECT * FROM [dbo].[Item] WHERE [itemId] = @itemId";
					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@itemId", itemId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						// Loop through the results and map them to Item objects
						if (reader.Read())
						{
							status = reader.GetString(reader.GetOrdinal("itemStatus"));
						}

					}
				}

				catch (Exception ex)
				{
					Debug.WriteLine($"Error occurred: {ex.Message} - GET STATUS");
				}

				finally
				{
					await connection.CloseAsync();
				}
			}

			return status;
		}


		//public async Task<string> getDatabaseQueryStatus(Task<string> task)
		//{
		//	return await task;
		//}
		//public async Task<int> getDatabaseQueryStatus(Task<int> task)
		//{
		//	return await task;
		//}
		// TEMP!!----------------------------------------------------------------------------
	}
}
