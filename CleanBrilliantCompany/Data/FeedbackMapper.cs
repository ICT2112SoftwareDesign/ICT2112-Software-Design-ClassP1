using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

public static class FeedbackMapper
{
    public static FeedbackRDM Map(SqlDataReader reader)
    {
        return new FeedbackRDM
        {
            FeedbackId = reader.GetInt32(0),
            StaffId = reader.GetInt32(1),
            StaffName = reader.GetString(2),
            Feedback = reader.GetString(3),
            Status = reader.GetString(4),
            ManagerComments = reader.GetString(5),
            DateSubmitted = reader.GetDateTime(6)
        };
    }

    public static List<FeedbackRDM> MapList(SqlDataReader reader)
    {
        var feedbackList = new List<FeedbackRDM>();
        while (reader.Read())
        {
            feedbackList.Add(Map(reader));
        }
        return feedbackList;
    }

    public static SqlCommand CreateInsertCommand(SqlConnection conn, int staffId, string feedback)
    {
        var cmd = new SqlCommand("INSERT INTO dbo.Feedback (StaffID, FeedbackText, DateSubmitted, Status) VALUES (@StaffID, @FeedbackText, GETDATE(), 'Pending')", conn);
        cmd.Parameters.AddWithValue("@StaffID", staffId);
        cmd.Parameters.AddWithValue("@FeedbackText", feedback);
        return cmd;
    }

    public static SqlCommand CreateUpdateFeedbackCommand(SqlConnection conn, int feedbackId, string feedback)
    {
        var cmd = new SqlCommand("UPDATE dbo.Feedback SET FeedbackText = @FeedbackText WHERE FeedbackID = @FeedbackID", conn);
        cmd.Parameters.AddWithValue("@FeedbackText", feedback);
        cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
        return cmd;
    }

    public static SqlCommand CreateUpdateManagerCommentCommand(SqlConnection conn, int feedbackId, string managerComment)
    {
        var cmd = new SqlCommand("UPDATE dbo.Feedback SET ManagerComments = @ManagerComment WHERE FeedbackID = @FeedbackID", conn);
        cmd.Parameters.AddWithValue("@ManagerComment", managerComment);
        cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
        return cmd;
    }

    public static SqlCommand CreateUpdateStatusCommand(SqlConnection conn, int feedbackId, string status)
    {
        var cmd = new SqlCommand("UPDATE dbo.Feedback SET Status = @Status WHERE FeedbackID = @FeedbackID", conn);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
        return cmd;
    }

    public static SqlCommand CreateDeleteCommand(SqlConnection conn, int feedbackId)
    {
        var cmd = new SqlCommand("DELETE FROM dbo.Feedback WHERE FeedbackID = @FeedbackID", conn);
        cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
        return cmd;
    }

    public static SqlCommand CreateGetFeedbackByIdCommand(SqlConnection conn, int feedbackId)
    {
        var cmd = new SqlCommand(@"
            SELECT f.FeedbackID, f.StaffID, s.name AS StaffName, f.FeedbackText, f.Status,
                   ISNULL(f.ManagerComments, '') AS ManagerComments, f.DateSubmitted
            FROM dbo.Feedback f
            JOIN dbo.Staff s ON f.StaffID = s.staffId
            WHERE f.FeedbackID = @FeedbackID", conn);
        cmd.Parameters.AddWithValue("@FeedbackID", feedbackId);
        return cmd;
    }

    public static SqlCommand CreateGetAllFeedbackCommand(SqlConnection conn)
    {
        return new SqlCommand(@"
            SELECT f.FeedbackID, f.StaffID, s.name AS StaffName, f.FeedbackText, f.Status,
                   ISNULL(f.ManagerComments, '') AS ManagerComments, f.DateSubmitted
            FROM dbo.Feedback f
            JOIN dbo.Staff s ON f.StaffID = s.staffId
            ORDER BY f.DateSubmitted DESC", conn);
    }
} 