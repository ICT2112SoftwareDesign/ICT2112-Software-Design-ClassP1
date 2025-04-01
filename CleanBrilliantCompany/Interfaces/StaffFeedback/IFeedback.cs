using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedback
    {
        bool ValidateFeedback(string feedback);
        bool FeedbackExists(int feedbackId);
    }
}