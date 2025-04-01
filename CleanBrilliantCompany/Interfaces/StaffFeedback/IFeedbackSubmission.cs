



using System;
using CleanBrilliantCompany.Models;

namespace CleanBrilliantCompany.Interfaces
{
    public interface IFeedbackSubmission
    {
        void AddFeedback(int staffId, string feedback);
        void EditFeedback(int feedbackId, string feedback);
    }

    

}