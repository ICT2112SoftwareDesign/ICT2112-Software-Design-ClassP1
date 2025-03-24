using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Models.Entity;
using System.Diagnostics;
using CleanBrilliantCompany.Interfaces;

namespace CleanBrilliantCompany.Mapper
{
	public class ReturnFormMapper : iReturnFormDatabase<ReturnForm>
	{
		private readonly string _connectionString;

		public ReturnFormMapper(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
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

							SELECT rf.returnId, rf.manufacturerId, rf.itemId, i.productId, i.warehouseId, rf.returnReason, rf.staffId 
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
									int? itemId = reader.GetInt32(reader.GetOrdinal("itemId"));
									int productId = reader.GetInt32(reader.GetOrdinal("productId"));
									string returnReason = reader.GetString(reader.GetOrdinal("returnReason"));
									int staffId = reader.GetInt32(reader.GetOrdinal("staffId"));

									// Map to objects.
									ReturnForm returnForm = ReturnForm.createForm(returnId: returnId, manufId: manufacturerId, itemId: itemId, productId: productId, returnReason: returnReason, staffId: staffId);

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
		public async Task<ReturnForm?> findByItemId(int itemId)
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
						INNER JOIN [dbo].[Item] i ON rf.returnId = i.returnId WHERE i.itemId = @itemId;

						COMMIT TRANSACTION;";

					SqlCommand command = new SqlCommand(query, connection);
					command.Parameters.AddWithValue("@itemId", itemId);

					using (SqlDataReader reader = await command.ExecuteReaderAsync())
					{
						if (getDatabaseQueryStatus(reader))
						{
							if (reader.Read())
							{
								// Loop through the results and map them to ReturnForm objects

								int returnId = reader.GetInt32(reader.GetOrdinal("returnId"));
								int manufacturerId = reader.GetInt32(reader.GetOrdinal("manufacturerId"));
                                int productId = reader.GetInt32(reader.GetOrdinal("productId"));
                                string returnReason = reader.GetString(reader.GetOrdinal("returnReason"));
								int staffId = reader.GetInt32(reader.GetOrdinal("staffId"));

								// Map to objects.
								returnForm = ReturnForm.createForm(returnId: returnId, manufId: manufacturerId, itemId: itemId, productId: productId, returnReason: returnReason, staffId: staffId);
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
		public async Task<bool> delete(int itemId)
		{
			bool isSuccess = false;

			using (SqlConnection connection = new SqlConnection(_connectionString))
			{
				try
				{
					await connection.OpenAsync();

					string query = @"BEGIN TRY 
										BEGIN TRANSACTION;

										DELETE FROM [dbo].[ReturnForm] WHERE itemId = @itemId;

										COMMIT TRANSACTION;
									END TRY 

									BEGIN CATCH 
										ROLLBACK;

										PRINT ERROR_MESSAGE();
									END CATCH;";

					SqlCommand command = new SqlCommand(query, connection);

					command.Parameters.AddWithValue("@itemId", itemId);

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

        public bool getDatabaseQueryStatus(SqlDataReader reader, int result = -1)
        {
            try
            {
                // If rowsAffected is provided (not -1), check if rows were affected
                if (result != -1)
                {
                    return result > 0;
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

        public List<ReturnForm> getDatabaseQueryStatus(Task<List<ReturnForm>> task)
        {
            return task.Result;
        }

        public ReturnForm? getDatabaseQueryStatus(Task<ReturnForm?> task)
        {
            return task.Result;
        }

        public bool getDatabaseQueryStatus(Task<bool> task)
        {
            return task.Result;
        }
    }
}
