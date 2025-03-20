using System;
using CleanBrilliantCompany.Interfaces;
using Microsoft.Data.SqlClient; // Updated to the latest SQL Client library
using System.Collections.Generic;

namespace CleanBrilliantCompany.Models
{

    public class FeedbackRDM
    {

        // Private fields
        private int feedbackId;
        private int staffId;
        private string status;
        private DateTime dateSubmitted;
        private string feedback;
        private string managerComments;

        public string StaffName { get; set; }

        // Public getter and setter methods
        public int GetFeedbackId()
        {
            return feedbackId;
        }

        public void SetFeedbackId(int id)
        {
            feedbackId = id;
        }

        public string GetStatus()
        {
            return status;
        }

        public void SetStatus(string status)
        {
            this.status = status;
        }

        public DateTime GetDateSubmitted()
        {
            return dateSubmitted;
        }

        public void SetDateSubmitted(DateTime dateSubmitted)
        {
            this.dateSubmitted = dateSubmitted;
        }

        public string GetFeedback()
        {
            return feedback;
        }

        public void SetFeedback(string feedback)
        {
            this.feedback = feedback;
        }

        public string GetManagerComments()
        {
            return managerComments;
        }

        public void SetManagerComments(string managerComments)
        {
            this.managerComments = managerComments;
        }

        public object FeedbackDetails(string field, object value)
        {
            switch (field.ToLower())
            {
                case "feedbackid":
                    feedbackId = Convert.ToInt32(value);
                    return feedbackId;
                case "staffid":
                    staffId = Convert.ToInt32(value);
                    return staffId;
                case "status":
                    status = value.ToString();
                    return status;
                case "datesubmitted":
                    dateSubmitted = Convert.ToDateTime(value);
                    return dateSubmitted;
                case "feedback":
                    feedback = value.ToString();
                    return feedback;
                case "managercomments":
                    managerComments = value.ToString();
                    return managerComments;
                default:
                    return null;
            }
        }

        public override string ToString()
        {
            return $"FeedbackRDM [feedbackId={feedbackId}, staffId={staffId}, staffName={StaffName}, status={status}, dateSubmitted={dateSubmitted}, feedback={feedback}, managerComments={managerComments}]";
        }
    }

    public class FeedbackRepository
    {
        private readonly string connectionString = "Server=tcp:inf2112.database.windows.net,1433;Initial Catalog=CleanBrilliantCompany;Persist Security Info=False;User ID=teammember;Password=RevacholInsulid141;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int GetDefaultStaffId()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Select the first available staffId
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Check if the staffId exists in GeneralStaff table
                string checkQuery = "SELECT COUNT(*) FROM dbo.GeneralStaff WHERE staffId = @StaffID";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@StaffID", staffId);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count == 0) // If staffId does not exist, return without inserting
                    {
                        throw new Exception("Invalid staffId. The staff member does not exist.");
                    }
                }

