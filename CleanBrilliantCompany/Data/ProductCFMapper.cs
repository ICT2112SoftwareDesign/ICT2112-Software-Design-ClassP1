using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Data
{
    public class ProductCFMapper : IProductCarbonFootprintDB
    {
        private bool _querySuccess;
        private readonly string _connectionString;

        public ProductCFMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found");
        }

        public bool insertProductCF(int productId, string productName, string productCategory, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO ProductCarbonFootprint 
                    (productId, productName, productCategory, carbonEmission, ecoStatus, dateCreated)
                    VALUES 
                    (@productId, @productName, @productCategory, @carbonEmission, @ecoStatus, @dateCreated)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productId", productId);
                        command.Parameters.AddWithValue("@productName", productName);
                        command.Parameters.AddWithValue("@productCategory", productCategory);
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

        public double retrieveProductCarbonFootprint(int productCFId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT carbonEmission 
                    FROM ProductCarbonFootprint 
                    WHERE productCFId = @productCFId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@productCFId", productCFId);

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

        public List<ProductCarbonFootprintRDM> retrieveAllProductCarbonFootprint()
        {
            List<ProductCarbonFootprintRDM> results = new List<ProductCarbonFootprintRDM>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM ProductCarbonFootprint";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productCFId = Convert.ToInt32(reader["productCFId"]);
                            int productId = Convert.ToInt32(reader["productId"]);
                            string productName = reader["productName"]?.ToString() ?? string.Empty;
                            string productCategory = reader["productCategory"]?.ToString() ?? string.Empty;
                            double carbonEmission = Convert.ToDouble(reader["carbonEmission"]);
                            string ecoStatus = reader["ecoStatus"]?.ToString() ?? string.Empty;
                            DateTime dateCreated = Convert.ToDateTime(reader["dateCreated"]);

                            var rdm = new ProductCarbonFootprintRDM(
                                productCFId,
                                productId,
                                productName,
                                productCategory,
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
                return new List<ProductCarbonFootprintRDM>();
            }
        }

        public float retrieveTotalCarbonFootprint()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT SUM(carbonEmission) FROM ProductCarbonFootprint";

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
