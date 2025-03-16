using CleanBrilliantCompany.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class CarbonFootprintMapper : ICarbonRepositoryQuery
    {
        private bool _lastQueryStatus = false;
        private readonly string _connectionString;

        public CarbonFootprintMapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Connection string not found");
        }

        public bool getQueryStatus()
        {
            return _lastQueryStatus;
        }

        public bool createCarbonFootprint(int entityId, string entityType, float carbonEmission, string ecoStatus, DateTime dateCreated)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "INSERT INTO CarbonFootprintRecord (EntityId, EntityType, CarbonEmission, EcoStatus, DateCreated) VALUES (@EntityId, @EntityType, @CarbonEmission, @EcoStatus, @DateCreated)";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EntityId", entityId);
                    cmd.Parameters.AddWithValue("@EntityType", entityType);
                    cmd.Parameters.AddWithValue("@CarbonEmission", carbonEmission);
                    cmd.Parameters.AddWithValue("@EcoStatus", ecoStatus);
                    cmd.Parameters.AddWithValue("@DateCreated", dateCreated);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    _lastQueryStatus = rowsAffected > 0;
                    return _lastQueryStatus;
                }
            }
        }

        public bool deleteCarbonFootprint(int carbonFootprintId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM CarbonFootprintRecord WHERE CarbonFootprintId = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", carbonFootprintId);
                    conn.Open();
                    _lastQueryStatus = cmd.ExecuteNonQuery() > 0;
                    return _lastQueryStatus;
                }
            }
        }

        public bool updateCarbonFootprint(int carbonFootprintId, int entityId, string entityType, float carbonEmission, string ecoStatus)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "UPDATE CarbonFootprintRecord SET EntityId = @EntityId, EntityType = @EntityType, CarbonEmission = @CarbonEmission, EcoStatus = @EcoStatus WHERE CarbonFootprintId = @Id";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EntityId", entityId);
                    cmd.Parameters.AddWithValue("@EntityType", entityType);
                    cmd.Parameters.AddWithValue("@CarbonEmission", carbonEmission);
                    cmd.Parameters.AddWithValue("@EcoStatus", ecoStatus);
                    cmd.Parameters.AddWithValue("@Id", carbonFootprintId);

                    conn.Open();
                    _lastQueryStatus = cmd.ExecuteNonQuery() > 0;
                    return _lastQueryStatus;
                }
            }
        }

        public float retrieveProductCarbonFootprint(int entityId, string entityType)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM CarbonFootprintRecord WHERE EntityId = @EntityId AND EntityType = @EntityType";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EntityId", entityId);
                    cmd.Parameters.AddWithValue("@EntityType", entityType);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToSingle(result) : 0f;
                }
            }
        }

        public float retrieveOrderCarbonFootprint(int entityId, string entityType)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM CarbonFootprintRecord WHERE EntityId = @EntityId AND EntityType = @EntityType";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@EntityId", entityId);
                    cmd.Parameters.AddWithValue("@EntityType", entityType);

                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToSingle(result) : 0f;
                }
            }
        }

        public List<CarbonFootprintRecordRDM> retrieveAllProductCarbonFootprint()
        {
            var list = new List<CarbonFootprintRecordRDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM CarbonFootprintRecord WHERE EntityType = 'Product'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new CarbonFootprintRecordRDM(
                                reader.GetInt32(reader.GetOrdinal("CarbonFootprintId")),
                                reader.GetInt32(reader.GetOrdinal("EntityId")),
                                reader.GetString(reader.GetOrdinal("EntityType")),
                                reader.GetDouble(reader.GetOrdinal("CarbonEmission")),
                                reader.GetString(reader.GetOrdinal("EcoStatus")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DateCreated")))
                            ));
                        }
                    }
                }
            }

            return list;
        }

        public List<CarbonFootprintRecordRDM> retrieveAllOrderCarbonFootprint()
        {
            var list = new List<CarbonFootprintRecordRDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM CarbonFootprintRecord WHERE EntityType = 'Order'";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new CarbonFootprintRecordRDM(
                                reader.GetInt32(reader.GetOrdinal("CarbonFootprintId")),
                                reader.GetInt32(reader.GetOrdinal("EntityId")),
                                reader.GetString(reader.GetOrdinal("EntityType")),
                                reader.GetDouble(reader.GetOrdinal("CarbonEmission")),
                                reader.GetString(reader.GetOrdinal("EcoStatus")),
                                DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("DateCreated")))
                            ));
                        }
                    }
                }
            }

            return list;
        }
    }
}