                // If staffId is valid, insert feedback
                string query = "INSERT INTO dbo.Feedback (StaffID, FeedbackText, DateSubmitted, Status) VALUES (@StaffID, @FeedbackText, GETDATE(), 'Pending')";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StaffID", staffId);
                    cmd.Parameters.AddWithValue("@FeedbackText", feedback);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public List<string> GetFeedbackList()
        {
            List<string> feedbackList = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
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
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE dbo.Feedback SET FeedbackText = @FeedbackText WHERE FeedbackID = @FeedbackID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FeedbackText", feedback);
                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // public void DeleteFeedback(int feedbackId)
        // {
        //     using (SqlConnection conn = new SqlConnection(connectionString))
        //     {
        //         string query = "DELETE FROM dbo.Feedback WHERE FeedbackID = @FeedbackID";

        //         using (SqlCommand cmd = new SqlCommand(query, conn))
        //         {
        //             cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);

        //             conn.Open();
        //             cmd.ExecuteNonQuery();
        //         }
        //     }
        // }
        public List<FeedbackRDM> GetFeedbackWithDetails()
        {
            List<FeedbackRDM> feedbackList = new List<FeedbackRDM>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT f.FeedbackID, s.name AS StaffName, f.FeedbackText, f.Status
                    FROM dbo.Feedback f
                    JOIN dbo.Staff s ON f.StaffID = s.staffId
                    ORDER BY f.DateSubmitted DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FeedbackRDM feedback = new FeedbackRDM
                            {
                                StaffName = reader.GetString(1),  // Staff Name
                            };
                            feedback.SetFeedbackId(reader.GetInt32(0)); // Feedback ID
                            feedback.SetFeedback(reader.GetString(2));  // Feedback Text
                            feedback.SetStatus(reader.GetString(3));    // Feedback Status

                            feedbackList.Add(feedback);
                        }
                    }
                }
            }
            return feedbackList;
        }

        public FeedbackRDM GetFeedbackById(int feedbackId)
        {
            FeedbackRDM feedback = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FeedbackID, StaffID, FeedbackText, Status FROM dbo.Feedback WHERE FeedbackID = @FeedbackID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            feedback = new FeedbackRDM();
                            feedback.SetFeedbackId(reader.GetInt32(0));
                            feedback.SetFeedback(reader.GetString(2));
                            feedback.SetStatus(reader.GetString(3));
                        }
                    }
                }
            }
            return feedback;
        }

        public void UpdateFeedback(int feedbackId, string updatedFeedback)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE dbo.Feedback SET FeedbackText = @FeedbackText WHERE FeedbackID = @FeedbackID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FeedbackText", updatedFeedback);
                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<FeedbackRDM> GetAllFeedback()
        {
            List<FeedbackRDM> feedbackList = new List<FeedbackRDM>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
            SELECT f.FeedbackID, s.name AS StaffName, f.FeedbackText, f.Status, f.ManagerComments
            FROM dbo.Feedback f
            JOIN dbo.Staff s ON f.StaffID = s.staffId
            ORDER BY f.DateSubmitted DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FeedbackRDM feedback = new FeedbackRDM();
                            feedback.SetFeedbackId(reader.GetInt32(0));
                            feedback.StaffName = reader.GetString(1);
                            feedback.SetFeedback(reader.GetString(2));
                            feedback.SetStatus(reader.GetString(3));
                            feedback.SetManagerComments(reader.IsDBNull(4) ? "" : reader.GetString(4));

                            feedbackList.Add(feedback);
                        }
                    }
                }
            }
            return feedbackList;
        }

        // Update manager's comments
        public void UpdateManagerComment(int feedbackId, string managerComment)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE dbo.Feedback SET ManagerComments = @ManagerComment WHERE FeedbackID = @FeedbackID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ManagerComment", managerComment);
                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Update feedback status
        public void UpdateFeedbackStatus(int feedbackId, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE dbo.Feedback SET Status = @Status WHERE FeedbackID = @FeedbackID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // Delete feedback
        public void DeleteFeedback(int feedbackId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM dbo.Feedback WHERE FeedbackID = @FeedbackID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }



    }

    // Feedback Management Implementation
    public class FeedbackManagement : IFeedbackManagement
    {
        private readonly FeedbackRepository repository;

        public FeedbackManagement(FeedbackRepository repo)
        {
            this.repository = repo;
        }

        public void DeleteFeedback()
        {
            var feedbackList = repository.GetFeedbackList();
            if (feedbackList.Count > 0)
                feedbackList.RemoveAt(0);
        }

        public bool ResolveFeedback(int feedbackId)
        {
            var feedbackList = repository.GetFeedbackList();
            return feedbackId < feedbackList.Count;
        }
    }


    // Feedback Retrieval Implementation
    public class FeedbackRetrieval : IFeedbackRetrieval
    {
        private readonly FeedbackRepository repository;

        public FeedbackRetrieval(FeedbackRepository repo)
        {
            this.repository = repo;
        }

        public string GetFeedbackById(int feedbackId)
        {
            var feedbackList = repository.GetFeedbackList();
            return feedbackId < feedbackList.Count ? feedbackList[feedbackId] : "Feedback Not Found";
        }

        public List<string> GetAllFeedback()
        {
            return repository.GetFeedbackList();
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
