using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedbackManagement
    {
        void DeleteFeedback();
        bool ResolveFeedback(int feedbackId);
    }

    
}