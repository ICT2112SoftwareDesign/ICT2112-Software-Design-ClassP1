using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class ReorderRequestMapper : IReorderRequestDB
    {
        private readonly string _connectionString;

        public ReorderRequestMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ReorderRequest_RDM> displayListOfReorders()
        {
            var list = new List<ReorderRequest_RDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT reorderId, ProductId, Quantity, ManufacturerId, ExpectedDeliveryDate, Status, DefectQuantity FROM dbo.ReorderRequest";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new ReorderRequest_RDM
                        {
                            ReorderId = reader.GetInt32(0),
                            ProductId = reader.GetInt32(1),
                            Quantity = reader.GetInt32(2),
                            ManufacturerId = reader.GetInt32(3),
                            ExpectedDeliveryDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                            Status = reader.IsDBNull(5) ? "Pending" : reader.GetString(5),
                            DefectQuantity = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),

                        });
                    }
                }
            }

            return list;
        }

        public void createReorderRequest(ReorderRequest_RDM reorder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    INSERT INTO dbo.ReorderRequest (ProductId, Quantity, ManufacturerId, ExpectedDeliveryDate, Status, DefectQuantity)
                    VALUES (@ProductId, @Quantity, @ManufacturerId, @ExpectedDeliveryDate, @Status, @DefectQuantity);
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", reorder.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", reorder.Quantity);
                    cmd.Parameters.AddWithValue("@ManufacturerId", reorder.ManufacturerId);
                    cmd.Parameters.AddWithValue("@ExpectedDeliveryDate", reorder.ExpectedDeliveryDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", reorder.Status ?? "Pending");
                    cmd.Parameters.AddWithValue("@DefectQuantity", reorder.DefectQuantity ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public ReorderRequest_RDM getReorderRequestDetails(int reorderId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"SELECT reorderId, ProductId, Quantity, ManufacturerId, ExpectedDeliveryDate, Status, DefectQuantity 
                                 FROM dbo.ReorderRequest WHERE reorderId = @ReorderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReorderId", reorderId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new ReorderRequest_RDM
                            {
                                ReorderId = reader.GetInt32(0),
                                ProductId = reader.GetInt32(1),
                                Quantity = reader.GetInt32(2),
                                ManufacturerId = reader.GetInt32(3),
                                ExpectedDeliveryDate = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                                Status = reader.IsDBNull(5) ? "Pending" : reader.GetString(5),
                                DefectQuantity = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6),

                            };
                        }
                    }
                }
            }

            return new ReorderRequest_RDM { Status = "Not Found" };
        }

        public void updateReorderRequest(ReorderRequest_RDM reorder)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE dbo.ReorderRequest
                    SET ProductId = @ProductId,
                        Quantity = @Quantity,
                        ManufacturerId = @ManufacturerId,
                        DefectQuantity = @DefectQuantity
                    WHERE ReorderId = @ReorderId
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReorderId", reorder.ReorderId);
                    cmd.Parameters.AddWithValue("@ProductId", reorder.ProductId);
                    cmd.Parameters.AddWithValue("@Quantity", reorder.Quantity);
                    cmd.Parameters.AddWithValue("@ManufacturerId", reorder.ManufacturerId);
                    cmd.Parameters.AddWithValue("@DefectQuantity", reorder.DefectQuantity ?? (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void cancelReorderRequest(int reorderId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"DELETE FROM dbo.ReorderRequest WHERE ReorderId = @ReorderId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ReorderId", reorderId);
                    cmd.ExecuteNonQuery();
                }
            }
        }


    }
}
