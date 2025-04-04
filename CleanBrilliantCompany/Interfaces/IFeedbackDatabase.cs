using CleanBrilliantCompany.Interfaces;
using CleanBrilliantCompany.Models;
using Microsoft.Data.SqlClient;
public interface IFeedbackDatabase
{
    void AddFeedback(int staffId, string feedback);
    void EditFeedback(int feedbackId, string feedback);
    FeedbackRDM GetFeedbackById(int feedbackId);
    List<FeedbackRDM> GetAllFeedback();
    void UpdateManagerComment(int feedbackId, string managerComment);
    void UpdateFeedbackStatus(int feedbackId, string status);
    void DeleteFeedback(int feedbackId);
}
