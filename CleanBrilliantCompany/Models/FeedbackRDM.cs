using System;
using CleanBrilliantCompany.Interfaces;
using Microsoft.Data.SqlClient; // Updated to the latest SQL Client library
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace CleanBrilliantCompany.Models
{

    public class FeedbackRDM
    {
        // public int FeedbackId { get; set; }
        // public int StaffId { get; set; }
        // public string StaffName { get; set; }
        // public string Feedback { get; set; }
        // public string Status { get; set; }
        // public string ManagerComments { get; set; }
        // public DateTime DateSubmitted { get; set; }

        private int _feedbackId;
        private int _staffId;
        private string _staffName;
        private string _feedback;
        private string _status;
        private string _managerComments;
        private DateTime _dateSubmitted;

        public int FeedbackId
        {
            get => _feedbackId;
            set => _feedbackId = value;
        }

        public int StaffId
        {
            get => _staffId;
            set => _staffId = value;
        }

        public string StaffName
        {
            get => _staffName;
            set => _staffName = value;
        }

        public string Feedback
        {
            get => _feedback;
            set => _feedback = value;
        }

        public string Status
        {
            get => _status;
            set => _status = value;
        }

        public string ManagerComments
        {
            get => _managerComments;
            set => _managerComments = value;
        }

        public DateTime DateSubmitted
        {
            get => _dateSubmitted;
            set => _dateSubmitted = value;
        }

        // ✅ Override ToString for debugging purposes
        public override string ToString()
        {
            return $"FeedbackId: {FeedbackId}, StaffId: {StaffId}, StaffName: {StaffName}, " +
                   $"Feedback: {Feedback}, Status: {Status}, ManagerComments: {ManagerComments}, DateSubmitted: {DateSubmitted}";
        }
    }

    public class FeedbackRepository : IFeedbackDatabase
    {
        private readonly string _connectionString;

        public FeedbackRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int GetDefaultStaffId()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                string query = "SELECT TOP 1 staffId FROM dbo.GeneralStaff ORDER BY staffId ASC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        return Convert.ToInt32(result);
                    }
                }
            }
            throw new Exception("No staff members found in the database.");
        }

        public void AddFeedback(int staffId, string feedback)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM dbo.GeneralStaff WHERE staffId = @StaffID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@StaffID", staffId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0)
                    {
                        throw new Exception("Invalid staffId. The staff member does not exist.");
                    }
                }

                using (SqlCommand cmd = FeedbackMapper.CreateInsertCommand(conn, staffId, feedback))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<string> GetFeedbackList()
        {
            List<string> feedbackList = new List<string>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = "SELECT FeedbackText FROM dbo.Feedback ORDER BY DateSubmitted DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            feedbackList.Add(reader.GetString(0));
                        }
                    }
                }
            }
            return feedbackList;
        }

        public void EditFeedback(int feedbackId, string feedback)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateUpdateFeedbackCommand(conn, feedbackId, feedback))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<FeedbackRDM> GetFeedbackWithDetails()
        {
            List<FeedbackRDM> feedbackList = new List<FeedbackRDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateGetAllFeedbackCommand(conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        feedbackList = FeedbackMapper.MapList(reader);
                    }
                }
            }
            return feedbackList;
        }

        public FeedbackRDM GetFeedbackById(int feedbackId)
        {
            FeedbackRDM feedback = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateGetFeedbackByIdCommand(conn, feedbackId))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            feedback = FeedbackMapper.Map(reader);
                        }
                    }
                }
            }
            return feedback;
        }

        public void UpdateFeedback(int feedbackId, string updatedFeedback)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateUpdateFeedbackCommand(conn, feedbackId, updatedFeedback))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateManagerComment(int feedbackId, string managerComment)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateUpdateManagerCommentCommand(conn, feedbackId, managerComment))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateFeedbackStatus(int feedbackId, string status)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateUpdateStatusCommand(conn, feedbackId, status))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteFeedback(int feedbackId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateDeleteCommand(conn, feedbackId))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<FeedbackRDM> GetAllFeedback()
        {
            List<FeedbackRDM> feedbackList = new List<FeedbackRDM>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = FeedbackMapper.CreateGetAllFeedbackCommand(conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        feedbackList = FeedbackMapper.MapList(reader);
                    }
                }
            }
            return feedbackList;
        }
    }



    // Feedback Management Implementation
    public class FeedbackManagement : IFeedbackManagement
    {
        private readonly FeedbackRepository _repository;

        public FeedbackManagement(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public void UpdateManagerComment(int feedbackId, string managerComment)
        {
            _repository.UpdateManagerComment(feedbackId, managerComment);
        }

        public void UpdateFeedbackStatus(int feedbackId, string status)
        {
            _repository.UpdateFeedbackStatus(feedbackId, status);
        }

        public void DeleteFeedback(int feedbackId)
        {
            _repository.DeleteFeedback(feedbackId);
        }
    }



    // Feedback Retrieval Implementation
    public class FeedbackRetrieval : IFeedbackRetrieval
    {
        private readonly FeedbackRepository _repository;

        public FeedbackRetrieval(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public FeedbackRDM GetFeedbackById(int feedbackId)
        {
            var feedback = _repository.GetFeedbackById(feedbackId);
            return feedback ?? new FeedbackRDM { Feedback = "Feedback Not Found" };
        }

        public List<FeedbackRDM> GetAllFeedback()
        {
            return _repository.GetAllFeedback();
        }
    }


    public class FeedbackSubmission : IFeedbackSubmission
    {
        private readonly FeedbackRepository _repository;

        public FeedbackSubmission(FeedbackRepository repository)
        {
            _repository = repository;
        }

        public void AddFeedback(int staffId, string feedback)
        {
            _repository.AddFeedback(staffId, feedback);
        }

        public void EditFeedback(int feedbackId, string feedback)
        {
            _repository.EditFeedback(feedbackId, feedback);
        }
    }



}
