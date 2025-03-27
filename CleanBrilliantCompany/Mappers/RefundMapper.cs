using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Data
{
    public class RefundMapper : IRefundDatabase
    {
        private readonly string _connectionString;

        public RefundMapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Insert Refund
        public Refund_RDM InsertRefund(int orderId, string refundReason, float refundAmount, List<string> images, List<string> videos, Dictionary<int, int> refundedProducts)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string insertQuery = @"
                            INSERT INTO dbo.Refund (orderId, refundedProducts, refundReason, refundAmount, refundRequestDate, images, videos, status)
                            VALUES (@OrderId, @RefundedProducts, @RefundReason, @RefundAmount, GETDATE(), @Images, @Videos, 'Pending');
                            SELECT SCOPE_IDENTITY();"; // Get last inserted Refund ID

                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@RefundReason", refundReason);
                            cmd.Parameters.AddWithValue("@RefundAmount", refundAmount);
                            cmd.Parameters.AddWithValue("@RefundedProducts", JsonSerializer.Serialize(refundedProducts)); // Convert to JSON
                            cmd.Parameters.AddWithValue("@Images", JsonSerializer.Serialize(images)); // Convert to JSON
                            cmd.Parameters.AddWithValue("@Videos", JsonSerializer.Serialize(videos)); // Convert to JSON

                            int refundId = Convert.ToInt32(cmd.ExecuteScalar()); // Get newly created Refund ID

                            transaction.Commit();

                            return new Refund_RDM
                            {
                                RefundId = refundId,
                                OrderId = orderId,
                                RefundReason = refundReason,
                                RefundAmount = refundAmount,
                                RefundRequestDate = DateTime.Now,
                                Status = "Pending",
                                RefundedProducts = refundedProducts,
                                Images = images,
                                Videos = videos
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        Console.WriteLine("Error inserting refund: " + ex.Message);
                        throw;
                    }
                }
            }
        }

        // Retrieve Refund
        public Refund_RDM ViewRefund(int refundId)
        {
            Refund_RDM refund = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string query = @"
                    SELECT refundId, orderId, refundedProducts, refundReason, refundAmount, refundRequestDate, refundProcessedDate, images, videos, status
                    FROM dbo.Refund
                    WHERE refundId = @RefundId;
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@RefundId", refundId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            refund = new Refund_RDM
                            {
                                RefundId = reader.GetInt32(0),
                                OrderId = reader.GetInt32(1),
                                RefundReason = reader.IsDBNull(3) ? "Unknown Reason" : reader.GetString(3),
                                RefundAmount = reader.IsDBNull(4) ? 0 : (float)reader.GetDecimal(4),
                                RefundRequestDate = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5),
                                RefundProcessedDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                Status = reader.IsDBNull(9) ? "Pending" : reader.GetString(9),
                                RefundedProducts = reader.IsDBNull(2) ? new Dictionary<int, int>()
                                                : JsonSerializer.Deserialize<Dictionary<int, int>>(reader.GetString(2)),
                                Images = reader.IsDBNull(7) ? new List<string>()
                                    : JsonSerializer.Deserialize<List<string>>(reader.GetString(7)),
                                Videos = reader.IsDBNull(8) ? new List<string>()
                                    : JsonSerializer.Deserialize<List<string>>(reader.GetString(8))
                            };
                        }
                    }
                }
            }

            // Return a default refund object if no record is found
            return refund ?? new Refund_RDM
            {
                RefundId = refundId,
                OrderId = 0,
                RefundReason = "Refund Not Found",
                RefundAmount = 0,
                RefundRequestDate = DateTime.MinValue,
                RefundProcessedDate = null,
                Status = "Not Found",
                RefundedProducts = new Dictionary<int, int>(),
                Images = new List<string>(),
                Videos = new List<string>()
            };
        }

        // Retrieve all Refunds 
        public List<Refund_RDM> GetAllRefunds()
        {
            List<Refund_RDM> refunds = new List<Refund_RDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT refundId, orderId, refundedProducts, refundReason, refundAmount, 
                        refundRequestDate, refundProcessedDate, images, videos, status
                    FROM dbo.Refund;
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Refund_RDM refund = new Refund_RDM
                            {
                                RefundId = reader.GetInt32(0),
                                OrderId = reader.GetInt32(1),
                                RefundReason = reader.IsDBNull(3) ? "Unknown Reason" : reader.GetString(3),  // ✅ Handle NULL
                                RefundAmount = reader.IsDBNull(4) ? 0f : (float)reader.GetDecimal(4), // ✅ Handle NULL
                                RefundRequestDate = reader.IsDBNull(5) ? DateTime.MinValue : reader.GetDateTime(5), // ✅ Handle NULL
                                RefundProcessedDate = reader.IsDBNull(6) ? (DateTime?)null : reader.GetDateTime(6),
                                Status = reader.IsDBNull(9) ? "Pending" : reader.GetString(9), // ✅ Handle NULL
                                RefundedProducts = reader.IsDBNull(2) ? new Dictionary<int, int>() : JsonSerializer.Deserialize<Dictionary<int, int>>(reader.GetString(2)), // ✅ Handle NULL
                                Images = reader.IsDBNull(7) ? new List<string>()
                                    : JsonSerializer.Deserialize<List<string>>(reader.GetString(7)), // ✅ Handle NULL
                                Videos = reader.IsDBNull(8) ? new List<string>()
                                    : JsonSerializer.Deserialize<List<string>>(reader.GetString(8))  // ✅ Handle NULL
                            };

                            refunds.Add(refund);
                        }
                    }
                }
            }
            return refunds;
        }

        // Update Refund Status 
        public void UpdateRefundStatus(int refundId, string status, DateTime processedDate)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    UPDATE dbo.Refund 
                    SET status = @Status, refundProcessedDate = @ProcessedDate
                    WHERE refundId = @RefundId";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@ProcessedDate", processedDate);
                    cmd.Parameters.AddWithValue("@RefundId", refundId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int GetTotalRefundCount()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM dbo.Refund";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public int GetPendingRefundCount()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM dbo.Refund WHERE status = 'Pending'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        // In RefundMapper.cs
        public decimal GetTotalRefundAmount()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT SUM(CAST(refundAmount AS DECIMAL(18,2))) FROM dbo.Refund WHERE status = 'Approved'";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    var result = cmd.ExecuteScalar();
                    return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
                }
            }
        }
    }
}