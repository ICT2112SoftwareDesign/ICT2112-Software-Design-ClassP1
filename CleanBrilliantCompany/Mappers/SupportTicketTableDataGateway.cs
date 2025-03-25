using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using CleanBrilliantCompany.Models.SupportTicket;

namespace CleanBrilliantCompany.Data.SupportTicket
{
    public class SupportTicketTableDataGateway
    {
        private readonly string _connectionString;

        public SupportTicketTableDataGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SupportTicketSDM? FetchSupportTicket(int ticketId)
        {
            SupportTicketSDM? ticket = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT ticketId, customerId, ticketStatus, ticketCreatedAt, ticketDetails, resolutionDetails
                    FROM dbo.SupportTicket
                    WHERE ticketId = @TicketId;
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ticket = new SupportTicketSDM
                            {
                                TicketId = reader.GetInt32(0),
                                CustomerId = reader.GetInt32(1),
                                Status = reader.IsDBNull(2) ? "Open" : reader.GetString(2),
                                CreatedAt = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                                TicketDetails = reader.IsDBNull(4) ? "" : reader.GetString(4),
                                ResolutionDetails = reader.IsDBNull(5) ? "" : reader.GetString(5)
                            };
                        }
                    }
                }
            }

            return ticket;
        }

        public List<SupportTicketSDM> FetchAllSupportTickets()
        {
            List<SupportTicketSDM> tickets = new List<SupportTicketSDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = @"
                    SELECT ticketId, customerId, ticketStatus, ticketCreatedAt, ticketDetails, resolutionDetails
                    FROM dbo.SupportTicket;
                ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tickets.Add(new SupportTicketSDM
                        {
                            TicketId = reader.GetInt32(0),
                            CustomerId = reader.GetInt32(1),
                            Status = reader.IsDBNull(2) ? "Open" : reader.GetString(2),
                            CreatedAt = reader.IsDBNull(3) ? DateTime.Now : reader.GetDateTime(3),
                            TicketDetails = reader.IsDBNull(4) ? "" : reader.GetString(4),
                            ResolutionDetails = reader.IsDBNull(5) ? "" : reader.GetString(5)
                        });
                    }
                }
            }

            return tickets;
        }

        public bool CreateSupportTicket(int customerId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string insertQuery = @"
                    INSERT INTO dbo.SupportTicket (customerId, ticketStatus, ticketCreatedAt, ticketDetails, resolutionDetails)
                    VALUES (@CustomerId, 'Submitted', GETDATE(), 'Pending customer input', '');
                ";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public void DeleteTicket(int ticketId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string deleteQuery = "DELETE FROM dbo.SupportTicket WHERE ticketId = @TicketId";

                using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateSupportTicket(int ticketId, string resolutionDetails)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string updateQuery = @"
                    UPDATE dbo.SupportTicket
                    SET resolutionDetails = @ResolutionDetails, ticketStatus = 'Closed'
                    WHERE ticketId = @TicketId;
                ";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);
                    cmd.Parameters.AddWithValue("@ResolutionDetails", resolutionDetails);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
