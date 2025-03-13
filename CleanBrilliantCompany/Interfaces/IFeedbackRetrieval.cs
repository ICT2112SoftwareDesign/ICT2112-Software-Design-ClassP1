using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedbackRetrieval
    {
        string GetFeedbackById(int feedbackId);
        List<string> GetAllFeedback();
    }

    
}