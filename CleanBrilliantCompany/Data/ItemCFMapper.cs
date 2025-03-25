using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Data
{
    public class ItemCFMapper : IItemCarbonFootprintDB
    {
        private bool _querySuccess;
        private readonly string _connectionString;

        public ItemCFMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found");
        }

        public bool insertItemCF(int itemId, int productId, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO ItemCarbonFootprint 
                    (itemId, productId, carbonEmission, ecoStatus, dateCreated)
                    VALUES 
                    (@itemId, @productId, @carbonEmission, @ecoStatus, @dateCreated)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@itemId", itemId);
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@carbonEmission", carbonEmission);
                        command.Parameters.AddWithValue("@ecoStatus", ecoStatus);
                        command.Parameters.AddWithValue("@dateCreated", dateCreated);

                        command.ExecuteNonQuery();
                    }
                }

                _querySuccess = true;
                return _querySuccess;
            }
            catch (Exception ex)
            {
                _querySuccess = false;
                return _querySuccess;
            }
        }

        public bool updateAllItemCF()
        {
            // stub to simulate the age of the items as 3 days old
            int STORAGE_DAYS_CONSTANT = 3;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                        UPDATE icf
                        SET icf.carbonEmission = CAST(POWER(1.02, @constant) * p.carbonEmission AS INT)
                        FROM ItemCarbonFootprint icf
                        INNER JOIN Item i ON icf.productId = i.productId
                        INNER JOIN Product p ON i.productId = p.productId;";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@constant", STORAGE_DAYS_CONSTANT);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            _querySuccess = true;
                        }
                        else
                        {
                            _querySuccess = false;
                        }
                    }
                }

                return _querySuccess;
            }
            catch
            {
                _querySuccess = false;
                return false;
            }
        }

        public double retrieveItemCarbonFootprint(int itemCFId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT carbonEmission 
                    FROM ItemCarbonFootprint 
                    WHERE itemCFId = @itemCFId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@itemCFId", itemCFId);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            _querySuccess = true;
                            return Convert.ToDouble(result);
                        }
                        else
                        {
                            _querySuccess = false;
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _querySuccess = false;
                return 0;
            }
        }

        public double retrieveItemCarbonFootprintByItemId(int itemId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT carbonEmission 
                    FROM ItemCarbonFootprint 
                    WHERE itemId = @itemId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@itemId", itemId);

                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            _querySuccess = true;
                            return Convert.ToDouble(result);
                        }
                        else
                        {
                            _querySuccess = false;
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _querySuccess = false;
                return 0;
            }
        }

        public List<ItemCarbonFootprintRDM> retrieveAllItemCarbonFootprint()
        {
            List<ItemCarbonFootprintRDM> results = new List<ItemCarbonFootprintRDM>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM ItemCarbonFootprint";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int itemCFId = Convert.ToInt32(reader["itemCFId"]);
                            int itemId = Convert.ToInt32(reader["itemId"]);
                            int productId = Convert.ToInt32(reader["productId"]);
                            double carbonEmission = Convert.ToDouble(reader["carbonEmission"]);
                            string ecoStatus = reader["ecoStatus"]?.ToString() ?? string.Empty;
                            DateTime dateCreated = Convert.ToDateTime(reader["dateCreated"]);

                            var rdm = new ItemCarbonFootprintRDM(
                                itemCFId,
                                itemId,
                                productId,
                                carbonEmission,
                                ecoStatus,
                                dateCreated
                            );

                            results.Add(rdm);
                        }
                    }
                }

                _querySuccess = true;
                return results;
            }
            catch (Exception ex)
            {
                _querySuccess = false;
                return new List<ItemCarbonFootprintRDM>();
            }
        }

        public float retrieveTotalCarbonFootprint()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT SUM(carbonEmission) FROM ItemCarbonFootprint";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            _querySuccess = true;
                            return Convert.ToSingle(result);
                        }
                        else
                        {
                            _querySuccess = true;
                            return 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _querySuccess = false;
                return 0;
            }
        }

        public bool getQueryStatus()
        {
            return _querySuccess;
        }
    }
}
