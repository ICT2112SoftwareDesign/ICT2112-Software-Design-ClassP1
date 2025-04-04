using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;

namespace CleanBrilliantCompany.Data
{
    public class OrderCFMapper : IOrderCarbonFootprintDB
    {
        private bool _querySuccess;
        private readonly string _connectionString;

        public OrderCFMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found");
        }

        public bool insertOrderCF(int orderId, string transportMode, double orderWeight, double distance, double carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    INSERT INTO OrderCarbonFootprint 
                    (orderId, transportMode, orderWeight, distance, carbonEmission, ecoStatus, dateCreated)
                    VALUES 
                    (@orderId, @transportMode, @orderWeight, @distance, @carbonEmission, @ecoStatus, @dateCreated)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@orderId", orderId);
                        command.Parameters.AddWithValue("@transportMode", transportMode);
                        command.Parameters.AddWithValue("@orderWeight", orderWeight);
                        command.Parameters.AddWithValue("@distance", distance);
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

        public double retrieveOrderCarbonFootprint(int orderCFId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = @"
                    SELECT carbonEmission 
                    FROM OrderCarbonFootprint 
                    WHERE orderCFId = @orderCFId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@orderCFId", orderCFId);

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

        public List<OrderCarbonFootprintRDM> retrieveAllOrderCarbonFootprint()
        {
            List<OrderCarbonFootprintRDM> results = new List<OrderCarbonFootprintRDM>();
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT * FROM OrderCarbonFootprint";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderCFId = Convert.ToInt32(reader["orderCFId"]);
                            int orderId = Convert.ToInt32(reader["orderId"]);
                            string transportMode = reader["transportMode"]?.ToString() ?? "";
                            double orderWeight = Convert.ToDouble(reader["orderWeight"]);
                            double distance = Convert.ToDouble(reader["distance"]);
                            double carbonEmission = Convert.ToDouble(reader["carbonEmission"]);
                            string ecoStatus = reader["ecoStatus"]?.ToString() ?? "";
                            DateTime dateCreated = Convert.ToDateTime(reader["dateCreated"]);

                            var rdm = new OrderCarbonFootprintRDM(
                                orderCFId,
                                orderId,
                                transportMode,
                                orderWeight,
                                distance,
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
                return new List<OrderCarbonFootprintRDM>();
            }
        }

        public float retrieveTotalCarbonFootprint()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    string query = "SELECT SUM(carbonEmission) FROM OrderCarbonFootprint";

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
