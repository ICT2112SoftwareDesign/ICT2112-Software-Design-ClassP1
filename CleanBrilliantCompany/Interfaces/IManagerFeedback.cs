using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    // Interface for Manager Feedback
    public interface IManagerFeedback
    {
        string GetFeedbackById(int feedbackId);
        List<string> GetAllFeedback();
        void DeleteFeedback();
        bool ResolveFeedback(int feedbackId);
    }


}